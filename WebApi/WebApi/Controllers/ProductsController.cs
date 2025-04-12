using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WebApi.DTOs.Products;
using WebApi.DTOs.ProductVariants;
using WebApi.EF;
using WebApi.Helper;
using WebApi.Helper.FileStorage;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly DBcontext _context;
        private const string LIST_IMAGE_PRODUCT = "Images/list-image-product";
        private readonly IFileStorageService _storageService;

        public ProductsController(DBcontext context, IFileStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductViewModel>>> GetAllProducts()
        {
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

            //update ảnh
            if (request.files != null)
            {
                var image = await _context.ProductImages.FirstOrDefaultAsync(i => i.ProdId == id);
                //Nếu trước đó chưa có ảnh thì tạo ảnh mới
                if (image == null)
                {
                    product.ProductImages = new List<ProductImage>()
                    {
                        new ProductImage()
                        {
                            CreatedAt = DateTime.Now,
                            Name = await this.SaveFile(request.files),
                        }
                    };
                }
                //Nếu đã có ảnh thì xóa ảnh cũ và cập nhật ảnh mới
                else
                {
                    _context.ProductImages.Remove(image);
                    await _storageService.DeleteFileProductAsync(image.Name);
                    await _context.SaveChangesAsync();
                    product.ProductImages = new List<ProductImage>()
                    {
                        new ProductImage()
                        {
                            CreatedAt = DateTime.Now,
                            Name = await this.SaveFile(request.files),
                        }
                    };
                }
            }
            //không update ảnh
            else 
            {
                var image = _context.ProductImages.Where(s => s.ProdId == id).ToList();
                product.ProductImages = image;
            };
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
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
            _context.Products.Add(product);
            if (request.files != null)
            {
                product.ProductImages = new List<ProductImage>()
                {
                    new ProductImage()
                    {
                        CreatedAt = DateTime.Now,
                        Name = await this.SaveFile(request.files),
                    }
                };
            };
            await _context.SaveChangesAsync();
            return Ok();
        }

        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileProductAsync(file.OpenReadStream(), fileName);
            return "/" + LIST_IMAGE_PRODUCT + "/" + fileName;
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var image = await _context.ProductImages.FirstOrDefaultAsync(s => s.ProdId == id);
            if (image != null)
            {
                _context.ProductImages.Remove(image);
                await _storageService.DeleteFileProductAsync(image.Name);
            }


            Models.ProductVariant[] spbts;
            spbts = _context.ProductVariants.Where(s => s.ProdId == id).ToArray();
            if (spbts != null)
            {
            _context.ProductVariants.RemoveRange(spbts);
            }    
            
            var sanPham = await _context.Products.FindAsync(id);
            if (sanPham == null)
            {
                return NotFound();
            }
            else
            {
                _context.Products.Remove(sanPham);
            }
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("productdetail/{id}")]
        public async Task<ActionResult<ProductDetailViewModel>> GetProductDetailByProdId(int id)
        {
            List<ListProdVariantWithColorSize> listSPBT;

            var temp = from s in _context.ProductVariants

                       join z in _context.Sizes
                       on s.SizeId equals z.Id

                       join m in _context.Colors
                       on s.ColorId equals m.Id
                       select new ListProdVariantWithColorSize()
                       {
                           Id = s.Id,
                           Stock = s.Stock,
                           ColorName = m.Name,
                           SizeName = z.Name,
                           ProdId = s.ProdId,
                       };
            listSPBT = await temp.Where(s => s.ProdId == id).ToListAsync();

            var kb = (from s in _context.Products

                     join spbt in _context.ProductVariants
                     on s.Id equals spbt.ProdId

                      join hinh in _context.ProductImages
                      on s.Id equals hinh.ProdId

                      join th in _context.Brands
                     on s.BrandId equals th.Id

                     join l in _context.Categories
                     on s.CategoryId equals l.Id

                     join ncc in _context.Suppliers
                     on s.SupplierId equals ncc.Id
                     select new ProductDetailViewModel()
                     {
                         Id = s.Id,
                         Name = s.Name,
                         Price = s.Price,
                         Tag = s.Tag,
                         Discount = s.Discount,
                         Description = s.Description,
                         Gender = s.Gender,
                         SupplierName = ncc.Name,
                         Material = s.Material,
                         Status = s.Status,
                         IsFeatured = s.IsFeatured,
                         CategoryId = s.CategoryId,
                         BrandId = s.BrandId,
                         CategoryName = l.Name,
                         BrandName = th.Name,
                         Image = _context.ProductImages.Where(q => q.ProdId == s.Id).Select(q => q.Name).FirstOrDefault(),
                         SanPhamBienThes = listSPBT,
                     }).ToList();
            var rs = kb.FirstOrDefault(s => s.Id == id);
            return rs;
        }

        [HttpGet("listnewproduct")]
        public async Task<ActionResult<IEnumerable<ProductViewModel>>> GetListNewProduct()
        {
            var kb = _context.Products.Select(
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
                       Status = s.Status,
                       IsFeatured = s.IsFeatured,
                       CategoryId = s.CategoryId,
                       BrandId = s.BrandId,
                       CategoryName = _context.Categories.Where(d => d.Id == s.CategoryId).Select(d => d.Name).FirstOrDefault(),
                       BrandName = _context.Brands.Where(d => d.Id == s.BrandId).Select(d => d.Name).FirstOrDefault(),
                       Image = _context.ProductImages.Where(q => q.ProdId == s.Id).Select(q => q.Name).FirstOrDefault(),
                   }).Take(20).Where(s => s.Status == "new" && s.IsFeatured == true);
            return await kb.ToListAsync();
        }

        [HttpPost("listproductarrange")]
        public async Task<ActionResult> GetListProductArrange(ProductArrangeRequest sx)
        {
            var kb = _context.Products.Where(d => d.Price > sx.Low && d.Price <= sx.High).Select(
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
                       Status = s.Status,
                       IsFeatured = s.IsFeatured,
                       CategoryId = s.CategoryId,
                       BrandId = s.BrandId,
                       CategoryName = _context.Categories.Where(d => d.Id == s.CategoryId).Select(d => d.Name).FirstOrDefault(),
                       BrandName = _context.Brands.Where(d => d.Id == s.BrandId).Select(d => d.Name).FirstOrDefault(),
                       Image = _context.ProductImages.Where(q => q.ProdId == s.Id).Select(q => q.Name).FirstOrDefault(),
                   }).Take(20);
            return Ok(await kb.ToListAsync());
        }

        [HttpGet("listproductfilterbycolor/{colorName}")]
        public async Task<IActionResult> GetListProdFilterByColor(string colorName)
        {
            var list_id_mau = _context.Colors.Where(d => d.Name == colorName).Select(d => d.Id.ToString()).ToList();
            var list_spbienthe_theomau = _context.ProductVariants.Where(d => list_id_mau.Contains((d.ColorId.ToString()))).Select(d => d.ProdId).Distinct().ToList();
            var kb = _context.Products.Where(d => list_spbienthe_theomau.Contains(d.Id)).Select(
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
                       Status = s.Status,
                       IsFeatured = s.IsFeatured,
                       CategoryId = s.CategoryId,
                       BrandId = s.BrandId,
                       CategoryName = _context.Categories.Where(d => d.Id == s.CategoryId).Select(d => d.Name).FirstOrDefault(),
                       BrandName = _context.Brands.Where(d => d.Id == s.BrandId).Select(d => d.Name).FirstOrDefault(),
                       Image = _context.ProductImages.Where(q => q.ProdId == s.Id).Select(q => q.Name).FirstOrDefault(),
                   }).Take(20);
            return Ok(await kb.ToListAsync());
        }



        [HttpGet("GetAllProductsClient")]
        public async Task<ActionResult<IEnumerable<ProductViewModel>>> GetAllProductsClient()
        {
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
                       CategoryName = _context.Categories.Where(d => d.Id == s.CategoryId).Select(d => d.Name).FirstOrDefault(),
                       BrandName = _context.Brands.Where(d => d.Id == s.BrandId).Select(d => d.Name).FirstOrDefault(),
                       Image = _context.ProductImages.Where(q => q.ProdId == s.Id).Select(q => q.Name).FirstOrDefault(),
                   }).Take(20).Where(s => s.IsFeatured == true).ToListAsync();
            return query;
        }
    }
}
