using System.ComponentModel.DataAnnotations;
using WebApi.Models.Common;

namespace WebApi.Models
{
    public class Coupon : BaseModel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Discount { get; set; }
    }
}
