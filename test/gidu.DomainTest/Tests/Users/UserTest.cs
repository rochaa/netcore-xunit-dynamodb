using ExpectedObjects;
using Xunit;
using gidu.Domain.Core.Users;
using gidu.Domain.Helpers;
using gidu.DomainTest.Builders;
using gidu.Domain.Utils;
using gidu.DomainTest.Utils;

namespace gidu.DomainTest.Tests.Users
{
    public class UserTest
    {
        private readonly User _user;

        public UserTest()
        {
            GlobalParameters.KeyPassword = "NTFkVTIwMTlqag==";
            _user = UserBuilder.New().Build();
        }

        [Fact]
        public void MustValidObject()
        {
            // Given
            var userExpected = new
            {
                _user.Name,
                _user.Email,
                _user.Permission,
                _user.Search,
                _user.Status
            };
            var userLog = "Admin";
            var password = Password.Decrypt(_user.Password);

            // When
            User user = new User(userExpected.Name, userExpected.Email, password,
                userExpected.Permission, userLog);

            // Then
            userExpected.ToExpectedObject().ShouldMatch(user);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("Lu")]
        [InlineData("Nome tão tão tão grande que ultrapassa o limite de 60 caracteres")]
        public void MustNotHaveInvalidName(string invalidName)
        {
            // Then
            Assert.Throws<DomainException>(() =>
                UserBuilder.New().WithName(invalidName).Build())
            .WithMessage(UserValidation.NameIsEmpty, UserValidation.NumberOfCharactersName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("email invalido")]
        [InlineData("email@invalido")]
        public void MustNotHaveInvalidEmail(string invalidEmail)
        {
            Assert.Throws<DomainException>(() =>
                UserBuilder.New().WithEmail(invalidEmail).Build())
            .WithMessage(UserValidation.EmailIsEmpty, UserValidation.EmailIsInvalid);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("123")]
        [InlineData("1234567890123456")]
        public void MustNotHaveInvalidPassword(string invalidPassword)
        {
            Assert.Throws<DomainException>(() =>
                UserBuilder.New().WithPassword(invalidPassword).Build())
            .WithMessage(UserValidation.PasswordIsEmpty, UserValidation.NumberOfCharactersPassword);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("MASTER")]
        public void MustNotHaveNonexistentPermission(string nonexistentPermission)
        {
            // Then
            Assert.Throws<DomainException>(() =>
                UserBuilder.New().WithPermission(nonexistentPermission).Build())
            .WithMessage(UserValidation.PermissionIsEmpty, UserValidation.NonexistentPermission);
        }
    }
}