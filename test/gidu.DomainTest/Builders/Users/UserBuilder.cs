using Bogus;
using gidu.Domain.Core.Users;

namespace gidu.DomainTest.Builders
{
    public class UserBuilder
    {
        protected string Id;
        protected string Name;
        protected string Email;
        protected string Password;
        protected string Permission;
        protected string Status;
        protected string RefreshToken;

        public static UserBuilder New()
        {
            var faker = new Faker("pt_BR");

            return new UserBuilder()
            {
                Name = faker.Person.FullName,
                Email = faker.Person.Email,
                Password = faker.Random.AlphaNumeric(10),
                Permission = UserPermission.ADMINISTRATOR,
                Status = UserStatus.ACTIVE,
                RefreshToken = faker.Random.AlphaNumeric(20)
            };
        }

        public UserBuilder WithName(string name)
        {
            Name = name;
            return this;
        }

        public UserBuilder WithEmail(string email)
        {
            Email = email;
            return this;
        }

        public UserBuilder WithPassword(string password)
        {
            Password = password;
            return this;
        }

        public UserBuilder WithPermission(string permission)
        {
            Permission = permission;
            return this;
        }

        public UserBuilder WithId(string id)
        {
            Id = id;
            return this;
        }

        public User Build()
        {
            var user = new User(Name, Email, Password, Permission, "Admin");
            if (!string.IsNullOrEmpty(Id))
                user.Id = Id;

            return user;
        }

        public UserRegisterDto BuildRegisterDto()
        {
            return new UserRegisterDto
            {
                Name = Name,
                Email = Email,
                Password = Password,
                Permission = Permission
            };
        }
    }
}