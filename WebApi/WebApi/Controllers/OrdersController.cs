using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.DTOs.Orders;
using WebApi.EF;
using System.Linq;
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

        [HttpPut("updatestatus/{id}")]
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

        [HttpGet("adminorderdetail/{id}")]
        public async Task<ActionResult<OrderDetailViewModel>> GetOrderDetail(int id)
        {
            var list = new List<ListOrderDetailsViewModel>();
            var productImageTable = (
                from sp in _context.Products
                
                join isp in _context.ProductImages on 
                sp.Id equals isp.ProdId into ispGroup
                from isp in ispGroup.DefaultIfEmpty()

                join spbt in _context.ProductVariants
                on sp.Id equals spbt.ProdId
                
                join sz in _context.Sizes 
                on spbt.SizeId equals sz.Id
                
                join ms in _context.Colors 
                on spbt.ColorId equals ms.Id
                
                join cthd in _context.OrderDetails 
                on spbt.Id equals cthd.ProdVariantId
                
                join hd1 in _context.Orders 
                on cthd.OrderId equals hd1.Id
                where cthd.OrderId == id
                orderby isp.Id
                select new
                {
                    cthd.Id,
                    ProductName = sp.Name,
                    SizeName = sz.Name,
                    ColorName = ms.Name,
                    cthd.Quantity,
                    Price = (decimal)sp.Price,
                    cthd.TotalPrice,
                    RowNum = 1
                }
            ).ToList();

            foreach (var item in productImageTable)
            {
                list.Add(new ListOrderDetailsViewModel()
                {
                    Id = item.Id,
                    ProductName = item.ProductName,
                    Price = item.Price,
                    ColorName = item.ColorName,
                    SizeName = item.SizeName,
                    Quantity = item.Quantity,
                    TotalPrice = (decimal)item.TotalPrice
                });
            }

            var hd = await (
                from h in _context.Orders
                join us in _context.AppUsers on h.UserId equals us.Id
                where h.Id == id
                select new OrderDetailViewModel()
                {
                    Id = h.Id,
                    FullName = us.FirstName + ' ' + us.LastName,
                    Address = us.Address,
                    Email = us.Email,
                    PhoneNumber = us.PhoneNumber,
                    order = new Order()
                    {
                        UserId = us.Id,
                        TotalPrice = h.TotalPrice,
                        Description = h.Description,
                        CreatedAt = h.CreatedAt,
                        Status = h.Status
                    },
                    listOrderDetails = list,
                }
            ).FirstOrDefaultAsync();

            return hd;
        }
    }
}
