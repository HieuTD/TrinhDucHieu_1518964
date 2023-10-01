using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Product> Products { get; set; }
        public ICollection<Color> Colors { get; set; }
        public ICollection<Size> Sizes { get; set; }
    }
}
