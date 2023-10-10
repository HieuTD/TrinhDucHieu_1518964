using System.Collections.Generic;
using WebApi.Models;

namespace WebApi.DTOs.Orders
{
    public class OrderDetailViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public Order order { get; set; }
        public List<ListOrderDetailsViewModel> listOrderDetails { get; set; }
    }
}
