namespace FlashCode.Mobile.Models
{
    public class MobileRegisterDto
    {
        public string? Token { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool AcceptContact { get; set; }
    }
}
