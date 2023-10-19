using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApi.Models.Common;

namespace WebApi.Models
{
    public class Product : BaseModel
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
        public string Status { get; set; } //Trạng thái sản phẩm: mới, cũ ...
        public bool IsFeatured { get; set; } //Ẩn hiện
        public int Gender { get; set; }

        public int BrandId { get; set; }
        [ForeignKey("BrandId")]
        public virtual Brand Brand { get; set; }

        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        public int SupplierId { get; set; }
        [ForeignKey("SupplierId")]
        public virtual Supplier Supplier { get; set; }

        public ICollection<Cart> Carts { get; set; }
        public ICollection<ProductVariant> ProductVariants { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
        public ICollection<ProductImage> ProductImages { get; set; }
    }
}
