using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class Coupon
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Discount { get; set; }
    }
}
