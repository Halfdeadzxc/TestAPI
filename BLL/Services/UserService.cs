using BLL.DTO;
using BLL.Services;
using BLL.ServicesInterfaces;
using DAL.Interfaces;
using DAL.Models;
using System.Security.Claims;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly JwtService _jwtService;
    private readonly PasswordService _passwordService;
    public UserService(IUserRepository userRepository, JwtService jwtService, PasswordService passwordService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
        _passwordService = passwordService;
    }

    public async Task<TokenResponseDTO> AuthenticateAsync(string username, string password, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var user = await _userRepository.GetUserByUsernameAsync(username, cancellationToken);
        if (user == null || !_passwordService.VerifyPassword(password, user.PasswordHash, cancellationToken))
        {
            throw new UnauthorizedAccessException("Invalid credentials");
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var accessToken = _jwtService.GenerateAccessToken(claims, 15, cancellationToken);
        var refreshToken = _jwtService.GenerateRefreshToken(user.Id, cancellationToken);
        await _userRepository.AddRefreshTokenAsync(refreshToken, cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        return new TokenResponseDTO
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken.Token
        };
    }

    public async Task<string> RefreshAccessTokenAsync(string refreshToken, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var storedToken = await _userRepository.GetRefreshTokenAsync(refreshToken, cancellationToken);
        if (storedToken == null || storedToken.ExpiryDate <= DateTime.UtcNow)
        {
            throw new UnauthorizedAccessException("Invalid or expired refresh token.");
        }

        var user = await _userRepository.GetUserByIdAsync(storedToken.UserId, cancellationToken);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role)
        };

        return _jwtService.GenerateAccessToken(claims, 15, cancellationToken);
    }
    public async Task<UserDTO> CreateUserAsync(UserDTO userDto, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var existingUser = await _userRepository.GetUserByUsernameAsync(userDto.Username, cancellationToken);
        if (existingUser != null)
        {
            throw new ArgumentException("A user with the same username already exists.");
        }

        var hashedPassword = _passwordService.HashPassword(userDto.PasswordHash, cancellationToken);

        var user = new User
        {
            Id = userDto.Id,
            Username = userDto.Username,
            PasswordHash = hashedPassword,
            Role = userDto.Role
        };

        await _userRepository.AddUserAsync(user, cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        return new UserDTO
        {
            Id = user.Id,
            Username = user.Username,
            Role = user.Role,
            PasswordHash= hashedPassword
        };
    }
}
