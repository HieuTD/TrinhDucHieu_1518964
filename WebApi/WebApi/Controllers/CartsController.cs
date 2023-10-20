using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.DTOs.Carts;
using WebApi.EF;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly DBcontext _context;

        public CartsController(DBcontext context)
        {
            _context = context;
        }

        [HttpGet("getCart/{id}")]
        public async Task<ActionResult<IEnumerable<CartViewModel>>> GetCartByUserId(string id)
        {
            var getiduser = id;
            var rs = await _context.Carts.Where(s => s.UserId == getiduser)
                .Select(d => new CartViewModel
                {
                    ProdVariantId = d.ProdVariantId,
                    CartID = d.Id,
                    Color = d.Color,
                    Size = d.Size,
                    Quantity = d.Quantity,
                    //ProductDetail = _context.SanPhams.Where(i => i.Id == d.SanPhamId).Select(
                    //    i => new ProductDetail
                    //    {
                    //        Image = _context.ImageSanPhams.Where(q => q.IdSanPham == d.SanPhamId).Select(q => q.ImageName).FirstOrDefault(),
                    //        Id = i.Id,
                    //        Ten = i.Ten,
                    //        GiaBan = i.GiaBan,
                    //        KhuyenMai = i.KhuyenMai
                    //    }).FirstOrDefault(),
                }).ToListAsync();
            return rs;
        }


        [HttpPut("update")]
        public async Task<ActionResult> UpdateCart(Cart json)
        {
            var temp = await _context.Carts.Where(s => s.Id == json.Id).FirstOrDefaultAsync();
            if (json.Quantity < 1)
            {
                _context.Carts.Remove(temp);
            }
            else
            {
                temp.Quantity = json.Quantity;
            }
            _context.SaveChanges();
            var resuft = _context.Carts.Where(s => s.UserId == json.UserId)
                .Select(d => new CartViewModel
                {
                    CartID = d.Id,
                    Color = d.Color,
                    Size = d.Size,
                    Quantity = d.Quantity,
                    //ProductDetail = _context.SanPhams.Where(i => i.Id == d.SanPhamId).Select(
                    //    i => new ProductDetail
                    //    {
                    //        Image = _context.ImageSanPhams.Where(q => q.IdSanPham == d.SanPhamId).Select(q => q.ImageName).FirstOrDefault(),
                    //        Id = i.Id,
                    //        Ten = i.Ten,
                    //        GiaBan = i.GiaBan,
                    //        KhuyenMai = i.KhuyenMai
                    //    }).FirstOrDefault(),
                }).ToList();
            return Ok(resuft);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteCart([FromBody] CartDeleteRequest delete)
        {
            var card = _context.Carts.Where(d => d.ProdVariantId == delete.ProdVariantId && d.UserId == delete.UserId).SingleOrDefault();
            _context.Carts.Remove(card);
            await _context.SaveChangesAsync();
            //return Json("1");
            return Ok();
        }
    }
}
