using System;

namespace WebApi.DTOs.Receipts
{
    public class ReceiptViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedAt { get; set; }
        public decimal TotalPrice { get; set; }
        public string Description { get; set; }
        public string UserName { get; set; }
        public string SupplierName { get; set; }
    }
}
