using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using System;
using WebApi.DTOs.Products;
using WebApi.DTOs.Blogs;

namespace WebApi.Helper
{
    public class FileHelper
    {
        public static bool DeleteFileOnTypeAndNameAsync(string type, string name)
        {
            try
            {
                if (type == "product")
                {
                    File.Delete(Path.Combine("wwwroot/Images/list-image-product", name));
                    return true;
                }
                else
                {
                    File.Delete(Path.Combine("wwwroot/Images/list-image-blog", name));
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static async Task<string> UploadImageAndReturnFileNameAsync(ProductCreateRequest sanpham, BlogCreateRequest blog, string type, IFormFile[] file, int i)
        {
            if (type == "product")
            {
                var path = Path.Combine(
                      Directory.GetCurrentDirectory(), "wwwroot/Images/list-image-product",
                     sanpham.Name + i + "." + file[i].FileName.Split(".")[file[i].FileName.Split(".").Length - 1]);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file[i].CopyToAsync(stream);
                }
                return sanpham.Name + i + "." + file[i].FileName.Split(".")
                            [file[i].FileName.Split(".").Length - 1];
            }
            else
            {
                var path = Path.Combine(
                     Directory.GetCurrentDirectory(), "wwwroot/Images/list-image-blog",
                    blog.Title + i + "." + file[i].FileName.Split(".")[file[i].FileName.Split(".").Length - 1]);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file[i].CopyToAsync(stream);
                }
                return blog.Title + i + "." + file[i].FileName.Split(".")
                            [file[i].FileName.Split(".").Length - 1];
            }
        }
    }
}
