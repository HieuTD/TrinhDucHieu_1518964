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
using WebApi.DTOs.ProductVariants;

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
                ctpn.ProdVariantId = StringHelper.ProdVariantIdHandle(chitietupload.ProdVariantName);
                ctpn.TotalPrice = chitietupload.ProdVariantPrice * chitietupload.Quantity;
                ctpn.ReceiptId = phieuNhap.Id;
                ctpn.Amonut = chitietupload.Quantity;
                ctpn.CreatedAt = DateTime.Now;
                ProductVariant spbt = await _context.ProductVariants.FindAsync(StringHelper.ProdVariantIdHandle(chitietupload.ProdVariantName));
                //Cập nhật lại số lượng hàng trong kho
                spbt.Stock = spbt.Stock + chitietupload.Quantity;
                spbt.UpdatedAt = DateTime.Now;
                _context.ProductVariants.Update(spbt);
                listctpn.Add(ctpn);
                _context.ReceiptDetails.Add(ctpn);
                await _context.SaveChangesAsync();
            }
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReceiptDetailViewModel>> GetReceiptDetailById(int id)
        {
            var listDetail = from spbt in _context.ProductVariants
                             join sp in _context.Products
                             on spbt.ProdId equals sp.Id
                             join l in _context.Categories
                             on sp.CategoryId equals l.Id
                             join m in _context.Colors
                             on spbt.ColorId equals m.Id
                             join s in _context.Sizes
                             on spbt.SizeId equals s.Id
                             join ctpn in _context.ReceiptDetails
                             on spbt.Id equals ctpn.ProdVariantId
                             select new GetListProdVariantByReceiptIdVM()
                             {
                                 Id = spbt.Id,
                                 ProductName = sp.Name + " " + s.Name + " " + m.Name,
                                 OriginalPrice = (decimal)sp.OriginalPrice,
                                 Quantity = ctpn.Amonut,
                                 TotalPrice = ctpn.TotalPrice,
                                 ReceiptId = (int)ctpn.ReceiptId
                             };
            var kb = (from phieunhap in _context.Receipts
                      join us in _context.AppUsers
                      on phieunhap.UserId equals us.Id
                      join ncc in _context.Suppliers
                      on phieunhap.SupplierId equals ncc.Id
                      select new ReceiptDetailViewModel()
                      {
                          Id = phieunhap.Id,
                          Description = phieunhap.Description,
                          CreatedAt = phieunhap.CreatedAt,
                          Name = phieunhap.Name,
                          TotalPrice = phieunhap.TotalPrice,
                          UserName = us.FirstName + " " + us.LastName,
                          Supplier = new Supplier()
                          {
                              Id = ncc.Id,
                              Name = ncc.Name,
                              Address = ncc.Address,
                              Description = ncc.Description,
                              PhoneNumber = ncc.PhoneNumber,
                          },
                          ListProdVariants = (List<GetListProdVariantByReceiptIdVM>)listDetail.Where(s => s.ReceiptId == id),
                      });
            return await kb.FirstOrDefaultAsync(s => s.Id == id);
        }
    }
}
