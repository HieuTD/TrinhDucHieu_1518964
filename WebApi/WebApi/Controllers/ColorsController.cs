using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.DTOs;
using WebApi.EF;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using WebApi.DTOs.Colors;
using WebApi.Models;
using WebApi.DTOs.Sizes;
using System;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColorsController : ControllerBase
    {
        private readonly DBcontext _context;

        public ColorsController(DBcontext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ColorViewModel>>> GetAllColors()
        {
            var colors = from cate in _context.Categories
                         join color in _context.Colors
                        on cate.Id equals color.CategoryId
                        select new ColorViewModel()
                        {
                            Id = color.Id,
                            CategoryId = cate.Id,
                            CategoryName = cate.Name,
                            ColorName = color.Name
                        };
            return await colors.ToListAsync();
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ColorViewModel>> GetColorById(int id)
        {
            var color = await _context.Colors.FindAsync(id);
            if (color == null)
            {
                return NotFound();
            }

            var colorVm = new ColorViewModel()
            {
                Id = color.Id,
                ColorName = color.Name,
                CategoryId = color.CategoryId
            };
            return colorVm;
        }

        [HttpPost]
        public async Task<ActionResult<ColorViewModel>> AddColor([FromForm] ColorCreateRequest request)
        {
            Color color = new Color()
            {
                Name = request.ColorName,
                CategoryId = request.CategoryId,
                CreatedAt = DateTime.Now
            };
            _context.Colors.Add(color);
            await _context.SaveChangesAsync();
            return Ok(color);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateColor(int id, [FromForm] ColorCreateRequest request)
        {
            var color = await _context.Colors.FindAsync(id);
            color.Name = request.ColorName;
            color.CategoryId = request.CategoryId;
            color.UpdatedAt = DateTime.Now;
            _context.Colors.Update(color);
            await _context.SaveChangesAsync();
            return Ok(color);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteColor(int id)
        {
            var color = await _context.Colors.FindAsync(id);
            if (color == null)
            {
                return NotFound();
            }
            _context.Colors.Remove(color);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("listColorCategory")]
        public async Task<ActionResult<IEnumerable<ListSizeCategory>>> GetListColorCategory()
        {
            var rs = from color in _context.Colors
                     join cate in _context.Categories
                     on color.CategoryId equals cate.Id
                     select new ListSizeCategory()
                     {
                         Id = color.Id,
                         Name = color.Name + " " + cate.Name
                     };
            return await rs.ToListAsync();
        }


        [HttpGet("listcolorbyprodid/{id}")]
        public async Task<ActionResult> GetListColorByProdId(int id)
        {
            var colorId = _context.ProductVariants.Where(d => d.ProdId == id).Select(d => d.ColorId).ToList();
            var rs = _context.Colors.Where(d => colorId.Contains(d.Id)).
                Select(d => new {
                    colorName = d.Name
                });
            return Ok(await rs.ToListAsync());
        }

        [HttpGet("color")]
        public async Task<ActionResult> GetColorName()
        {
            var result = await _context.Colors.Select(d => new
            {
                colorName = d.Name
            }).Distinct().ToListAsync();
            return Ok(result);
        }

        [HttpPost("listColorByProdIdAdmin")]
        public async Task<ActionResult> GetListColorByProdIdAdmin(GetListSizeByColorRequest request)
        {
            var categoryId = _context.Products.Where(d => d.Id == request.ProdId).Select(d => d.CategoryId).SingleOrDefault();
            var listColor = _context.Colors.Where(c => c.CategoryId == categoryId).Select(
                d => new
                {
                    Id = d.Id,
                    Name = d.Name
                }).ToList();
            return Ok(listColor);
        }
    }
}
