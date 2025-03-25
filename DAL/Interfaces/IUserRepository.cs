using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetUserByUsernameAsync(string username, CancellationToken cancellationToken);
        Task<User?> GetUserByIdAsync(int id, CancellationToken cancellationToken);
        Task AddUserAsync(User user, CancellationToken cancellationToken);
        Task<RefreshToken?> GetRefreshTokenAsync(string token, CancellationToken cancellationToken);
        Task AddRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken);
    }

}
