using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace graduated_project.Models
{
    public class ShopSpheredbcontext:IdentityDbContext<AppUser>
    {
        public ShopSpheredbcontext()
        {
            
        }
        public ShopSpheredbcontext(DbContextOptions<ShopSpheredbcontext> options): base(options)
        {
        }

        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        //public virtual DbSet<AppUser> AppUsers { get; set; }
        public virtual DbSet<AppUserProduct> AppUserProducts { get; set; }
        public virtual DbSet<Category> Categorys { get; set; }
        public virtual DbSet<ProductOrder> ProductOrders { get; set; }





    }
}
