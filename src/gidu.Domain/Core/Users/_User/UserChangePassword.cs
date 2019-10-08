using System.Threading.Tasks;
using gidu.Domain.Helpers;

namespace gidu.Domain.Core.Users
{
    public class UserChangePassword
    {
        public static string UserNotFound = "Usuário não encontrado";
        private readonly IUserRepository _userRepository;

        public UserChangePassword(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task UpdateAsync(string id, string newPassword, string userLog)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
                throw new DomainException("UserChangePassword.UpdateAsync", UserNotFound);

            user.UpdatePassword(newPassword, userLog);
            await _userRepository.SaveAsync(user);
        }
    }
}