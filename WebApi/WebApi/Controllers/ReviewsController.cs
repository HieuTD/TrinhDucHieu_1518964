using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using WebApi.EF;
using WebApi.DTOs.Reviews;
using WebApi.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly DBcontext _context;

        public ReviewsController(DBcontext context)
        {
            _context = context;
        }

        [HttpPost()]
        public async Task<ActionResult> AddReview(ReviewCreateRequest request)
        {
            var result = new Review
            {
                CreatedAt = DateTime.Now,
                ProdId = request.ProdId,
                Content = request.Content,
                UserId = request.UserId,
                Rating = request.Rating
            };
            _context.Add(result);
            _context.SaveChanges();
            var listcomment = _context.Reviews.Where(d => d.ProdId == request.ProdId).Select(
                d => new 
                {
                    Content = d.Content,
                    UserName = _context.AppUsers.Where(s => s.Id == d.UserId).Select(s => s.FirstName + " " + s.LastName).SingleOrDefault(),
                    CreatedAt = d.CreatedAt,
                    Rating = d.Rating
                });
            return Ok(await listcomment.ToListAsync());
        }

        [HttpPost("getListReview")]
        public async Task<ActionResult> GetListReview(ReviewCreateRequest request)
        {
            var listcomment = _context.Reviews.Where(d => d.ProdId == request.ProdId).Select(
                 d => new
                 {
                     Content = d.Content,
                     UserName = _context.AppUsers.Where(s => s.Id == d.UserId).Select(s => s.FirstName + " " + s.LastName).SingleOrDefault(),
                     CreatedAt = d.CreatedAt,
                     Rating = d.Rating
                 });
            return Ok(await listcomment.ToListAsync());
        }
    }
}
