using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.DTOs.Brands;
using WebApi.DTOs.Categories;
using WebApi.EF;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly DBcontext _context;

        public CategoriesController(DBcontext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryViewModel>>> GetAllCategories()
        {
            return await _context.Categories
                .Select(i => new CategoryViewModel()
                {
                    Id = i.Id,
                    CategoryName = i.Name
                }).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryViewModel>> GetCategoryById(int id)
        {
            var rs = await _context.Categories.FindAsync(id);
            if (rs == null)
            {
                return NotFound();
            }
            var category = new CategoryViewModel()
            {
                Id = rs.Id,
                CategoryName = rs.Name
            };
            return category;
        }

        [HttpPost]
        public async Task<ActionResult<Category>> AddCategory([FromForm] CategoryCreateRequest request)
        {
            var category = new Category();
            category.Name = request.CategoryName;
            category.CreatedAt = DateTime.Now;
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return Ok(category);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromForm] CategoryCreateRequest request)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(b => b.Id == id);
            category.Name = request.CategoryName;
            category.UpdatedAt = DateTime.Now;
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
            return Ok(category);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            Product[] product;
            product = await _context.Products.Where(s => s.CategoryId == id).ToArrayAsync();
            Size[] sizes;
            sizes = await _context.Sizes.Where(s => s.CategoryId == id).ToArrayAsync();
            Color[] colors;
            colors = await _context.Colors.Where(s => s.CategoryId == id).ToArrayAsync();
            var category = await _context.Categories.FindAsync(id);

            if (product == null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
            else
            {
                _context.Sizes.RemoveRange(sizes);
                _context.Products.RemoveRange(product);
                _context.Colors.RemoveRange(colors);
                await _context.SaveChangesAsync();
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
            return Ok();
        }
    }
}
