using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.DTOs.Blogs;
using WebApi.EF;
using WebApi.Helper;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogsController : ControllerBase
    {
        private readonly DBcontext _context;

        public BlogsController(DBcontext context)
        {
            _context = context;
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
            //Notification notification = new Notification()
            //{
            //    TenSanPham = request.TieuDe,
            //    TranType = "Add"
            //};
            //_context.Notifications.Add(notification);
            var file = request.files.ToArray();
            _context.Blogs.Add(blog);
            await _context.SaveChangesAsync();
            if (request.files != null)
            {
                for (int i = 0; i < file.Length; i++)
                {
                    if (file[i].Length > 0 && file[i].Length < 5120)
                    {
                        BlogImage imageBlog = new BlogImage();
                        _context.BlogImages.Add(imageBlog);
                        await _context.SaveChangesAsync();
                        imageBlog.Name = await FileHelper.UploadImageAndReturnFileNameAsync(null, request, "blog", (IFormFile[])request.files, i);
                        imageBlog.BlogId = blog.Id;
                        _context.BlogImages.Update(imageBlog);
                        await _context.SaveChangesAsync();
                    }
                }
            }
            //await _hubContext.Clients.All.BroadcastMessage();
            return Ok();
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

            //Notification notification = new Notification()
            //{
            //    TenSanPham = upload.TieuDe,
            //    TranType = "Edit"
            //};
            //_context.Notifications.Add(notification);
            BlogImage[] images = _context.BlogImages.Where(s => s.BlogId == id).ToArray();
            _context.BlogImages.RemoveRange(images);
            BlogImage image = new BlogImage();
            var file = upload.files.ToArray();
            var imageBlogs = _context.BlogImages.ToArray().Where(s => s.BlogId == id);
            foreach (var i in imageBlogs)
            {
                FileHelper.DeleteFileOnTypeAndNameAsync("blog", i.Name);
            }
            if (upload.files != null)
            {
                for (int i = 0; i < file.Length; i++)
                {
                    if (file[i].Length > 0 && file[i].Length < 5120)
                    {
                        listImage.Add(new BlogImage()
                        {
                            Name = await FileHelper.UploadImageAndReturnFileNameAsync(null, upload, "blog", (IFormFile[])upload.files, i),
                            BlogId = blog.Id,
                        });
                    }
                }
            }
            else // xu li khi khong cap nhat hinh
            {
                List<BlogImage> List;
                List = _context.BlogImages.Where(s => s.BlogId == id).ToList();
                foreach (BlogImage img in List)
                    listImage.Add(new BlogImage()
                    {
                        Name = img.Name,
                        BlogId = blog.Id,
                    }); ;
            };
            blog.BlogImages = listImage;
            _context.Blogs.Update(blog);
            await _context.SaveChangesAsync();
            //await _hubContext.Clients.All.BroadcastMessage();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBlog(int id)
        {
            var imageBlogs = _context.BlogImages.ToArray().Where(s => s.BlogId == id);
            foreach (var i in imageBlogs)
            {
                FileHelper.DeleteFileOnTypeAndNameAsync("blog", i.Name);
            }
            _context.BlogImages.RemoveRange(imageBlogs);
            var blog = await _context.Blogs.FindAsync(id);
            _context.Blogs.Remove(blog);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
