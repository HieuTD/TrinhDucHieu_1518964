namespace WebApi.DTOs.ProductDetails
{
    public class ProductDetailCreateRequest
    {
        public int Stock { get; set; }
        public int? ProdId { get; set; }
        public int? ColorId { get; set; }
        public int? SizeId { get; set; }
    }
}
