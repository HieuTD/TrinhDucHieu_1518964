using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.DTOs.Coupon;
using WebApi.EF;
using WebApi.Helper;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponsController : ControllerBase
    {
        private readonly DBcontext _context;

        public CouponsController(DBcontext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CouponViewModel>>> GetAllCoupons()
        {
            return await _context.Coupons
                .Select(i => new CouponViewModel()
                {
                    Id = i.Id,
                    CouponName = i.Name,
                    Discount = i.Discount
                }).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult> AddCoupon([FromForm] CouponCreateRequest request)
        {
            Coupon coupon = new Coupon();
            coupon.Name = StringHelper.RandomString(5);
            coupon.Discount = request.Discount;
            coupon.CreatedAt = DateTime.Now;
            _context.Add(coupon);
            await _context.SaveChangesAsync();
            return Ok(coupon);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCoupon([FromForm] CouponCreateRequest request, int id)
        {
            Coupon coupon = await _context.Coupons.FindAsync(id);
            coupon.Name = StringHelper.RandomString(5);
            coupon.Discount = request.Discount;
            coupon.UpdatedAt = DateTime.Now;

            _context.Update(coupon);
            await _context.SaveChangesAsync();
            return Ok(coupon);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCoupon(int id)
        {
            Coupon coupon = await _context.Coupons.FindAsync(id);
            _context.Coupons.Remove(coupon);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
