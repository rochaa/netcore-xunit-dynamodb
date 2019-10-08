using gidu.Domain.Core.Users;
using Moq;

namespace gidu.DomainTest.Mocks
{
    public class UserMocks
    {
        public readonly Mock<IUserRepository> UserRepository;

        public UserMocks()
        {
            UserRepository = new Mock<IUserRepository>();
        }

        public void MockUserGetById(User userReturned, string id)
        {
            UserRepository
                .Setup(o => o.GetByIdAsync(id))
                .ReturnsAsync(userReturned);
        }

        public void MockUserGetByEmail(User userReturned, string email)
        {
            UserRepository
                .Setup(o => o.GetByEmailAsync(email))
                .ReturnsAsync(userReturned);
        }

        public void MockUserGetByRefreshToken(User userReturned, string refreshToken)
        {
            UserRepository
                .Setup(o => o.GetByRefreshTokenAsync(refreshToken))
                .ReturnsAsync(userReturned);
        }
    }
}