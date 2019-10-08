using System.Threading.Tasks;
using gidu.Domain.Helpers;

namespace gidu.Domain.Core.Members
{
    public class MemberRegister
    {
        public static string MemberNotFound = "Member não encontrado";
        public static string MemberExcluded = "Member está excluído";
        private readonly IMemberRepository _memberRepository;

        public MemberRegister(IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }

        // public async Task StoreAsync(MemberDto memberDto)
        // {
        //     var member = await _memberRepository.GetByIdAsync(memberDto.Id);

        //     if (member == null)
        //         member = new Member(memberDto);
        //     else
        //     {
        //         if (member.Status == MemberStatus.EXCLUDED)
        //             throw new DomainException("MemberRegister.StoreAsync", MemberExcluded);

        //         member.Update(memberDto);
        //     }

        //     await _memberRepository.SaveAsync(member);
        // }

        public async Task DeleteAsync(string id, string user)
        {
            var member = await _memberRepository.GetByIdAsync(id);

            if (member == null)
                throw new DomainException("MemberRegister.DeleteAsync", MemberNotFound);
            else
                member.Delete(user);

            await _memberRepository.SaveAsync(member);
        }
    }
}