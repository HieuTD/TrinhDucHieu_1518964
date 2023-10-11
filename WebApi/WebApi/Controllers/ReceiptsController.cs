using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.DTOs.Receipts;
using WebApi.EF;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using System;
using WebApi.Helper;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceiptsController : ControllerBase
    {
        private readonly DBcontext _context;

        public ReceiptsController(DBcontext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReceiptViewModel>>> GetAllReceipts()
        {
            var query = from ncc in _context.Suppliers

                     join pnh in _context.Receipts
                     on ncc.Id equals pnh.SupplierId

                     join us in _context.AppUsers
                     on pnh.UserId equals us.Id

                     select new ReceiptViewModel()
                     {
                         Id = pnh.Id,
                         Name = pnh.Name,
                         CreatedAt = pnh.CreatedAt,
                         Description = pnh.Description,
                         TotalPrice = pnh.TotalPrice,
                         UserName = us.FirstName + ' ' + us.LastName,
                         SupplierName = ncc.Name,
                     };
            return await query.ToListAsync();
        }

        [HttpPost]
        public async Task<IActionResult> AddReceipt(ReceiptCreateRequest uploadPhieuNhap)
        {
            Receipt phieuNhap = new Receipt()
            {
                UserId = uploadPhieuNhap.UserId,
                CreatedAt = DateTime.Now,
                SupplierId = int.Parse(uploadPhieuNhap.SupplierId),
                Name = StringHelper.RandomString(7),
                TotalPrice = uploadPhieuNhap.TotalPrice,
                Description = uploadPhieuNhap.Description,
            };
            _context.Add(phieuNhap);
            await _context.SaveChangesAsync();
            List<ReceiptDetail> listctpn = new List<ReceiptDetail>();
            foreach (var chitietupload in uploadPhieuNhap.ReceiptDetails)
            {
                ReceiptDetail ctpn = new ReceiptDetail();
                ctpn.ProdDetailId = StringHelper.ProdDetailIdHandle(chitietupload.ProdDetailName);
                ctpn.TotalPrice = chitietupload.ProdDetailPrice * chitietupload.Quantity;
                ctpn.ReceiptId = phieuNhap.Id;
                ctpn.Amonut = chitietupload.Quantity;
                ProductDetail spbt = await _context.ProductDetails.FindAsync(StringHelper.ProdDetailIdHandle(chitietupload.ProdDetailName));
                //Cập nhật lại số lượng hàng trong kho
                spbt.Stock = spbt.Stock + chitietupload.Quantity;
                _context.ProductDetails.Update(spbt);
                listctpn.Add(ctpn);
                _context.ReceiptDetails.Add(ctpn);
                await _context.SaveChangesAsync();
            }
            //_context.Receipts.Update(phieuNhap);
            await _context.SaveChangesAsync();
            //await _hubContext.Clients.All.BroadcastMessage();
            return Ok();
        }
    }
}
