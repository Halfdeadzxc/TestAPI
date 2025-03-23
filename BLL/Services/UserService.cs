using AutoMapper;
using BLL.DTO;
using BLL.ServicesInterfaces;
using BLL.Validators;
using DAL.Interfaces;
using DAL.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtService _jwtService;
        private readonly IMapper _mapper;
        private readonly IValidator<UserDTO> _userDTOValidator;
        private readonly PasswordService _passwordService;
        public UserService(IUserRepository userRepository, JwtService jwtService, PasswordService passwordService, IMapper mapper, IValidator<UserDTO> userDtoValidator)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _mapper = mapper;
            _passwordService = passwordService;
            _userDTOValidator = userDtoValidator;
        }

        public async Task<UserDTO?> AuthenticateUserAsync(string username, string password, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = await _userRepository.GetUserByUsernameAsync(username);
            if (user == null || !_passwordService.VerifyPassword(password, user.PasswordHash))
            {
                return null;
            }

            cancellationToken.ThrowIfCancellationRequested();

            return _mapper.Map<UserDTO>(user);
        }

        public async Task<RefreshTokenDTO> GenerateRefreshTokenAsync(int userId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var refreshToken = _jwtService.GenerateRefreshToken(userId, cancellationToken);
            await _userRepository.AddRefreshTokenAsync(_mapper.Map<RefreshToken>(refreshToken));

            cancellationToken.ThrowIfCancellationRequested();

            return refreshToken;
        }

        public async Task<bool> ValidateRefreshTokenAsync(string token, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var refreshToken = await _userRepository.GetRefreshTokenAsync(token);
            if (refreshToken == null) return false;

            cancellationToken.ThrowIfCancellationRequested();

            return _jwtService.IsRefreshTokenValid(_mapper.Map<RefreshTokenDTO>(refreshToken), cancellationToken);
        }

        public async Task<UserDTO?> GetUserByRefreshTokenAsync(string token, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var refreshToken = await _userRepository.GetRefreshTokenAsync(token);

            if (refreshToken == null || !_jwtService.IsRefreshTokenValid(_mapper.Map<RefreshTokenDTO>(refreshToken), cancellationToken))
            {
                return null;
            }

            cancellationToken.ThrowIfCancellationRequested();

            var user = await _userRepository.GetUserByIdAsync(refreshToken.UserId);
            return user == null ? null : _mapper.Map<UserDTO>(user);
        }

        public async Task<UserDTO?> GetUserByUsernameAsync(string username, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = await _userRepository.GetUserByUsernameAsync(username);

            cancellationToken.ThrowIfCancellationRequested();

            return user == null ? null : _mapper.Map<UserDTO>(user);
        }

        public async Task<UserDTO> CreateUserAsync(UserDTO userDto, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var validationResult = _userDTOValidator.Validate(userDto);
            if (!validationResult.IsValid)
            {
                var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                throw new ArgumentException($"Validation failed: {errors}");
            }

            var existingUser = await _userRepository.GetUserByUsernameAsync(userDto.Username);
            if (existingUser != null)
            {
                throw new ArgumentException("A user with the same username already exists.");
            }

            cancellationToken.ThrowIfCancellationRequested();

            var user = _mapper.Map<User>(userDto);
            user.PasswordHash = _passwordService.HashPassword(userDto.PasswordHash);

            await _userRepository.AddUserAsync(user);

            cancellationToken.ThrowIfCancellationRequested();

            return _mapper.Map<UserDTO>(user);
        }

    }
}
