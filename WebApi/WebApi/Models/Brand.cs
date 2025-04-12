using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApi.Models.Common;

namespace WebApi.Models
{
    public class Brand : BaseModel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        //public DateTime DateTime { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
