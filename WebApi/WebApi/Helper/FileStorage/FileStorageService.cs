using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Threading.Tasks;

namespace WebApi.Helper.FileStorage
{
    public class FileStorageService : IFileStorageService
    {
        private readonly string _userContentFolder;
        private const string LIST_IMAGE_PRODUCT = "Images\\list-image-product";
        public FileStorageService(IWebHostEnvironment webHostEnvironment)
        {
            _userContentFolder = Path.Combine(webHostEnvironment.WebRootPath, LIST_IMAGE_PRODUCT);
        }

        public string GetFileUrl(string fileName)
        {
            return $"/{LIST_IMAGE_PRODUCT}/{fileName}";
        }

        public async Task SaveFileAsync(Stream mediaBinaryStream, string fileName)
        {
            var filePath = Path.Combine(_userContentFolder, fileName);
            using var output = new FileStream(filePath, FileMode.Create);
            await mediaBinaryStream.CopyToAsync(output);
        }

        public async Task DeleteFileAsync(string fileName)
        {
            var filePath = Path.Combine(_userContentFolder, fileName);
            if (File.Exists(filePath))
            {
                await Task.Run(() => File.Delete(filePath));
            }
        }
    }
}
