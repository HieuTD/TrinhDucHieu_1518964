using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
    public class OrderDetail
    {
        [Key]
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal Price { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }

        public string? OrderId { get; set; }
        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }

        public string? ProdId { get; set; }
        [ForeignKey("ProdId")]
        public virtual Product Product { get; set; }

        public string? ProdDetailId { get; set; }
        [ForeignKey("ProdDetailId")]
        public virtual ProductDetail ProductDetail { get; set; }
    }
}
