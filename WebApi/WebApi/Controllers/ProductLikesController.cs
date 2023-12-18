using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebApi.DTOs.Products;
using WebApi.EF;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductLikesController : ControllerBase
    {
        private readonly DBcontext _context;

        public ProductLikesController(DBcontext context)
        {
            _context = context;
        }


        [HttpPost("like")]
        public async Task<ActionResult> AddLikeProduct(ProductLikeCreateRequest request)
        {
            var result = await _context.ProductLikes.Where(d => d.ProdId == request.ProdId && d.UserId == request.UserId).FirstOrDefaultAsync();
            if (result == null)
            {
                result = new ProductLike
                {
                    ProdId = request.ProdId,
                    UserId = request.UserId,
                    CreatedAt = DateTime.Now
                };
                _context.Add(result);
                _context.SaveChanges();
                return Ok(1);
            }
            else
            {
                _context.Remove(result);
                _context.SaveChanges();
                return Ok(2);
            }
        }

        [HttpGet("listprodlikebyuserid/{id}")]
        public async Task<ActionResult> GetListProdLikeByUserId(string id)
        {
            var result = _context.ProductLikes.Where(d => d.UserId == id).Select(
                d => new ProductLike
                {
                    Id = d.Id,
                    ProdId = d.ProdId,
                    Name = _context.Products.Where(s => s.Id == d.ProdId).Select(s => s.Name).FirstOrDefault(),
                    Price = (int)_context.Products.Where(s => s.Id == d.ProdId).Select(s => s.Price).FirstOrDefault(),
                });
            return Ok(await result.ToListAsync());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteLike(int id)
        {
            var prodLike = _context.ProductLikes.Where(d => d.Id == id).SingleOrDefault();
            _context.ProductLikes.Remove(prodLike);
            await _context.SaveChangesAsync();
            return Ok("1");
        }
    }
}
