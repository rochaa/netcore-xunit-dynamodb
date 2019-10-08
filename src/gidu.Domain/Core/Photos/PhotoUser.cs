using System;
using System.IO;
using System.Threading.Tasks;
using gidu.Domain.Core.Logs;
using gidu.Domain.Core.Users;
using gidu.Domain.Helpers;

namespace gidu.Domain.Core.Photos
{
    public class PhotoUser : PhotoRegister
    {
        private readonly IUserRepository _userRepository;
        public readonly string UserNotFound = "Usuário não encontrado.";

        public PhotoUser(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task UploadPhotoAsync(MemoryStream PhotoMemoryStream, string userId, string userLog)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            // 1. Checar se usuário existe.
            if (user == null)
                throw new DomainException("PhotoRegister.UploadPhotoUserAsync", UserNotFound);

            // 2. Checar se já existe foto para o usuário.
            if (string.IsNullOrEmpty(user.KeyPhoto))
            {
                var key = Guid.NewGuid().ToString();
                user.AddKeyPhoto(key);
            }

            // 3. Upload da foto no S3.
            await UploadS3Async(PhotoMemoryStream, user.KeyPhoto, GlobalParameters.FolderPhotoUsers);

            // 4. Registrar log do Upload
            user.AddLog(LogOperation.UPLOADPHOTO, userLog);

            // 5. Salvar dados
            await _userRepository.SaveAsync(user);
        }
    }
}