using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.DTOs.Orders;
using WebApi.EF;
using System.Linq
using Microsoft.EntityFrameworkCore;
using WebApi.Models;
using Microsoft.AspNetCore.SignalR;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly DBcontext _context;

        public OrdersController(DBcontext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderViewModel>>> GetAllOrders()
        {
            var rs = from order in _context.Orders
                     join user in _context.AppUsers
                     on order.UserId equals user.Id
                     select new OrderViewModel()
                     {
                         Id = order.Id,
                         Description = order.Description,
                         CreatedAt = order.CreatedAt,
                         Status = order.Status,
                         TotalPrice = order.TotalPrice,
                         FullName = user.FirstName + ' ' + user.LastName,
                     };
            return await rs.ToListAsync();
        }

        [HttpPut("updateStatus/{id}")]
        public async Task<IActionResult> UpdateStatusOrder(int id, OrderViewModel hd)
        {
            var rs = await _context.Orders.FindAsync(id);
            rs.Status = (int)hd.Status;
            _context.Orders.Update(rs);
            await _context.SaveChangesAsync();
            //await _hubContext.Clients.All.BroadcastMessage();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            OrderDetail[] orderDetail;
            orderDetail = _context.OrderDetails.Where(s => s.OrderId == id).ToArray();
            _context.OrderDetails.RemoveRange(orderDetail);
            Order order;
            order = await _context.Orders.FindAsync(id);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
