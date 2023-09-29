using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class Size
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int? CateId { get; set; }
        [ForeignKey("CateId")]
        public virtual Category Category { get; set; }
        public virtual ICollection<ProductDetail> ProductDetails { get; set; }
    }
}
