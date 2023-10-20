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
using System;

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

        //Lấy chỉ tiết hóa đơn cho ADMIN
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


        [HttpGet("listorderbyuserid/{id}")]
        public async Task<ActionResult> GetListOrderByUserId(string id)
        {
            var result = await _context.Orders.Where(d => d.UserId == id).ToListAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetOrderById(int id)
        {
            var rs = await _context.Orders.Where(d => d.Id == id).FirstOrDefaultAsync();
            rs.AppUser = await _context.AppUsers.Where(d => d.Id == rs.UserId).FirstOrDefaultAsync();
            return Ok(rs);
        }

        [HttpPost]
        public async Task<ActionResult<Order>> AddOrder(Order hd)
        {
            Order hoaDon = new Order()
            {
                Status = 0,
                Description = hd.Description,
                UserId = hd.UserId,
                CreatedAt = DateTime.Now,
                //Tinh = hd.Tinh,
                //Huyen = hd.Huyen,
                //Xa = hd.Xa,
                Address = hd.Address,
                TotalPrice = hd.TotalPrice
            };
            _context.Orders.Add(hoaDon);
            await _context.SaveChangesAsync();
            //NotificationCheckout notification = new NotificationCheckout()
            //{
            //    ThongBaoMaDonHang = hoaDon.Id,
            //};
            //_context.NotificationCheckouts.Add(notification);
            var cart = _context.Carts.Where(d => d.UserId == hd.UserId).ToList();
            for (int i = 0; i < cart.Count; i++)
            {
                var thisSanPhamBienThe = _context.ProductVariants.Find(cart[i].ProdVariantId);
                OrderDetail cthd = new OrderDetail();
                cthd.ProdId = cart[i].ProdId;
                cthd.ProdVariantId = cart[i].ProdVariantId;
                cthd.OrderId = hoaDon.Id;
                cthd.Price = (decimal)cart[i].Price;
                cthd.Quantity = cart[i].Quantity;
                cthd.TotalPrice = (decimal)cart[i].Price * cart[i].Quantity;
                cthd.Size = cart[i].Size;
                cthd.Color = cart[i].Color;
                //Khi hoàn tất đặt đơn hàng thì số lượng sản phẩm trong kho sẽ trừ đi số sản phẩm mua trong giỏ hàng
                thisSanPhamBienThe.Stock = thisSanPhamBienThe.Stock - cart[i].Quantity;
                _context.ProductVariants.Update(thisSanPhamBienThe);
                _context.OrderDetails.Add(cthd);
                _context.Carts.Remove(cart[i]);
                await _context.SaveChangesAsync();
            };
            //await _hubContext.Clients.All.BroadcastMessage();
            return Ok(1);
        }
    }
}
