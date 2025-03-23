using BLL.DTO;
using System.Threading;
using System.Threading.Tasks;

namespace BLL.ServicesInterfaces
{
    public interface IUserService
    {
        Task<UserDTO?> AuthenticateUserAsync(string username, string password, CancellationToken cancellationToken = default);

        Task<RefreshTokenDTO> GenerateRefreshTokenAsync(int userId, CancellationToken cancellationToken = default);

        Task<bool> ValidateRefreshTokenAsync(string token, CancellationToken cancellationToken = default);

        Task<UserDTO?> GetUserByRefreshTokenAsync(string token, CancellationToken cancellationToken = default);

        Task<UserDTO?> GetUserByUsernameAsync(string username, CancellationToken cancellationToken = default);

        Task<UserDTO> CreateUserAsync(UserDTO userDto, CancellationToken cancellationToken = default);
    }
}
