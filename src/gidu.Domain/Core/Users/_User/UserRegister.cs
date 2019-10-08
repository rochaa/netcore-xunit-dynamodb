using System.Threading.Tasks;
using gidu.Domain.Helpers;

namespace gidu.Domain.Core.Users
{
    public class UserRegister
    {
        public static string EmailAlreadyExists = "Email já cadastrado";
        public static string UserNotFound = "Usuário não encontrado";
        public static string UserInactive = "Usuário está inativo";
        private readonly IUserRepository _userRepository;

        public UserRegister(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> StoreAsync(string id, string name, string email, string password, string permission, string userLog)
        {
            User user = null;
            if (string.IsNullOrEmpty(id))
            {
                user = new User(name, email, password, permission, userLog);
                await CheckEmailExists(email);
            }
            else
            {
                user = await _userRepository.GetByIdAsync(id);

                if (user == null)
                    throw new DomainException("UserRegister.DeleteAsync", UserNotFound);

                if (user.Status == UserStatus.INACTIVE)
                    throw new DomainException("UserRegister.StoreAsync", UserInactive);

                if (user.Email != email)
                    await CheckEmailExists(email);

                user.Update(name, email, permission, userLog);
            }

            await _userRepository.SaveAsync(user);
            return user;
        }

        public async Task DeleteAsync(string id, string userLog)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
                throw new DomainException("UserRegister.DeleteAsync", UserNotFound);
            else
                user.Delete(userLog);

            await _userRepository.SaveAsync(user);
        }

        private async Task CheckEmailExists(string email)
        {
            var userWithEmailAlreadyExists = await _userRepository.GetByEmailAsync(email);
            if (userWithEmailAlreadyExists != null)
                throw new DomainException("UserRegister.StoreAsync", EmailAlreadyExists);
        }
    }
}