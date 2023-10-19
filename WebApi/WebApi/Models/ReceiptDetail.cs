using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WebApi.Models.Common;

namespace WebApi.Models
{
    public class ReceiptDetail : BaseModel
    {
        [Key]
        public int Id { get; set; }
        public int Amonut { get; set; }
        public decimal TotalPrice { get; set; }

        public int? ReceiptId { get; set; }
        [ForeignKey("ReceiptId")]
        public virtual Receipt Receipt { get; set; }

        public int? ProdVariantId { get; set; }
        [ForeignKey("ProdVariantId")]
        public virtual ProductVariant ProductVariant { get; set; }
    }
}
