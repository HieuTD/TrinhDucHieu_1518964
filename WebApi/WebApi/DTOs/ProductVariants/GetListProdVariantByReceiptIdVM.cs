namespace WebApi.DTOs.ProductVariants
{
    public class GetListProdVariantByReceiptIdVM
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public int ReceiptId { get; set; }
    }
}
