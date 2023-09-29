using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
    public class ProductDetail
    {
        [Key]
        public int Id { get; set; }

        public int? ProdId { get; set; }
        [ForeignKey("ProdId")]
        public virtual Product Product { get; set; }
        public int? ColorId { get; set; }
        [ForeignKey("ColorId")]
        public virtual Color Color { get; set; }
        public int Stock { get; set; }
        public int? SizeId { get; set; }
        [ForeignKey("SizeId")]
        public virtual Size Size { get; set; }
    }
}
