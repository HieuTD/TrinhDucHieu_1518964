using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Threading.Tasks;

namespace WebApi.Helper.FileStorage
{
    public class FileStorageService : IFileStorageService
    {
        private readonly string _productImageFolder;
        private const string LIST_IMAGE_PRODUCT = "Images\\list-image-product";

        private readonly string _blogImageFolder;
        private const string LIST_IMAGE_Blog= "Images\\list-image-blog";

        public FileStorageService(IWebHostEnvironment webHostEnvironment)
        {
            _productImageFolder = Path.Combine(webHostEnvironment.WebRootPath, LIST_IMAGE_PRODUCT);
            _blogImageFolder = Path.Combine(webHostEnvironment.WebRootPath, LIST_IMAGE_Blog);
        }

        public string GetFileUrl(string fileName)
        {
            return $"/{LIST_IMAGE_PRODUCT}/{fileName}";
        }

        public async Task SaveFileProductAsync(Stream mediaBinaryStream, string fileName)
        {
            var filePath = Path.Combine(_productImageFolder, fileName);
            using var output = new FileStream(filePath, FileMode.Create);
            await mediaBinaryStream.CopyToAsync(output);
        }

        public async Task DeleteFileProductAsync(string fileName)
        {
            var filePath = Path.Combine(_productImageFolder, fileName);
            if (File.Exists(filePath))
            {
                await Task.Run(() => File.Delete(filePath));
            }
        }

        public async Task SaveFileBlogAsync(Stream mediaBinaryStream, string fileName)
        {
            var filePath = Path.Combine(_blogImageFolder, fileName);
            using var output = new FileStream(filePath, FileMode.Create);
            await mediaBinaryStream.CopyToAsync(output);
        }

        public async Task DeleteFileBlogAsync(string fileName)
        {
            var filePath = Path.Combine(_blogImageFolder, fileName);
            if (File.Exists(filePath))
            {
                await Task.Run(() => File.Delete(filePath));
            }
        }
    }
}
