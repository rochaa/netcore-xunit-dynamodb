using System.Threading.Tasks;
using gidu.Domain.Core.Users;
using gidu.Domain.Core.Users.Login;
using gidu.Domain.Helpers;
using gidu.DomainTest.Builders;
using gidu.DomainTest.Mocks;
using gidu.DomainTest.Utils;
using Xunit;

namespace gidu.DomainTest.Tests.Users
{
    public class UserLoginTest
    {
        private readonly UserMocks _userMocks;
        private readonly UserAuthentication _userAuthentication;
        private readonly UserLoginDto _userLoginDto;

        public UserLoginTest()
        {
            GlobalParameters.KeyPassword = "NTFkVTIwMTlqag==";

            _userMocks = new UserMocks();
            _userAuthentication = new UserAuthentication(_userMocks.UserRepository.Object);
            _userLoginDto = LoginBuilder.New().Build();
        }

        [Fact]
        public async Task MustLoginSuccessfullyAsync()
        {
            // Given
            var existingUser = UserBuilder.New()
                .WithEmail(_userLoginDto.Email)
                .WithPassword(_userLoginDto.Password)
                .Build();

            // When
            _userMocks.MockUserGetByEmail(existingUser, _userLoginDto.Email);
            var userAuthenticated = await _userAuthentication.AuthenticateAsync(_userLoginDto);

            // Then
            Assert.Equal(existingUser.Email, userAuthenticated.Email);
        }

        [Fact]
        public async Task MustNotifiedWhenUserNotFoundAsync()
        {
            // Given
            User userNotFound = null;

            // When
            _userMocks.MockUserGetByEmail(userNotFound, _userLoginDto.Email);

            // Then
            (await Assert.ThrowsAsync<DomainException>(() =>
                _userAuthentication.AuthenticateAsync(_userLoginDto)))
            .WithMessage(UserAuthentication.UserNotFound);
        }

        [Fact]
        public async Task MustNotifiedWhenPasswordIsWrongAsync()
        {
            // Given
            var userWithPasswordIsWrong = UserBuilder.New()
                .WithEmail(_userLoginDto.Email)
                .Build();

            // When
            _userMocks.MockUserGetByEmail(userWithPasswordIsWrong, _userLoginDto.Email);

            // Then
            (await Assert.ThrowsAsync<DomainException>(() =>
                _userAuthentication.AuthenticateAsync(_userLoginDto)))
            .WithMessage(UserAuthentication.WrongPassword);
        }

        [Fact]
        public async Task MustNotifiedWhenUserIsInactiveAsync()
        {
            // Given
            var userInactive = UserBuilder.New()
                .WithEmail(_userLoginDto.Email)
                .WithPassword(_userLoginDto.Password)
                .Build();

            // When
            userInactive.Delete("Admin");
            _userMocks.MockUserGetByEmail(userInactive, _userLoginDto.Email);

            // Then
            (await Assert.ThrowsAsync<DomainException>(() =>
                _userAuthentication.AuthenticateAsync(_userLoginDto)))
            .WithMessage(UserAuthentication.UserInactive);
        }

        [Fact]
        public async Task MustGetNewRefreshTokenAsync()
        {
            // Given
            var existingUser = UserBuilder.New()
                .Build();
            _userLoginDto.Grant_Type = UserLoginGrantType.RefreshToken;

            // When
            _userMocks.MockUserGetByRefreshToken(existingUser, _userLoginDto.Refresh_Token);
            existingUser = await _userAuthentication.AuthenticateAsync(_userLoginDto);

            // Then
            Assert.NotEqual(_userLoginDto.Refresh_Token, existingUser.RefreshToken);
        }

        [Fact]
        public async Task MustNotifiedWhenRefreshTokenNotFoundAsync()
        {
            // Given
            User userWithRefreshTokenNotFound = null;
            _userLoginDto.Grant_Type = UserLoginGrantType.RefreshToken;

            // When
            _userMocks.MockUserGetByRefreshToken(userWithRefreshTokenNotFound, _userLoginDto.Refresh_Token);

            // Then
            (await Assert.ThrowsAsync<DomainException>(() =>
                _userAuthentication.AuthenticateAsync(_userLoginDto)))
            .WithMessage(UserAuthentication.RefreshTokenNotFound);
        }

        [Fact]
        public async Task MustNotifiedWhenGrantTypeInvalidAsync()
        {
            // Given
            _userLoginDto.Grant_Type = "invalid";

            // Then
            (await Assert.ThrowsAsync<DomainException>(() =>
                _userAuthentication.AuthenticateAsync(_userLoginDto)))
            .WithMessage(UserAuthentication.GrantTypeInvalid);
        }
    }
}