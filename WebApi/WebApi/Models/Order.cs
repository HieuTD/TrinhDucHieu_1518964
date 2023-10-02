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
        public int Status { get; set; }
        public string Address { get; set; }

        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual AppUser AppUser { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
