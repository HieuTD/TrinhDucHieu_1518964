using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace WebApi.DTOs.Products
{
    public class ProductCreateRequest
    {
        public string Name { get; set; }
        public decimal? Discount { get; set; }
        public string? Description { get; set; }
        public string Tag { get; set; }
        public int Gender { get; set; }
        public int? CategoryId { get; set; }
        public int? BrandId { get; set; }
        public int? SupplierId { get; set; }
        public decimal Price { get; set; }
        public decimal OriginalPrice { get; set; }
        public string Status { get; set; } //Trạng thái sản phẩm: mới, cũ ...
        public bool IsFeatured { get; set; } //Ẩn hiện
        public string Material { get; set; }
        public IFormFile files { get; set; }
    }
}
