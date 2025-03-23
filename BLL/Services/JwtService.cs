using BLL.DTO;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using System.Threading;

namespace BLL.Services
{
    public class JwtService
    {
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly IValidator<RefreshTokenDTO> _refreshTokenValidator;

        public JwtService(IConfiguration configuration, IValidator<RefreshTokenDTO> refreshTokenValidator)
        {
            _secretKey = configuration["Jwt:SecretKey"];
            _issuer = configuration["Jwt:Issuer"];
            _audience = configuration["Jwt:Audience"];
            _refreshTokenValidator = refreshTokenValidator;
        }

        public string GenerateAccessToken(IEnumerable<System.Security.Claims.Claim> claims, int lifetimeMinutes, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(lifetimeMinutes),
                signingCredentials: creds
            );

            cancellationToken.ThrowIfCancellationRequested();

            return new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler().WriteToken(token);
        }

        public RefreshTokenDTO GenerateRefreshToken(int userId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var refreshToken = new RefreshTokenDTO
            {
                Token = Guid.NewGuid().ToString(),
                ExpiryDate = DateTime.UtcNow.AddDays(1)
            };

            var validationResult = _refreshTokenValidator.Validate(refreshToken);
            if (!validationResult.IsValid)
            {
                var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                throw new ArgumentException($"Validation failed: {errors}");
            }

            cancellationToken.ThrowIfCancellationRequested();

            return refreshToken;
        }

        public bool IsRefreshTokenValid(RefreshTokenDTO refreshToken, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var validationResult = _refreshTokenValidator.Validate(refreshToken);
            if (!validationResult.IsValid)
            {
                return false;
            }

            cancellationToken.ThrowIfCancellationRequested();

            return refreshToken.ExpiryDate > DateTime.UtcNow;
        }
    }
}
