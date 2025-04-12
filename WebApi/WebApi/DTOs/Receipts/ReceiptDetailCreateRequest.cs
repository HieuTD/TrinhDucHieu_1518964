namespace WebApi.DTOs.Receipts
{
    public class ReceiptDetailCreateRequest
    {
        public decimal ProdVariantPrice { get; set; }
        public string ProdVariantName { get; set; }
        public int Quantity { get; set; }
    }
}
