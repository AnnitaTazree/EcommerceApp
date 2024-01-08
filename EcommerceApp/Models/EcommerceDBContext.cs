using Microsoft.EntityFrameworkCore;

namespace EcommerceApp.Models
{
    public class EcommerceDBContext: DbContext
    {
        public EcommerceDBContext(DbContextOptions<EcommerceDBContext> options):base(options)
        {
            
        }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<SalesOrder> SalesOrders { get; set; }
        public DbSet<SalesItem> SalesItems { get; set; }
    }
}
