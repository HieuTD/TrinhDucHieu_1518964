using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.DTOs.Carts;
using WebApi.DTOs.Products;
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

        [HttpGet("getcartbyuserid/{id}")]
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
                    ProductDetail = _context.Products.Where(i => i.Id == d.ProdId).Select(
                        i => new ProductDetailViewModel
                        {
                            Image = _context.ProductImages.Where(q => q.ProdId == d.ProdId).Select(q => q.Name).FirstOrDefault(),
                            Id = i.Id,
                            Name = i.Name,
                            Price = i.Price,
                            Discount = i.Discount
                        }).FirstOrDefault(),
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
            var result = _context.Carts.Where(s => s.UserId == json.UserId)
                .Select(d => new CartViewModel
                {
                    CartID = d.Id,
                    Color = d.Color,
                    Size = d.Size,
                    Quantity = d.Quantity,
                    ProductDetail = _context.Products.Where(i => i.Id == d.ProdId).Select(
                        i => new ProductDetailViewModel
                        {
                            Image = _context.ProductImages.Where(q => q.ProdId == d.ProdId).Select(q => q.Name).FirstOrDefault(),
                            Id = i.Id,
                            Name = i.Name,
                            Price = i.Price,
                            Discount = i.Discount
                        }).FirstOrDefault(),
                }).ToList();
            return Ok(result);
        }

        [HttpPost("delete")]
        public async Task<IActionResult> DeleteCart([FromBody] CartDeleteRequest delete)
        {
            var cart = _context.Carts.Where(d => d.ProdVariantId == delete.ProdVariantId && d.UserId == delete.UserId).SingleOrDefault();
            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Cart>> GetCartById(int id)
        {
            var cart = await _context.Carts.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }
            return cart;
        }

        [HttpPost]
        public async Task<ActionResult<Cart>> AddCart(Cart cart)
        {
            var shoppingCartItem = _context.Carts
                .SingleOrDefault(s => s.ProdId == cart.ProdId && s.UserId == cart.UserId && s.Color == cart.Color && s.Size == cart.Size);

            //Nếu chưa có loại sản phẩm tìm kiếm trong giỏ hàng thì thêm mới sản phẩm đó vào giỏ
            if (shoppingCartItem == null)
            {
                Cart newCart = new Cart();
                newCart.UserId = cart.UserId;
                newCart.ProdId = cart.ProdId;
                newCart.ProdVariantId = cart.ProdVariantId;
                newCart.Size = cart.Size;
                newCart.Color = cart.Color;
                newCart.Price = _context.Products.Where(d => d.Id == cart.ProdId).Select(d => d.Price).FirstOrDefault();
                newCart.Quantity = cart.Quantity;
                _context.Carts.Add(newCart);
                await _context.SaveChangesAsync();
            }
            //Nếu có trong giỏ hàng rồi thì chỉ cộng thêm số lượng
            else
            {
                shoppingCartItem.Quantity = shoppingCartItem.Quantity + cart.Quantity;
            }
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetCartById", new { id = cart.Id }, cart);
        }
    }
}
