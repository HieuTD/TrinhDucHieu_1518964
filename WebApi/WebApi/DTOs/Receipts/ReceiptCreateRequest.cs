using System.Collections.Generic;

namespace WebApi.DTOs.Receipts
{
    public class ReceiptCreateRequest
    {
        public string UserId { get; set; }
        public string Description { get; set; }
        public decimal TotalPrice { get; set; }
        public string SupplierId { get; set; }
        public ICollection<ReceiptDetailCreateRequest> ReceiptDetails { get; set; }
    }
}
