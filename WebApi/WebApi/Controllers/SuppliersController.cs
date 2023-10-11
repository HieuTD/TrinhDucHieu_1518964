using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebApi.DTOs.Suppliers;
using WebApi.EF;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuppliersController : ControllerBase
    {
        private readonly DBcontext _context;

        public SuppliersController(DBcontext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SupplierViewModel>>> GetAllSuppliers()
        {
            return await _context.Suppliers
                .Select(i => new SupplierViewModel()
                {
                    Id = i.Id,
                    SupplierName = i.Name,
                    PhoneNumber = i.PhoneNumber,
                    Description = i.Description,
                    Address = i.Address
                }).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Supplier>> GetSupplierById(int id)
        {
            return await _context.Suppliers.FindAsync(id);
        }

        [HttpPost]
        public async Task<IActionResult> AddSupplier([FromForm] SupplierCreateRequest request)
        {
            var supplier = new Supplier();
            supplier.Name = request.SupplierName;
            supplier.PhoneNumber = request.PhoneNumber;
            supplier.Description = request.Description;
            supplier.Address = request.Address;
            supplier.CreatedAt = DateTime.Now;
            _context.Suppliers.Add(supplier);
            await _context.SaveChangesAsync();
            return Ok(supplier);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSupplier([FromForm] SupplierCreateRequest request, int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            supplier.Name = request.SupplierName;
            supplier.PhoneNumber = request.PhoneNumber;
            supplier.Description = request.Description;
            supplier.Address = request.Address;
            supplier.UpdatedAt = DateTime.Now;
            _context.Suppliers.Update(supplier);
            await _context.SaveChangesAsync();
            return Ok(supplier);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSupplier(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            _context.Suppliers.Remove(supplier);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
