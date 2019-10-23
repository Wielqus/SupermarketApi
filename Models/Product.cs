using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Supermarket.API.Models
{
    public class Product
    {
        [Required]
        public long Id { get; set; }

        [Required]
        [DefaultValue(false)]
        public string Name { get; set; }

        [Required]
        [DefaultValue(false)]
        public long Price { get; set; }
        public Category Category { get; set; }


    }
}