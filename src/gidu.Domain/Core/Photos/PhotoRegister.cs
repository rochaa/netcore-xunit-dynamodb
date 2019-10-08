using System.IO;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using gidu.Domain.Helpers;

namespace gidu.Domain.Core.Photos
{
    public class PhotoRegister
    {
        private static readonly string _accessKey = GlobalParameters.AccessKey;
        private static readonly string _secretKey = GlobalParameters.SecretKey;
        private static readonly string _bucket = GlobalParameters.BucketPhoto;

        public async Task<Stream> GetPhotoAsync(string key, string folder)
        {
            using (var client = new AmazonS3Client(_accessKey, _secretKey, RegionEndpoint.USEast1))
            {
                var fileTransferUtility = new TransferUtility(client);
                return await fileTransferUtility.OpenStreamAsync($"{_bucket}/{folder}", key);
            }
        }

        protected async Task UploadS3Async(MemoryStream PhotoMemoryStream, string key, string folder)
        {
            using (var client = new AmazonS3Client(_accessKey, _secretKey, RegionEndpoint.USEast1))
            {
                var uploadRequest = new TransferUtilityUploadRequest
                {
                    InputStream = PhotoMemoryStream,
                    Key = key,
                    BucketName = $"{_bucket}/{folder}",
                    CannedACL = S3CannedACL.AuthenticatedRead
                };

                var fileTransferUtility = new TransferUtility(client);
                await fileTransferUtility.UploadAsync(uploadRequest);
            }
        }
    }
}