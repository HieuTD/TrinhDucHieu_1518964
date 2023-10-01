using System.Collections.Generic;
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
        public int Gender { get; set; }

        public string BrandId { get; set; }
        [ForeignKey("BrandId")]
        public virtual Brand Brand { get; set; }

        public string CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        public string SupplierId { get; set; }
        [ForeignKey("SupplierId")]
        public virtual Supplier Supplier { get; set; }

        public ICollection<Cart> Carts { get; set; }
        public ICollection<ProductDetail> ProductDetails { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
