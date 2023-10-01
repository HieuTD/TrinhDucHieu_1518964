using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class Supplier
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public ICollection<Product> Products { get; set; }
        public ICollection<Receipt> Receipts { get; set; }
    }
}
