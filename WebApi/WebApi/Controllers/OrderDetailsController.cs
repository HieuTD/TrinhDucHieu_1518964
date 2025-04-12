using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WebApi.EF;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailsController : ControllerBase
    {
        private readonly DBcontext _context;

        public OrderDetailsController(DBcontext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetOrderDetailById(int id)
        {
            var result = _context.OrderDetails.Where(d => d.OrderId == id)
                .Select(
                d => new OrderDetail
                {
                    Price = d.Price,
                    Quantity = d.Quantity,
                    Color = d.Color,
                    Size = d.Size,
                    Product = _context.Products.Where(t => t.Id == d.ProdId).FirstOrDefault()
                }
                );
            return Ok(await result.ToListAsync());
        }
    }
}
