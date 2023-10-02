using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApi.Models.Common;

namespace WebApi.Models
{
    public class AuthHistory : BaseModel
    {
        [Key]
        public int Id { get; set; }
        //public DateTime DateTime { get; set; }

        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual AppUser AppUser { get; set; }
    }
}
