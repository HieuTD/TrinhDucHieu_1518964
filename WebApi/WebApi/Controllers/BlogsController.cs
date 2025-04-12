using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WebApi.DTOs.Blogs;
using WebApi.EF;
using WebApi.Helper;
using WebApi.Helper.FileStorage;
using WebApi.Models;
using static System.Net.Mime.MediaTypeNames;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogsController : ControllerBase
    {
        private readonly DBcontext _context;
        private const string LIST_IMAGE_BLOG = "Images/list-image-blog";
        private readonly IFileStorageService _storageService;
        public BlogsController(DBcontext context, IFileStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BlogViewModel>>> GetAllBlogs()
        {
            var blogs = _context.Blogs.Select(b => new BlogViewModel()
            {
                Id = b.Id,
                Title = b.Title,
                Description = b.Description,
                Image = _context.BlogImages.Where(s => s.BlogId == b.Id).Select(s => s.Name).FirstOrDefault(),
                UserName = _context.AppUsers.Where(s => s.Id == b.UserId).Select(s => s.FirstName + " " + s.LastName).FirstOrDefault(),
                CreatedAt = b.CreatedAt
            });
            return await blogs.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Blog>> AddBlog([FromForm] BlogCreateRequest request)
        {
            Blog blog = new Blog();
            blog.Title = request.Title;
            blog.Description = request.Description;
            blog.UserId = request.UserId;
            blog.CreatedAt = DateTime.Now;
            if (request.files != null)
            {
                blog.BlogImages = new List<BlogImage>()
                {
                    new BlogImage
                    {
                        CreatedAt = DateTime.Now,
                        Name = await this.SaveFile(request.files),
                    }
                };
            }
            _context.Blogs.Add(blog);
            await _context.SaveChangesAsync();
            return Ok();
        }

        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileBlogAsync(file.OpenReadStream(), fileName);
            return "/" + LIST_IMAGE_BLOG + "/" + fileName;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBlog(int id, [FromForm] BlogCreateRequest upload)
        {
            var listImage = new List<BlogImage>();
            Blog blog = new Blog();
            blog = await _context.Blogs.FindAsync(id);
            blog.Title = upload.Title;
            blog.Description = upload.Description;
            blog.UserId = upload.UserId;
            blog.UpdatedAt = DateTime.Now;

            if (upload.files != null)
            {
                var image = await _context.BlogImages.FirstOrDefaultAsync(i => i.BlogId == id);
                //Nếu trước đó chưa có ảnh thì tạo ảnh mới
                if (image == null)
                {
                    blog.BlogImages = new List<BlogImage>()
                    {
                        new BlogImage()
                        {
                            CreatedAt = DateTime.Now,
                            Name = await this.SaveFile(upload.files),
                        }
                    };
                }
                //Nếu đã có ảnh thì xóa ảnh cũ và cập nhật ảnh mới
                else
                {
                    _context.BlogImages.Remove(image);
                    await _storageService.DeleteFileBlogAsync(image.Name);
                    await _context.SaveChangesAsync();
                    blog.BlogImages = new List<BlogImage>()
                    {
                        new BlogImage()
                        {
                            CreatedAt = DateTime.Now,
                            Name = await this.SaveFile(upload.files),
                        }
                    };
                }
            }
            else // xu li khi khong cap nhat hinh
            {
                var image = _context.BlogImages.Where(s => s.BlogId == id).ToList();
                blog.BlogImages = image;
            };
            _context.Blogs.Update(blog);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBlog(int id)
        {
            var image = await _context.BlogImages.FirstOrDefaultAsync(s => s.BlogId == id);
            if (image != null)
            {
                _context.BlogImages.Remove(image);
                await _storageService.DeleteFileBlogAsync(image.Name);
            }

            var blog = await _context.Blogs.FindAsync(id);
            _context.Blogs.Remove(blog);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
