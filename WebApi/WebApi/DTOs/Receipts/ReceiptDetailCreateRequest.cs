namespace WebApi.DTOs.Receipts
{
    public class ReceiptDetailCreateRequest
    {
        public decimal ProdDetailPrice { get; set; }
        public string ProdDetailName { get; set; }
        public int Quantity { get; set; }
    }
}
