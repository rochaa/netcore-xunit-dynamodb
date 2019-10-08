using System.Threading.Tasks;
using gidu.Domain.Core.Users;
using gidu.Domain.Helpers;
using gidu.DomainTest.Builders;
using gidu.DomainTest.Mocks;
using gidu.DomainTest.Utils;
using Moq;
using Xunit;

namespace gidu.DomainTest.Tests.Users
{
    public class UserRegisterTest
    {
        private readonly UserMocks _userMocks;
        private readonly UserRegister _userRegister;
        private readonly UserRegisterDto _userRegisterDto;
        private readonly string _userLog;

        public UserRegisterTest()
        {
            GlobalParameters.KeyPassword = "NTFkVTIwMTlqag==";

            _userMocks = new UserMocks();
            _userRegister = new UserRegister(_userMocks.UserRepository.Object);
            _userRegisterDto = UserBuilder.New().BuildRegisterDto();
            _userLog = "Admin";
        }

        [Fact]
        public async Task MustInsertANewUserAsync()
        {
            // Given
            var id = string.Empty;
            User userNotFound = null;

            // When
            _userMocks.MockUserGetByEmail(userNotFound, _userRegisterDto.Email);
            await _userRegister.StoreAsync(id, _userRegisterDto.Name, _userRegisterDto.Email,
                _userRegisterDto.Password, _userRegisterDto.Permission, _userLog);

            // Then
            _userMocks.UserRepository.Verify(r => r.SaveAsync(It.Is<User>(
                u => u.Name == _userRegisterDto.Name &&
                     u.Id == null)));
        }

        [Fact]
        public async Task MustNotifiedEmailAlreadyExistsInInsertAsync()
        {
            // Given
            var id = string.Empty;
            User userFound = UserBuilder.New().WithEmail(_userRegisterDto.Email).Build();

            // When
            _userMocks.MockUserGetByEmail(userFound, _userRegisterDto.Email);

            // Then
            (await Assert.ThrowsAsync<DomainException>(() =>
                _userRegister.StoreAsync(id, _userRegisterDto.Name, _userRegisterDto.Email,
                    _userRegisterDto.Password, _userRegisterDto.Permission, _userLog)))
            .WithMessage(UserRegister.EmailAlreadyExists);
        }

        [Fact]
        public async Task MustUpdateUserAsync()
        {
            // Given
            var id = "123ABC";
            User userToUpdate = UserBuilder.New().WithId(id).Build();
            User userNotFound = null;

            // When
            _userMocks.MockUserGetById(userToUpdate, id);
            _userMocks.MockUserGetByEmail(userNotFound, _userRegisterDto.Email);
            await _userRegister.StoreAsync(id, _userRegisterDto.Name, _userRegisterDto.Email,
                _userRegisterDto.Password, _userRegisterDto.Permission, _userLog);

            // Then
            _userMocks.UserRepository.Verify(r => r.SaveAsync(It.Is<User>(
                u => u.Name == _userRegisterDto.Name &&
                     u.Id == id)));
        }

        [Fact]
        public async Task MustNotifiedUserNotFoundInUpdateAsync()
        {
            // Given
            var id = "789GHI";
            User userToUpdate = null;

            // When
            _userMocks.MockUserGetById(userToUpdate, id);

            // Then
            (await Assert.ThrowsAsync<DomainException>(() =>
                _userRegister.StoreAsync(id, _userRegisterDto.Name, _userRegisterDto.Email,
                    _userRegisterDto.Password, _userRegisterDto.Permission, _userLog)))
            .WithMessage(UserRegister.UserNotFound);
        }

        [Fact]
        public async Task MustNotifiedUserIsInactiveInUpdateAsync()
        {
            // Given
            var id = "456DEF";
            var userToUpdate = UserBuilder.New()
                .WithId(id)
                .WithEmail(_userRegisterDto.Email)
                .Build();

            // When
            userToUpdate.Delete(_userLog);
            _userMocks.MockUserGetById(userToUpdate, id);

            // Then
            (await Assert.ThrowsAsync<DomainException>(() =>
                _userRegister.StoreAsync(id, _userRegisterDto.Name, _userRegisterDto.Email,
                    _userRegisterDto.Password, _userRegisterDto.Permission, _userLog)))
            .WithMessage(UserRegister.UserInactive);
        }

        [Fact]
        public async Task MustNotifiedEmailAlreadyExistsInUpdateAsync()
        {
            // Given
            var id = "012JLM";
            var userToUpdate = UserBuilder.New().Build();
            var userFound = UserBuilder.New().WithEmail(_userRegisterDto.Email).Build();

            // When
            _userMocks.MockUserGetById(userToUpdate, id);
            _userMocks.MockUserGetByEmail(userFound, _userRegisterDto.Email);

            // Then
            (await Assert.ThrowsAsync<DomainException>(() =>
                _userRegister.StoreAsync(id, _userRegisterDto.Name, _userRegisterDto.Email,
                    _userRegisterDto.Password, _userRegisterDto.Permission, _userLog)))
            .WithMessage(UserRegister.EmailAlreadyExists);
        }

        [Fact]
        public async Task MustToInactiveUserAsync()
        {
            // Given
            var id = "NOP345";
            var userExists = UserBuilder.New().WithId(id).Build();

            // When
            _userMocks.MockUserGetById(userExists, id);
            await _userRegister.DeleteAsync(id, _userLog);

            // Then
            _userMocks.UserRepository.Verify(r => r.SaveAsync(It.Is<User>(
                u => u.Id == id &&
                     u.Status == UserStatus.INACTIVE)));
        }

        [Fact]
        public async Task MustNotifiedUserNotFoundInRemoveAsync()
        {
            // Given
            var id = "NOP345";
            User userNotFound = null;

            // When
            _userMocks.MockUserGetById(userNotFound, id);

            // Then
            (await Assert.ThrowsAsync<DomainException>(() =>
                _userRegister.DeleteAsync(id, _userLog)))
            .WithMessage(UserRegister.UserNotFound);
        }
    }
}