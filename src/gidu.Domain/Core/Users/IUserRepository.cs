using System.Threading.Tasks;
using gidu.Domain.Helpers;

namespace gidu.Domain.Core.Users
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByEmailAsync(string email);
        Task<User> GetByRefreshTokenAsync(string refreshToken);
    }
}