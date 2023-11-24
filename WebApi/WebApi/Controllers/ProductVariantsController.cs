using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.EF;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using WebApi.Models;
using System;
using WebApi.DTOs.Products;
using WebApi.DTOs.ProductVariants;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductVariantsController : ControllerBase
    {
        private readonly DBcontext _context;

        public ProductVariantsController(DBcontext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductVariantViewModel>>> GetAllProductVariants()
        {
            var query = from pd in _context.ProductVariants

                     join p in _context.Products
                     on pd.ProdId equals p.Id

                     join c in _context.Colors
                     on pd.ColorId equals c.Id
                     
                     join s in _context.Sizes
                     on pd.SizeId equals s.Id
                     select new ProductVariantViewModel()
                     {
                         Id = pd.Id,
                         ProdId = p.Id,
                         ProductName = p.Name,
                         ColorId = c.Id,
                         ColorName = c.Name,
                         SizeId = s.Id,
                         SizeName = s.Name,
                         Stock = pd.Stock
                     };
            return await query.Take(50).ToListAsync();
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ProductVariantViewModel>> GetProductVariantById(int id)
        {
            var prodVariant = await _context.ProductVariants.FindAsync(id);
            if (prodVariant == null)
            {
                return NotFound();
            }
            return new ProductVariantViewModel()
            {
                Id = prodVariant.Id,
                ProdId = prodVariant.Id,
                ColorId = prodVariant.Id,
                SizeId = prodVariant.Id,
                Stock = prodVariant.Stock
            };
        }

        [HttpPost]
        public async Task<ActionResult<ProductVariantViewModel>> AddProductVariant([FromForm] ProductVariantCreateRequest request)
        {
            ProductVariant prodVariant = new ProductVariant();
            prodVariant.Stock = request.Stock;
            prodVariant.ProdId = request.ProdId;
            prodVariant.ColorId = request.ColorId;
            prodVariant.SizeId = request.SizeId;
            prodVariant.CreatedAt = DateTime.Now;

            _context.ProductVariants.Add(prodVariant);
            await _context.SaveChangesAsync();
            return Ok(prodVariant);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProductVariant(int id, [FromForm] ProductVariantCreateRequest request)
        {
            ProductVariant prodVariant = await _context.ProductVariants.FindAsync(id);
            prodVariant.ProdId = request.ProdId;
            prodVariant.ColorId = request.ColorId;
            prodVariant.SizeId = request.SizeId;
            prodVariant.UpdatedAt = DateTime.Now;

            _context.ProductVariants.Update(prodVariant);

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductVariant(int id)
        {
            ProductVariant prodVariant = await _context.ProductVariants.FindAsync(id);
            _context.ProductVariants.Remove(prodVariant);
            await _context.SaveChangesAsync();
            return Ok();
        }


        [HttpGet("listprodvariant/{id}")]
        public async Task<ActionResult<IEnumerable<GetListProdVariantByProdIdVM>>> GetListProdVariantByProdId(int id)
        {
            var kb = from spbt in _context.ProductVariants
                     join sp in _context.Products.Where(s => s.Id == id)
                     on spbt.ProdId equals sp.Id
                     join l in _context.Categories
                     on sp.CategoryId equals l.Id
                     join m in _context.Colors
                     on spbt.ColorId equals m.Id
                     join s in _context.Sizes
                     on spbt.SizeId equals s.Id
                     select new GetListProdVariantByProdIdVM()
                     {
                         Id = spbt.Id,
                         Name = "Id: " + spbt.Id + " Tên: " + sp.Name + " " + s.Name + " " + m.Name,
                         OriginalPrice = (decimal)sp.OriginalPrice,
                     };
            return await kb.ToListAsync();
        }
    }
}
