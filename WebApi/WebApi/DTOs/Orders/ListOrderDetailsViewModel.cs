namespace WebApi.DTOs.Orders
{
    public class ListOrderDetailsViewModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string SizeName { get; set; }
        public string ColorName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
