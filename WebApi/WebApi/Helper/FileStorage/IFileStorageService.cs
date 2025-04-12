using System.IO;
using System.Threading.Tasks;

namespace WebApi.Helper.FileStorage
{
    public interface IFileStorageService
    {
        string GetFileUrl(string fileName);

        Task SaveFileProductAsync(Stream mediaBinaryStream, string fileName);

        Task DeleteFileProductAsync(string fileName);

        Task SaveFileBlogAsync(Stream mediaBinaryStream, string fileName);

        Task DeleteFileBlogAsync(string fileName);
    }
}
