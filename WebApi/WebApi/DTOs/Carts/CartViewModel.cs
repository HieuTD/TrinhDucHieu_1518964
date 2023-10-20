using WebApi.DTOs.Products;
using WebApi.Models;

namespace WebApi.DTOs.Carts
{
    public class CartViewModel
    {
        public int CartID { get; set; }
        public string UserID { get; set; }
        public string Color { get; set; }
        public int Quantity { get; set; }
        public int? ProdVariantId { get; set; }
        public string Size { get; set; }
        public ProductDetailViewModel ProductDetail { get; set; }
    }
}
