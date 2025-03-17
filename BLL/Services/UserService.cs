using AutoMapper;
using BLL.DTO;
using BLL.ServicesInterfaces;
using DAL.Interfaces;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtService _jwtService;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, JwtService jwtService, IMapper mapper)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _mapper = mapper;
        }

        public async Task<UserDTO?> AuthenticateUserAsync(string username, string password)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);
            if (user == null || !VerifyPasswordHash(password, user.PasswordHash))
            {
                return null;
            }

            return _mapper.Map<UserDTO>(user);
        }

        public async Task<RefreshTokenDTO> GenerateRefreshTokenAsync(int userId)
        {
            var refreshToken = new RefreshToken
            {
                Token = Guid.NewGuid().ToString(),
                UserId = userId,
                ExpiryDate = DateTime.UtcNow.AddDays(1)
            };

            await _userRepository.AddRefreshTokenAsync(refreshToken);
            return _mapper.Map<RefreshTokenDTO>(refreshToken);
        }

        public async Task<bool> ValidateRefreshTokenAsync(string token)
        {
            var refreshToken = await _userRepository.GetRefreshTokenAsync(token);
            return refreshToken != null && refreshToken.ExpiryDate > DateTime.UtcNow;
        }
        public async Task<UserDTO?> GetUserByRefreshTokenAsync(string token)
        {
            var refreshToken = await _userRepository.GetRefreshTokenAsync(token);
            if (refreshToken == null || refreshToken.ExpiryDate <= DateTime.UtcNow)
            {
                return null;
            }

            var user = await _userRepository.GetUserByIdAsync(refreshToken.UserId);
            return user == null ? null : _mapper.Map<UserDTO>(user);
        }
        public async Task<UserDTO?> GetUserByUsernameAsync(string username)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);
            return user == null ? null : _mapper.Map<UserDTO>(user);
        }

        private bool VerifyPasswordHash(string password, string storedHash)
        {
            
            return password == storedHash; 
        }
    }
}
