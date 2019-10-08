using Bogus;
using gidu.Domain.Core.Users;
using gidu.Domain.Core.Users.Login;

namespace gidu.DomainTest.Builders
{
    public class LoginBuilder
    {
        protected string Email;
        protected string Password;
        protected string Grant_Type;
        protected string Refresh_Token;

        public static LoginBuilder New()
        {
            var faker = new Faker("pt_BR");

            return new LoginBuilder()
            {
                Email = faker.Person.Email,
                Password = faker.Random.AlphaNumeric(10),
                Grant_Type = UserLoginGrantType.Password,
                Refresh_Token = faker.Random.AlphaNumeric(15)
            };
        }

        public LoginBuilder WithGrantType(string grant_type)
        {
            Grant_Type = grant_type;
            return this;
        }

        public UserLoginDto Build()
        {
            return new UserLoginDto()
            {
                Email = Email,
                Password = Password,
                Grant_Type = Grant_Type,
                Refresh_Token = Refresh_Token
            };
        }
    }
}