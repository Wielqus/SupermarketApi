using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recipes.Models
{
    public class Recipe
    {
        [Required]
        public long Id { get; set; }

        [Required]
        [DefaultValue(false)]
        public string Name { get; set; }

        [Required]
        [DefaultValue(false)]
        public string Description { get; set; }


    }
}