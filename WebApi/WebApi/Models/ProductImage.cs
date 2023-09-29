using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
    public class ProductImage
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public int? ProdId { get; set; }
        [ForeignKey("ProdId")]
        public virtual Product Product { get; set; }
    }
}
