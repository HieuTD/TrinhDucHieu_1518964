using System.Collections.Generic;
using System;
using WebApi.Models;
using WebApi.DTOs.ProductVariants;

namespace WebApi.DTOs.Products
{
    public class ProductDetailViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal? Discount { get; set; }
        public string Description { get; set; }
        public string Tag { get; set; }
        public string Image { get; set; }
        public decimal? Price { get; set; }
        public string Status { get; set; }
        public bool? IsFeatured { get; set; }
        public string SupplierName { get; set; }
        public string Material { get; set; }
        public int? BrandId { get; set; }
        public int? Gender { get; set; }
        public int? CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string BrandName { get; set; }
        public ICollection<ProductImage> ProductImages { get; set; }
        public ICollection<ListProdVariantWithColorSize> SanPhamBienThes { get; set; }
    }
}
