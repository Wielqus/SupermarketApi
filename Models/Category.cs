using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;


namespace Supermarket.API.Models
{
   public class Category
    {
        [Required]
        public long Id { get; set; }

        [Required]
        [DefaultValue(false)]
        public string Name { get; set; }

        public List<Product> Products { get; set; }

    }
}