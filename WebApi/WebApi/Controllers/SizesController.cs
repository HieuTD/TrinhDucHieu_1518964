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
using System;

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
                CreatedAt = DateTime.Now
            };
            _context.Sizes.Add(size);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetSizeById", new { id = size.Id }, size);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSize(int id, [FromForm] SizeCreateRequest request)
        {
            var size = await _context.Sizes.FindAsync(id);
            size.Name = request.SizeName;
            size.CategoryId = request.CategoryId;
            size.UpdatedAt = DateTime.Now;
            _context.Sizes.Update(size);
            await _context.SaveChangesAsync();
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
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("listSizeCategory")]
        public async Task<ActionResult<IEnumerable<ListSizeCategory>>> GetListSizeCategory()
        {
            var rs = from size in _context.Sizes
                     join cate in _context.Categories
                     on size.CategoryId equals cate.Id
                     select new ListSizeCategory()
                     {
                         Id = size.Id,
                         Name = size.Name + " " + cate.Name
                     };
            var kbs = rs.ToListAsync();
            return await kbs;
        }


        [HttpPost("listSizeByColor")]
        public async Task<ActionResult> GetListSizeByColor(GetListSizeByColorRequest request)
        {
            var categoryId = _context.Products.Where(d => d.Id == request.ProdId).Select(d => d.CategoryId).SingleOrDefault();
            var mauloai = request.ColorName + categoryId;
            var colorId = _context.Colors.Where(d => d.Name + d.CategoryId == mauloai && d.CategoryId == categoryId).Select(d => d.Id).SingleOrDefault();
            var listSizeId = _context.ProductVariants.Where(d => d.ColorId == colorId && d.ProdId == request.ProdId).Select(d => d.SizeId.ToString()).ToList();
            var rs = _context.Sizes.Where(d => listSizeId.Contains(d.Id.ToString())).Select(
                d => new
                {
                    size = d.Name
                }).ToList();
            return Ok(rs);
        }

        [HttpPost("listSizeByProdId")]
        public async Task<ActionResult> GetListSizeByProdId(GetListSizeByColorRequest request)
        {
            var categoryId = _context.Products.Where(d => d.Id == request.ProdId).Select(d => d.CategoryId).SingleOrDefault();
            var listSize = _context.Sizes.Where(s => s.CategoryId == categoryId).Select(
                d => new
                {
                    Id = d.Id,
                    Name = d.Name
                }).ToList();
            return Ok(listSize);
        }
    }
}
