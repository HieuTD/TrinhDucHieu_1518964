using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }
        public decimal? Price { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual AppUser AppUser { get; set; }
    }
}
