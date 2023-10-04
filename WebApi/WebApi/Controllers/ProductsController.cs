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

        //[HttpPut("capnhattrangthaihoatdong/{id}")]
        //public async Task<ActionResult> PutSanPhamTrangThaiHoatDong(int id, SanPham sp)
        //{
        //    SanPham sanpham = new SanPham();
        //    sanpham = await _context.SanPhams.FirstOrDefaultAsync(s => s.Id == id);
        //    sanpham.TrangThaiHoatDong = !sp.TrangThaiHoatDong;
        //    await _context.SaveChangesAsync();
        //    await _hubContext.Clients.All.BroadcastMessage();
        //    return Ok();
        //}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] ProductCreateRequest request)
        {
            var listImage = new List<ProductImage>();
            Product sanpham = await _context.Products.FirstOrDefaultAsync(s => s.Id == id);
            sanpham.Name = request.Name;
            sanpham.UpdatedAt = DateTime.Now;
            sanpham.Description = request.Description;
            sanpham.Price = request.Price;
            sanpham.Tag = request.Tag;
            sanpham.Gender = request.Gender;
            sanpham.OriginalPrice = request.OriginalPrice;
            sanpham.Discount = request.Discount;
            sanpham.Material = request.Material;
            sanpham.Status = request.Status;
            sanpham.IsFeatured = request.IsFeatured;
            if (request.BrandId == null)
            {
                sanpham.BrandId = sanpham.BrandId;
            }
            else
            {
                sanpham.BrandId = request.BrandId;
            }
            if (request.CategoryId == null)
            {
                sanpham.CategoryId = sanpham.CategoryId;
            }
            else
            {
                sanpham.CategoryId = request.CategoryId;
            }
            if (request.SupplierId == null)
            {
                sanpham.SupplierId = sanpham.SupplierId;
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
            var file = request.files.ToArray();
            var productImages = _context.ProductImages.ToArray().Where(s => s.ProdId == id);
            foreach (var i in productImages)
            {
                FileHelper.DeleteFileOnTypeAndNameAsync("product", i.Name);
            }
            if (request.files != null)
            {
                for (int i = 0; i < file.Length; i++)
                {
                    if (file[i].Length > 0 && file[i].Length < 5120)
                    {
                        listImage.Add(new ProductImage()
                        {
                            Name = await FileHelper.UploadImageAndReturnFileNameAsync(request, null, "product", (IFormFile[])request.files, i),
                            ProdId = sanpham.Id,
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
                        ProdId = sanpham.Id,
                    }); ;
            };
            sanpham.ProductImages = listImage;
            _context.Products.Update(sanpham);
            await _context.SaveChangesAsync();
            //await _hubContext.Clients.All.BroadcastMessage();
            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult<Product>> AddProduct([FromForm] ProductCreateRequest upload)
        {
            var listImage = new List<ProductImage>();
            Product sanpham = new Product()
            {
                Name = upload.Name,
                CreatedAt = DateTime.Now,
                Description = upload.Description,
                Material = upload.Material,
                IsFeatured = upload.IsFeatured,
                Status = upload.Status,
                Price = upload.Price,
                Gender = upload.Gender,
                OriginalPrice = upload.OriginalPrice,
                Tag = upload.Tag,
                Discount = upload.Discount,
                CategoryId = upload.CategoryId,
                BrandId = upload.BrandId,
                SupplierId = upload.SupplierId,
            };
            //Notification notification = new Notification()
            //{
            //    TenSanPham = upload.Ten,
            //    TranType = "Add"
            //};
            //_context.Notifications.Add(notification);
            var file = upload.files.ToArray();
            _context.Products.Add(sanpham);
            await _context.SaveChangesAsync();
            if (upload.files != null)
            {
                for (int i = 0; i < file.Length; i++)
                {
                    if (file[i].Length > 0 && file[i].Length < 5120)
                    {
                        var imageSanPham = new ProductImage();
                        imageSanPham.Name = await FileHelper.UploadImageAndReturnFileNameAsync(upload, null, "product", (IFormFile[])upload.files, i);
                        imageSanPham.ProdId = sanpham.Id;
                        _context.ProductImages.Update(imageSanPham);
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
            var imageSanPhams = _context.ProductImages.ToArray().Where(s => s.ProdId == id);
            foreach (var i in imageSanPhams)
            {
                FileHelper.DeleteFileOnTypeAndNameAsync("product", i.Name);
            }
            Models.ProductDetail[] spbts;
            spbts = _context.ProductDetails.Where(s => s.ProdId == id).ToArray();
            _context.ProductDetails.RemoveRange(spbts);
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
