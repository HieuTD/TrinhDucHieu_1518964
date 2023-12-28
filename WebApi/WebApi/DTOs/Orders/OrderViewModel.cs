using System;

namespace WebApi.DTOs.Orders
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int? Status { get; set; }
        public string FullName { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int TypePayment { get; set; } 
    }
}
