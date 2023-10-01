using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public string Content { get; set; }
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual AppUser AppUser { get; set; }
        public string? ProdId { get; set; }
        [ForeignKey("ProdId")]
        public virtual Product Product { get; set; }
    }
}
