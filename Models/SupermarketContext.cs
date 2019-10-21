using Microsoft.EntityFrameworkCore;

namespace Supermarket.Models
{
    public class SupermarketContext : DbContext
    {
        public SupermarketContext(DbContextOptions<SupermarketContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
    }
}