using System;
using System.IO;
using System.Threading.Tasks;
using gidu.Domain.Core.Logs;
using gidu.Domain.Core.Members;
using gidu.Domain.Helpers;

namespace gidu.Domain.Core.Photos
{
    public class PhotoMember : PhotoRegister
    {
        private readonly IMemberRepository _memberRepository;
        private readonly string MemberNotFound = "Membro não encontrado.";

        public PhotoMember(IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }

        public async Task UploadPhotoAsync(MemoryStream PhotoMemoryStream, string memberId, string userLog)
        {
            var member = await _memberRepository.GetByIdAsync(memberId);

            // 1. Checar se membro existe.
            if (member == null)
                throw new DomainException("PhotoRegister.UploadPhotoMemberAsync", MemberNotFound);

            // 2. Checar se já existe foto para o membro.
            if (string.IsNullOrEmpty(member.KeyPhoto))
            {
                var key = Guid.NewGuid().ToString();
                member.AddKeyPhoto(key);
            }

            // 3. Upload da foto no S3.
            await UploadS3Async(PhotoMemoryStream, member.KeyPhoto, GlobalParameters.FolderPhotoMembers);

            // 4. Registrar log do Upload  
            member.AddLog(LogOperation.UPLOADPHOTO, userLog);

            // 5. Salvar dados
            await _memberRepository.SaveAsync(member);
        }
    }
}