using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using gidu.Domain.Core.Users;

namespace gidu.Repository.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(GiduContext contexto) :
            base(contexto, Environment.GetEnvironmentVariable("TB_Users"))
        { }

        public async Task<User> GetByEmailAsync(string email)
        {
            List<ScanCondition> filters = new List<ScanCondition> {
                new ScanCondition ("Email", ScanOperator.Equal, email)
            };

            var users = await GetByFiltersAsync(filters);
            return users.FirstOrDefault();
        }

        public async Task<User> GetByRefreshTokenAsync(string refreshToken)
        {
            List<ScanCondition> filters = new List<ScanCondition> {
                new ScanCondition ("RefreshToken", ScanOperator.Equal, refreshToken)
            };

            var users = await GetByFiltersAsync(filters);
            return users.FirstOrDefault();
        }
    }
}