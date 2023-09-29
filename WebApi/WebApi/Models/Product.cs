using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal? Discount { get; set; }
        public string Tag { get; set; }
        public string Material { get; set; }
        public string Status { get; set; }
        public bool IsFeatured { get; set; }

    }
}
