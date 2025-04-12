namespace WebApi.DTOs.ProductVariants
{
    public class ProductVariantViewModel
    {
        public int Id { get; set; }
        public int Stock { get; set; }
        public int? ProdId { get; set; }
        public string ProductName { get; set; }
        public int? ColorId { get; set; }
        public string ColorName { get; set; }
        public int? SizeId { get; set; }
        public string SizeName { get; set; }
    }
}
