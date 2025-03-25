using BLL.DTO;
using System.Threading;
using System.Threading.Tasks;

namespace BLL.ServicesInterfaces
{
    public interface IUserService
    {
        Task<TokenResponseDTO> AuthenticateAsync(string username, string password, CancellationToken cancellationToken);

        Task<string> RefreshAccessTokenAsync(string refreshToken, CancellationToken cancellationToken);
        Task<UserDTO> CreateUserAsync(UserDTO userDto, CancellationToken cancellationToken);
    }
}
