using BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ServicesInterfaces
{
    public interface IUserService
    {
        Task<UserDTO?> AuthenticateUserAsync(string username, string password);
        Task<RefreshTokenDTO> GenerateRefreshTokenAsync(int userId);
        Task<bool> ValidateRefreshTokenAsync(string token);
        Task<UserDTO?> GetUserByRefreshTokenAsync(string token);
        Task<UserDTO?> GetUserByUsernameAsync(string username);

    }
}
