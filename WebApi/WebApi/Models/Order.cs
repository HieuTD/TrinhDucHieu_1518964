using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApi.Models.Common;

namespace WebApi.Models
{
    public class Order : BaseModel
    {
        [Key]
        public int Id { get; set; }
        //public DateTime DateTime { get; set; }
        public string Description { get; set; }
        public int Status { get; set; } //0: chưa xác nhận, 1: đã xác nhận, 2: Hoàn thành, 3: Đã Hủy
        public decimal TotalPrice { get; set; }
        public string Address { get; set; }
        public int TypePayment { get; set; } //0: thanh toán COD, 1: thanh toán online

        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual AppUser AppUser { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
