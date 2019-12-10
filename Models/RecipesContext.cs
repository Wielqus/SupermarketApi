using Microsoft.EntityFrameworkCore;
using Recipes.Models;

namespace Recipes.Models
{
    public class RecipesContext : DbContext
    {
        public RecipesContext(DbContextOptions<RecipesContext> options)
            : base(options)
        {
        }
        public DbSet<Recipe> Recipes { get; set; }

    }
}