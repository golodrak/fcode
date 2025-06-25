using Microsoft.AspNetCore.Identity;

namespace FlashCode.API.Models.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? Company { get; set; }
        public bool AcceptContact { get; set; }
        public string MobileToken { get; set; } = string.Empty;
    }
}
