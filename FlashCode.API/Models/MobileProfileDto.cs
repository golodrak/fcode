namespace FlashCode.API.Models
{
    public class MobileProfileDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool AcceptContact { get; set; }
    }
}
