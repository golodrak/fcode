namespace FlashCode.Dashboard.Models
{
    public class LoginViewModel
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class LoginResult
    {
        public string Token { get; set; } = string.Empty;
    }
}
