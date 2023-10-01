using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
    public class ProductLike
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }

        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual AppUser AppUser { get; set; }
        public string? ProdId { get; set; }
        [ForeignKey("ProdId")]
        public virtual Product Product { get; set; }
    }
}
