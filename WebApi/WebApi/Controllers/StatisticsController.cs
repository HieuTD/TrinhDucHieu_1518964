using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApi.EF;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;
using System.Collections.Generic;
using WebApi.DTOs.Statistics;
using System.Drawing;

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

        //Thống kê số lần xuất hiện trong giỏ hàng
        [HttpGet("toporderproducts")]
        public async Task<ActionResult<IEnumerable<ListTopOrderProducts>>> GetTopOrderProducts()
        {
            var solanxuathiens = new List<ListTopOrderProducts>();

            var result = await (from sanPham in _context.Products

                                join sanPhamBienThe in _context.ProductVariants
                                on sanPham.Id equals sanPhamBienThe.ProdId

                                join mauSac in _context.Colors 
                                on sanPhamBienThe.ColorId equals mauSac.Id

                                join size in _context.Sizes 
                                on sanPhamBienThe.SizeId equals size.Id

                                join chiTietHoaDon in _context.OrderDetails 
                                on sanPhamBienThe.Id equals chiTietHoaDon.ProdVariantId

                                join hoaDon in _context.Orders 
                                on chiTietHoaDon.OrderId equals hoaDon.Id

                                where hoaDon.Status == 2
                                group new { sanPham, size, mauSac, sanPhamBienThe, chiTietHoaDon } 
                                by new 
                                { 
                                    ProductName = sanPham.Name, 
                                    ColorName = mauSac.Name,
                                    SizeName = size.Name,
                                    sanPhamBienThe.Stock 
                                } 
                                into g
                                orderby g.Sum(x => x.chiTietHoaDon.Quantity) descending
                                select new
                                {
                                    Ten = g.Key.ProductName + " " + g.Key.ColorName + " Size " + g.Key.SizeName,
                                    SoLanXuatHienTrongDonHang = g.Sum(x => x.chiTietHoaDon.Quantity)
                                })
                                .Take(10)
                                .ToListAsync();

            foreach (var item in result)
            {
                solanxuathiens.Add(new ListTopOrderProducts()
                {
                    ProductName = item.Ten,
                    Count = item.SoLanXuatHienTrongDonHang
                });
            }

            return solanxuathiens;
        }

        // Thống kê top doanh thu sản phẩm
        [HttpGet("topprofitproducts")]
        public async Task<ActionResult<IEnumerable<ListTopProfitProducts>>> GetTopProfitProducts()
        {
            var tenspdss = new List<ListTopProfitProducts>();

            var result = await (from sanPham in _context.Products
                                
                                join sanPhamBienThe in _context.ProductVariants
                                on sanPham.Id equals sanPhamBienThe.ProdId

                                join mauSac in _context.Colors 
                                on sanPhamBienThe.ColorId equals mauSac.Id

                                join size in _context.Sizes 
                                on sanPhamBienThe.SizeId equals size.Id

                                join chiTietHoaDon in _context.OrderDetails 
                                on sanPhamBienThe.Id equals chiTietHoaDon.ProdVariantId

                                join hoaDon in _context.Orders 
                                on chiTietHoaDon.OrderId equals hoaDon.Id

                                where hoaDon.Status == 2
                                group new { sanPham, size, mauSac, sanPhamBienThe, chiTietHoaDon }
                                by new
                                {
                                    ProductName = sanPham.Name,
                                    ColorName = mauSac.Name,
                                    SizeName = size.Name,
                                }  
                                into g
                                orderby g.Sum(x => x.chiTietHoaDon.TotalPrice) descending
                                select new
                                {
                                    Ten = g.Key.ProductName + " " + g.Key.ColorName + " Size " + g.Key.SizeName,
                                    DoanhSoCaoNhat = g.Sum(x => x.chiTietHoaDon.TotalPrice)
                                })
                                .Take(10)
                                .ToListAsync();

            foreach (var item in result)
            {
                tenspdss.Add(new ListTopProfitProducts()
                {
                    ProductName = item.Ten,
                    Profit = (decimal)item.DoanhSoCaoNhat
                });
            }

            return tenspdss;
        }

        [HttpGet("topprofitbymonth")]
        public async Task<ActionResult<IEnumerable<ListTopProfitByMonth>>> GetTopProfitByMonth()
        {
            var sells = await _context.Orders.Where(s => s.Status == 2)
                .GroupBy(a => a.CreatedAt.Value.Date.Month)
                .Select(a => new ListTopProfitByMonth 
                { 
                    Profit = a.Sum(b => b.TotalPrice), 
                    Month = a.Key.ToString() 
                })
                .OrderBy(a => a.Profit)
                .ToListAsync();
            return sells;
        }
    }
}
