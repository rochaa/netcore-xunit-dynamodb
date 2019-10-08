namespace gidu.Domain.Core.Users
{
    public class UserLoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Grant_Type { get; set; }
        public string Refresh_Token { get; set; }
    }
}