using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using gidu.Domain.Core.Members;

namespace gidu.Repository.Repositories
{
    public class MemberRepository : Repository<Member>, IMemberRepository
    {
        public MemberRepository(GiduContext contexto) :
            base(contexto, Environment.GetEnvironmentVariable("TB_Members"))
        { }

        public async Task<List<Member>> GetLastMembersChangeAsync()
        {
            var limit = 15;
            return await GetAllAsync(limit);
        }
    }
}