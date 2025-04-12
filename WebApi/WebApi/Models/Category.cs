using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApi.Models.Common;

namespace WebApi.Models
{
    public class Category : BaseModel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Product> Products { get; set; }
        public ICollection<Color> Colors { get; set; }
        public ICollection<Size> Sizes { get; set; }
    }
}
