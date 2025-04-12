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
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.IdentityModel.Protocols;
using Microsoft.Extensions.Configuration;
using WebApi.Helper.Common;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly DBcontext _context;
        private IConfiguration Configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrdersController(DBcontext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            Configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
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
                         TypePayment = order.TypePayment
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
                        Status = h.Status,
                        TypePayment = h.TypePayment
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
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };

            var rs = await _context.Orders.Where(d => d.Id == id).FirstOrDefaultAsync();
            rs.AppUser = await _context.AppUsers.Where(d => d.Id == rs.UserId).FirstOrDefaultAsync();
            var json = JsonSerializer.Serialize(rs, options);
            return Ok(json);
        }

        [HttpPost]
        public async Task<ActionResult<Order>> AddOrder(OrderCreateRequest request)
        {
            Order order = new Order()
            {
                Status = 0,
                Description = request.Description,
                UserId = request.UserId,
                CreatedAt = DateTime.Now,
                Address = request.Address,
                TotalPrice = request.TotalPrice,
                TypePayment = (int)request.TypePayment
            };
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            var cart = _context.Carts.Where(d => d.UserId == request.UserId).ToList();
            for (int i = 0; i < cart.Count; i++)
            {
                var thisSanPhamBienThe = _context.ProductVariants.Find(cart[i].ProdVariantId);
                OrderDetail cthd = new OrderDetail();
                cthd.ProdId = cart[i].ProdId;
                cthd.ProdVariantId = cart[i].ProdVariantId;
                cthd.OrderId = order.Id;
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

            string urlPayment = "";
            if(request.TypePayment == 1)
            {
                urlPayment = await UrlPayment(0, order.Id);
                var url = new
                {
                    urlVNPay = urlPayment
                };
                return Ok(url);
            }
            else
            {
                return Ok();
            }
        }

        private async Task<string> UrlPayment(int TypePayment, int orderId)
        {
            var urlPayment = "";
            var order = _context.Orders.FirstOrDefault(o => o.Id == orderId);
            //Get Config Info
            string vnp_Returnurl = Configuration.GetValue<string>("Vnpay:vnp_Returnurl"); //URL nhan ket qua tra ve 
            string vnp_Url = Configuration.GetValue<string>("Vnpay:vnp_Url"); //URL thanh toan cua VNPAY 
            string vnp_TmnCode = Configuration.GetValue<string>("Vnpay:vnp_TmnCode"); //Ma định danh merchant kết nối (Terminal Id)
            string vnp_HashSecret = Configuration.GetValue<string>("Vnpay:vnp_HashSecret"); //Secret Key

            //Build URL for VNPAY
            VnPayLibrary vnpay = new VnPayLibrary();

            vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            vnpay.AddRequestData("vnp_Amount", ((long)order.TotalPrice * 100).ToString()); //Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán là 100,000 VND (một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần (khử phần thập phân), sau đó gửi sang VNPAY là: 10000000
            if (TypePayment == 1)
            {
                vnpay.AddRequestData("vnp_BankCode", "VNPAYQR");
            }
            else if (TypePayment == 2)
            {
                vnpay.AddRequestData("vnp_BankCode", "VNBANK");
            }
            else if (TypePayment == 3)
            {
                vnpay.AddRequestData("vnp_BankCode", "INTCARD");
            }

            vnpay.AddRequestData("vnp_CreateDate", order.CreatedAt?.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress(_httpContextAccessor));
            vnpay.AddRequestData("vnp_Locale", "vn");
            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang:" + order.Id);
            vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other

            vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
            vnpay.AddRequestData("vnp_TxnRef", order.Id.ToString()); // Mã tham chiếu của giao dịch tại hệ thống của merchant. Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY. Không được trùng lặp trong ngày

            urlPayment = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
            return urlPayment;
        }
    }
}
