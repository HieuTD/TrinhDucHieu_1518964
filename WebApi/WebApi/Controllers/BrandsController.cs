using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.DTOs.Brands;
using WebApi.EF;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private readonly DBcontext _context;

        public BrandsController(DBcontext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BrandViewModel>>> GetAllBrands()
        {
            return await _context.Brands
                .Select(i => new BrandViewModel()
                {
                    Id = i.Id,
                    BrandName = i.Name
                }).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BrandViewModel>> GetBrandById(int id)
        {
            var rs = await _context.Brands.FindAsync(id);
            if (rs == null)
            {
                return NotFound();
            }
            var brand = new BrandViewModel()
            {
                Id = rs.Id,
                BrandName = rs.Name
            };
            return brand;
        }

        [HttpPost]
        public async Task<ActionResult<BrandViewModel>> AddBrand([FromForm] BrandCreateRequest request)
        {
            var brand = new Brand();
            brand.Name = request.BrandName;
            brand.CreatedAt = DateTime.Now;
            _context.Brands.Add(brand);
            await _context.SaveChangesAsync();
            return Ok(brand);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBrand(int id, [FromForm] BrandCreateRequest request)
        {
            var brand = await _context.Brands.FirstOrDefaultAsync(b => b.Id == id);
            brand.Name = request.BrandName;
            brand.UpdatedAt = DateTime.Now;
            _context.Brands.Update(brand);
            await _context.SaveChangesAsync();
            return Ok(brand);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBrand(int id)
        {
            Product[] product;
            product = await _context.Products.Where(b => b.BrandId == id).ToArrayAsync();
            var brand = await _context.Brands.FindAsync(id);
            if (product == null)
            {
                _context.Brands.Remove(brand);
                await _context.SaveChangesAsync();
            }
            else
            {
                _context.Products.RemoveRange(product);
                await _context.SaveChangesAsync();
                _context.Brands.Remove(brand);
                await _context.SaveChangesAsync();
            }
            return Ok();
        }
    }
}
