using Microsoft.AspNetCore.Identity;

namespace AkaryakitOtomasyonu.Models
{
    public class User : IdentityUser
    {
        public string Role { get; set; } = string.Empty;
        public ICollection<Fueling> Fuelings { get; set; }
    }
}
