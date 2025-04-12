using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApi.Models.Common;

namespace WebApi.Models
{
    public class ProductVariant : BaseModel
    {
        [Key]
        public int Id { get; set; }
        public int Stock { get; set; }

        public int? ProdId { get; set; }
        [ForeignKey("ProdId")]
        public virtual Product Product { get; set; }

        public int? ColorId { get; set; }
        [ForeignKey("ColorId")]
        public virtual Color Color { get; set; }

        public int? SizeId { get; set; }
        [ForeignKey("SizeId")]
        public virtual Size Size { get; set; }

        public ICollection<Cart> Carts { get; set; }
        public ICollection<ReceiptDetail> ReceiptDetails { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
