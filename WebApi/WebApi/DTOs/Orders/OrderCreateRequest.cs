namespace WebApi.DTOs.Orders
{
    public class OrderCreateRequest
    {
        public string Description { get; set; }
        public decimal TotalPrice { get; set; }
        public string Address { get; set; }
        public string? UserId { get; set; }
        public int? TypePayment { get; set; }
    }
}
