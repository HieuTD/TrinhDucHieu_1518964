namespace WebApi.DTOs.ProductVariants
{
    public class ListProdVariantWithColorSize
    {
        public int Id { get; set; }
        public int? ProdId { get; set; }
        public string ColorName { get; set; }
        public int? Gender { get; set; }
        public int Stock { get; set; }
        public string SizeName { get; set; }
    }
}
