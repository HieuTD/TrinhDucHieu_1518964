using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.DTOs.Products;
using WebApi.EF;
using WebApi.Helper;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly DBcontext _context;

        public ProductsController(DBcontext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductViewModel>>> GetAllProducts()
        {
            //var listIdSanPhamliked = await _context.UserLikes.Select(s => s.IdSanPham).ToListAsync();
            var query = await _context.Products.Select(
                   s => new ProductViewModel()
                   {
                       Id = s.Id,
                       Name = s.Name,
                       Price = s.Price,
                       OriginalPrice = s.OriginalPrice,
                       Tag = s.Tag,
                       Discount = s.Discount,
                       Description = s.Description,
                       Gender = s.Gender,
                       Material = s.Material,
                       IsFeatured = s.IsFeatured,
                       Status = s.Status,
                       CategoryId = s.CategoryId,
                       BrandId = s.BrandId,
                       SupplierId = s.SupplierId,
                       //SoLuongComment = _context.UserComments.Where(x => x.IdSanPham == s.Id).Count(),
                       //SoLuongLike = _context.UserComments.Where(x => x.IdSanPham == s.Id).Count(),
                       CategoryName = _context.Categories.Where(d => d.Id == s.CategoryId).Select(d => d.Name).FirstOrDefault(),
                       BrandName = _context.Brands.Where(d => d.Id == s.BrandId).Select(d => d.Name).FirstOrDefault(),
                       Image = _context.ProductImages.Where(q => q.ProdId == s.Id).Select(q => q.Name).FirstOrDefault(),
                   }).Take(20).ToListAsync();
            return query;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var rs = await _context.Products.FindAsync(id);
            if (rs == null)
            {
                return NotFound();
            }
            return rs;
        }

        [HttpGet("listprod/{supplierid}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductBySupplierId(int supplierId)
        {
            return await _context.Products.Where(s => s.SupplierId == supplierId).ToListAsync();
        }

        [HttpPut("updateproductfeature/{id}")]
        public async Task<ActionResult> UpdateProductFeature(int id, Product sp)
        {
            Product sanpham = new Product();
            sanpham = await _context.Products.FirstOrDefaultAsync(s => s.Id == id);
            sanpham.IsFeatured = !sp.IsFeatured;
            await _context.SaveChangesAsync();
            //await _hubContext.Clients.All.BroadcastMessage();
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] ProductCreateRequest request)
        {
            var listImage = new List<ProductImage>();
            Product product = await _context.Products.FirstOrDefaultAsync(s => s.Id == id);
            product.Name = request.Name;
            product.UpdatedAt = DateTime.Now;
            product.Description = request.Description;
            product.Price = request.Price;
            product.Tag = request.Tag;
            product.Gender = request.Gender;
            product.OriginalPrice = request.OriginalPrice;
            product.Discount = request.Discount;
            product.Material = request.Material;
            product.Status = request.Status;
            product.IsFeatured = request.IsFeatured;
            if (request.BrandId == null)
            {
                product.BrandId = product.BrandId;
            }
            else
            {
                product.BrandId = (int)request.BrandId;
            }
            if (request.CategoryId == null)
            {
                product.CategoryId = product.CategoryId;
            }
            else
            {
                product.CategoryId = (int)request.CategoryId;
            }
            if (request.SupplierId == null)
            {
                product.SupplierId = product.SupplierId;
            }
            //Notification notification = new Notification()
            //{
            //    TenSanPham = request.Ten,
            //    TranType = "Edit"
            //};
            //_context.Notifications.Add(notification);
            ProductImage[] images = _context.ProductImages.Where(s => s.ProdId == id).ToArray();
            _context.ProductImages.RemoveRange(images);
            ProductImage image = new ProductImage();
            var productImages = _context.ProductImages.ToArray().Where(s => s.ProdId == id);
            foreach (var i in productImages)
            {
                FileHelper.DeleteFileOnTypeAndNameAsync("product", i.Name);
            }
            if (request.files != null)
            {
                var file = request.files.ToArray();
                for (int i = 0; i < file.Length; i++)
                {
                    if (file[i].Length > 0 && file[i].Length < 5120)
                    {
                        listImage.Add(new ProductImage()
                        {
                            Name = await FileHelper.UploadImageAndReturnFileNameAsync(request, null, "product", (IFormFile[])request.files, i),
                            ProdId = product.Id,
                        });
                    }
                }
            }
            else // xu li khi khong cap nhat hinh
            {
                List<ProductImage> List;
                List = _context.ProductImages.Where(s => s.ProdId == id).ToList();
                foreach (ProductImage img in List)
                    listImage.Add(new ProductImage()
                    {
                        Name = img.Name,
                        ProdId = product.Id,
                    }); ;
            };
            product.ProductImages = listImage;
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            //await _hubContext.Clients.All.BroadcastMessage();
            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult<Product>> AddProduct([FromForm] ProductCreateRequest request)
        {
            var listImage = new List<ProductImage>();
            Product product = new Product()
            {
                Name = request.Name,
                CreatedAt = DateTime.Now,
                Description = request.Description,
                Material = request.Material,
                IsFeatured = request.IsFeatured,
                Status = request.Status,
                Price = request.Price,
                Gender = request.Gender,
                OriginalPrice = request.OriginalPrice,
                Tag = request.Tag,
                Discount = request.Discount,
                CategoryId = (int)request.CategoryId,
                BrandId = (int)request.BrandId,
                SupplierId = (int)request.SupplierId,
            };
            //Notification notification = new Notification()
            //{
            //    TenSanPham = request.Ten,
            //    TranType = "Add"
            //};
            //_context.Notifications.Add(notification);
            var file = request.files.ToArray();
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            if (request.files != null)
            {
                for (int i = 0; i < file.Length; i++)
                {
                    if (file[i].Length > 0 && file[i].Length < 5120)
                    {
                        var productImage = new ProductImage();
                        productImage.Name = await FileHelper.UploadImageAndReturnFileNameAsync(request, null, "product", (IFormFile[])request.files, i);
                        productImage.ProdId = product.Id;
                        _context.ProductImages.Update(productImage);
                        await _context.SaveChangesAsync();
                    }
                }
            }
            //await _hubContext.Clients.All.BroadcastMessage();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var productImages = _context.ProductImages.ToArray().Where(s => s.ProdId == id);
            foreach (var i in productImages)
            {
                FileHelper.DeleteFileOnTypeAndNameAsync("product", i.Name);
            }
            Models.ProductVariant[] spbts;
            spbts = _context.ProductVariants.Where(s => s.ProdId == id).ToArray();
            _context.ProductVariants.RemoveRange(spbts);
            ProductImage[] images = _context.ProductImages.Where(s => s.ProdId == id).ToArray();
            _context.ProductImages.RemoveRange(images);
            await _context.SaveChangesAsync();
            var sanPham = await _context.Products.FindAsync(id);
            if (sanPham == null)
            {
                return NotFound();
            }
            var CategoryConstraint = _context.Categories.Where(s => s.Id == id);
            var BrandConstraint = _context.Brands.SingleOrDefaultAsync(s => s.Id == id);
            if (CategoryConstraint != null)
            {
                _context.Products.Remove(sanPham);
            }
            if (BrandConstraint != null)
            {
                _context.Products.Remove(sanPham);
            }
            //Notification notification = new Notification()
            //{
            //    TenSanPham = sanPham.Ten,
            //    TranType = "Delete"
            //};
            //_context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
            //await _hubContext.Clients.All.BroadcastMessage();
            return Ok();
        }
    }
}
