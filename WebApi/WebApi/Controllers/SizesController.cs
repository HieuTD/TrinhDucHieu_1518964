using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.DTOs.Sizes;
using WebApi.EF;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SizesController : ControllerBase
    {
        private readonly DBcontext _context;

        public SizesController(DBcontext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SizeViewModel>>> GetAllSizes()
        {
            var rs = from cate in _context.Categories
                     join size in _context.Sizes
                     on cate.Id equals size.CategoryId
                     select new SizeViewModel()
                     {
                         Id = size.Id,
                         SizeName = size.Name,
                         CategoryId = (int)cate.Id,
                         CategoryName = cate.Name
                     };
            return await rs.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SizeViewModel>> GetSizeById(int id)
        {
            var rs = await _context.Sizes.FindAsync(id);
            if (rs == null)
            {
                return NotFound();
            }
            var size = new SizeViewModel()
            {
                Id = rs.Id,
                SizeName = rs.Name
            };
            return size;
        }

        [HttpPost]
        public async Task<ActionResult<SizeViewModel>> AddSize([FromForm] SizeCreateRequest request)
        {
            Size size = new Size()
            {
                Name = request.SizeName,
                CategoryId = request.CategoryId,
            };
            _context.Sizes.Add(size);
            //Notification notification = new Notification()
            //{
            //    TenSanPham = upload.TenSize,
            //    TranType = "Add"
            //};
            //_context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
            //await _hubContext.Clients.All.BroadcastMessage();
            return CreatedAtAction("GetSizeById", new { id = size.Id }, size);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSize(int id, [FromForm] SizeCreateRequest request)
        {
            var size = await _context.Sizes.FindAsync(id);
            size.Name = request.SizeName;
            size.CategoryId = request.CategoryId;
            _context.Sizes.Update(size);
            //Notification notification = new Notification()
            //{
            //    TenSanPham = upload.TenSize,
            //    TranType = "Edit"
            //};
            //_context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
            //await _hubContext.Clients.All.BroadcastMessage();
            return Ok(size);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSize(int id)
        {
            var size = await _context.Sizes.FindAsync(id);
            if (size == null)
            {
                return NotFound();
            }
            _context.Sizes.Remove(size);
            //Notification notification = new Notification()
            //{
            //    TenSanPham = size.TenSize,
            //    TranType = "Delete"
            //};
            //_context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
            //await _hubContext.Clients.All.BroadcastMessage();
            return Ok();
        }
    }
}
