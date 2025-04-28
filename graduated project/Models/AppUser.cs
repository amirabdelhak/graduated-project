using Microsoft.AspNetCore.Identity;

namespace graduated_project.Models
{
    public class AppUser:IdentityUser
    {
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public string? Mobilnum { get; set; }

        public virtual ICollection<Order>? Orders { get; set; }
        public virtual ICollection<AppUserProduct>? AppUserProducts { get; set; }

    }
}
