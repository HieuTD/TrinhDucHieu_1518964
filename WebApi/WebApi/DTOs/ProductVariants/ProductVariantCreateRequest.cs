namespace WebApi.DTOs.ProductVariants
{
    public class ProductVariantCreateRequest
    {
        public int Stock { get; set; }
        public int? ProdId { get; set; }
        public int? ColorId { get; set; }
        public int? SizeId { get; set; }
    }
}
