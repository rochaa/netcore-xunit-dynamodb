using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using gidu.Domain.Core.Users.Login;
using gidu.Domain.Helpers;
using gidu.Domain.Utils;

namespace gidu.Domain.Core.Users
{
    public class UserAuthentication
    {
        public static string GrantTypeInvalid = "Grant type inválido";
        public static string UserNotFound = "Usuário não encontrado";
        public static string WrongPassword = "Senha incorreta";
        public static string UserInactive = "Usuário está inativo";
        public static string RefreshTokenNotFound = "Refresh token de usuário não encontrado";
        private readonly IUserRepository _userRepository;

        public UserAuthentication(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> AuthenticateAsync(UserLoginDto userLoginDto)
        {
            User user = null;
            var sourceError = "UserAuthentication.AuthenticateAsync";

            if (userLoginDto.Grant_Type == UserLoginGrantType.Password)
                user = await AuthenticateByEmailAsync(userLoginDto.Email, userLoginDto.Password);
            else if (userLoginDto.Grant_Type == UserLoginGrantType.RefreshToken)
                user = await AuthenticateByRefreshTokenAsync(userLoginDto.Refresh_Token);
            else
                throw new DomainException(sourceError, GrantTypeInvalid);

            user.UpdateRefreshToken();
            await _userRepository.SaveAsync(user);

            return user;
        }

        public List<Claim> CreateRights(User user)
        {
            // Informações pessoais do usuário.
            return new List<Claim> {
                new Claim (ClaimTypes.Name, user.Email),
                new Claim ("Name", user.Name),
                new Claim (ClaimTypes.Role, user.Permission)
            };
        }

        private async Task<User> AuthenticateByEmailAsync(string email, string password)
        {
            var sourceError = "UserAuthentication.AuthenticateByEmailAsync";
            var user = await _userRepository.GetByEmailAsync(email);

            if (user == null)
                throw new DomainException(sourceError, UserNotFound);

            if (user.Status == UserStatus.INACTIVE)
                throw new DomainException(sourceError, UserInactive);

            var passwordDb = Password.Decrypt(user.Password);
            if (passwordDb != password)
                throw new DomainException(sourceError, WrongPassword);

            return user;
        }

        private async Task<User> AuthenticateByRefreshTokenAsync(string refreshToken)
        {
            var sourceError = "UserAuthentication.AuthenticateByRefreshTokenAsync";
            var user = await _userRepository.GetByRefreshTokenAsync(refreshToken);

            if (user == null)
                throw new DomainException(sourceError, RefreshTokenNotFound);

            if (user.Status == UserStatus.INACTIVE)
                throw new DomainException(sourceError, UserInactive);

            return user;
        }
    }
}