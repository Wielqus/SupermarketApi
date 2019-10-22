using Microsoft.EntityFrameworkCore;
using Supermarket.Models;

namespace Supermarket.Models
{
    public class SupermarketContext : DbContext
    {
        public SupermarketContext(DbContextOptions<SupermarketContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Category { get; set; }
    }
}