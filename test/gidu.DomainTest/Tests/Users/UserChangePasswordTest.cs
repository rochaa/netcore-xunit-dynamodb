using System.Threading.Tasks;
using gidu.Domain.Core.Users;
using gidu.Domain.Helpers;
using gidu.DomainTest.Builders;
using gidu.DomainTest.Mocks;
using gidu.DomainTest.Utils;
using Xunit;

namespace gidu.DomainTest.Tests.Users
{
    public class UserChangePasswordTest
    {
        private readonly UserMocks _userMocks;
        private readonly UserChangePassword _userChangePassword;
        private readonly UserRegisterDto _userRegisterDto;
        private readonly string _userLog;

        public UserChangePasswordTest()
        {
            GlobalParameters.KeyPassword = "NTFkVTIwMTlqag==";

            _userMocks = new UserMocks();
            _userChangePassword = new UserChangePassword(_userMocks.UserRepository.Object);
            _userRegisterDto = UserBuilder.New().BuildRegisterDto();
            _userLog = "Admin";
        }

        [Fact]
        public async Task MustChangeUserPasswordAsync()
        {
            // Given
            var userId = "123YWZ";
            var userAlreadyExists = UserBuilder.New().WithId(userId).Build();
            var oldPasswordEncrypt = userAlreadyExists.Password;

            // When
            _userMocks.MockUserGetById(userAlreadyExists, userId);
            await _userChangePassword.UpdateAsync(userId, _userRegisterDto.Password, _userLog);

            // Then
            Assert.NotEqual(userAlreadyExists.Password, oldPasswordEncrypt);
        }

        [Fact]
        public async Task MustNotifiedIfUserNotFoundAsync()
        {
            // Given
            var userId = "456MNO";
            User userNotFound = null;

            // When
            _userMocks.MockUserGetById(userNotFound, userId);

            // Then
            (await Assert.ThrowsAsync<DomainException>(async () =>
                await _userChangePassword.UpdateAsync(userId, _userRegisterDto.Password, _userLog)))
            .WithMessage(UserRegister.UserNotFound);
        }
    }
}