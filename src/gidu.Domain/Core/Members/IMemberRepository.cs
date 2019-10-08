using System.Collections.Generic;
using System.Threading.Tasks;
using gidu.Domain.Helpers;

namespace gidu.Domain.Core.Members
{
    public interface IMemberRepository : IRepository<Member>
    {
        Task<List<Member>> GetLastMembersChangeAsync();
    }
}