using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApi.EF;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly DBcontext _context;

        public StatisticsController(DBcontext context)
        {
            _context = context;
        }

        [Route("productcount")]
        [HttpGet]
        public async Task<ActionResult<int>> GetProductCount()
        {
            int result = await (from prod in _context.Products
                               select prod).CountAsync();
            return result;
        }

        [Route("ordercount")]
        [HttpGet]
        public async Task<ActionResult<int>> GetOrderCount()
        {
            int result = await (from order in _context.Orders.Where(s => s.Status == 2)
                               select order).CountAsync();
            return result;
        }

        [Route("usercount")]
        [HttpGet]
        public async Task<ActionResult<int>> GetUserCount()
        {
            int result = await (from user in _context.AppUsers
                               select user).CountAsync();
            return result;
        }

        [Route("moneycount")]
        [HttpGet]
        public async Task<ActionResult<decimal>> GetMoneyCount()
        {
            var result = await (from order in _context.Orders
                               select order).ToListAsync();
            decimal orderTotalPrice = 0;
            foreach (Order hd in result)
            {
                orderTotalPrice = orderTotalPrice + hd.TotalPrice;
            }
            return orderTotalPrice;
        }
    }
}
