using System;

namespace gidu.Domain.Helpers
{
    public class GlobalParameters
    {
        public static string KeyPassword = Environment.GetEnvironmentVariable("Key_Password");
        public static string AccessKey = Environment.GetEnvironmentVariable("AWS_AccessKey");
        public static string SecretKey = Environment.GetEnvironmentVariable("AWS_SecretKey");
        public static string BucketPhoto = Environment.GetEnvironmentVariable("AWS_Bucket_Photo");
        public static string FolderPhotoMembers = "members";
        public static string FolderPhotoUsers = "users";
        public static int TokenValidityInMinutes = 60;
    }
}