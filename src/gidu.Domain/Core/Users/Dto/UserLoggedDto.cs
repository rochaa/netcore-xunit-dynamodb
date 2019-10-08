namespace gidu.Domain.Core.Users
{
    public class UserLoggedDto
    {
        public string Token { get; set; }
        public string Refresh_Token { get; set; }
        public UserDto User { get; set; }
    }
}