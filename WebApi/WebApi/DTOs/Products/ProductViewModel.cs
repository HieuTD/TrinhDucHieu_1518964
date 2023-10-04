namespace WebApi.DTOs.Products
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal? Discount { get; set; }
        public string? Description { get; set; }
        public string Tag { get; set; }
        public string Image { get; set; }
        public int Gender { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public decimal Price { get; set; }
        public decimal OriginalPrice { get; set; }
        public string Status { get; set; } //Trạng thái sản phẩm: mới, cũ ...
        public bool IsFeatured { get; set; } //Ẩn hiện
        public string Material { get; set; }
    }
}
