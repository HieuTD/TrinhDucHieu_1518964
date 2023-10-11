using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.DTOs.ProductDetails;
using WebApi.EF;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using WebApi.Models;
using System;
using WebApi.DTOs.Products;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductDetailsController : ControllerBase
    {
        private readonly DBcontext _context;

        public ProductDetailsController(DBcontext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDetailViewModel>>> GetAllProductDetails()
        {
            var query = from pd in _context.ProductDetails

                     join p in _context.Products
                     on pd.ProdId equals p.Id

                     join c in _context.Colors
                     on pd.ColorId equals c.Id
                     
                     join s in _context.Sizes
                     on pd.SizeId equals s.Id
                     select new ProductDetailViewModel()
                     {
                         Id = pd.Id,
                         ProdId = p.Id,
                         ProductName = p.Name,
                         ColorId = c.Id,
                         ColorName = c.Name,
                         SizeId = s.Id,
                         SizeName = s.Name,
                         Stock = pd.Stock
                     };
            return await query.Take(50).ToListAsync();
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDetailViewModel>> GetProductDetailById(int id)
        {
            var prodDetail = await _context.ProductDetails.FindAsync(id);
            if (prodDetail == null)
            {
                return NotFound();
            }
            return new ProductDetailViewModel()
            {
                Id = prodDetail.Id,
                ProdId = prodDetail.Id,
                ColorId = prodDetail.Id,
                SizeId = prodDetail.Id,
                Stock = prodDetail.Stock
            };
        }

        [HttpPost]
        public async Task<ActionResult<ProductDetailViewModel>> AddProductDetail([FromForm] ProductDetailCreateRequest request)
        {
            //Notification notification = new Notification()
            //{
            //    TranType = "Add"
            //};
            //_context.Notifications.Add(notification);
            ProductDetail prodDetail = new ProductDetail();
            prodDetail.Stock = request.Stock;
            prodDetail.ProdId = request.ProdId;
            prodDetail.ColorId = request.ColorId;
            prodDetail.SizeId = request.SizeId;
            prodDetail.CreatedAt = DateTime.Now;

            _context.ProductDetails.Add(prodDetail);
            await _context.SaveChangesAsync();
            //await _hubContext.Clients.All.BroadcastMessage();
            return Ok(prodDetail);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProductDetail(int id, [FromForm] ProductDetailCreateRequest request)
        {
            ProductDetail prodDetail = await _context.ProductDetails.FindAsync(id);
            prodDetail.Stock = request.Stock;
            prodDetail.ProdId = request.ProdId;
            prodDetail.ColorId = request.ColorId;
            prodDetail.SizeId = request.SizeId;
            prodDetail.UpdatedAt = DateTime.Now;

            _context.ProductDetails.Update(prodDetail);

            //Notification notification = new Notification()
            //{
            //    TranType = "Edit"
            //};
            //_context.Notifications.Add(notification);
            //await _hubContext.Clients.All.BroadcastMessage();
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSanPhamBienTh(int id)
        {
            ProductDetail prodDetail = await _context.ProductDetails.FindAsync(id);
            _context.ProductDetails.Remove(prodDetail);
            //Notification notification = new Notification()
            //{
            //    //TenSanPham = spbt.ImagePath,
            //    TranType = "Delete",
            //};
            //_context.Notifications.Add(notification);
            //await _hubContext.Clients.All.BroadcastMessage();
            await _context.SaveChangesAsync();
            return Ok();
        }


        [HttpGet("listproddetail/{id}")]
        public async Task<ActionResult<IEnumerable<GetListProdDetailByProdIdVM>>> GetListProdDetailByProdId(int id)
        {
            var kb = from spbt in _context.ProductDetails
                     join sp in _context.Products.Where(s => s.Id == id)
                     on spbt.ProdId equals sp.Id
                     join l in _context.Categories
                     on sp.CategoryId equals l.Id
                     join m in _context.Colors
                     on spbt.ColorId equals m.Id
                     join s in _context.Sizes
                     on spbt.SizeId equals s.Id
                     select new GetListProdDetailByProdIdVM()
                     {
                         Id = spbt.Id,
                         Name = "Id: " + spbt.Id + " Tên: " + sp.Name + " " + l.Name + " " + m.Name,
                         OriginalPrice = (decimal)sp.OriginalPrice,
                     };
            return await kb.ToListAsync();
        }
    }
}
