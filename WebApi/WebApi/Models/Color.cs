using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
    public class Color
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public int? CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        public ICollection<ProductDetail> ProductDetails { get; set; }
    }
}
