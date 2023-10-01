using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
    public class BlogImage
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? BlogId { get; set; }
        [ForeignKey("BlogId")]
        public virtual Blog Blog { get; set; }
    }
}
