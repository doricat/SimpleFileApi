using System.IO;
using System.Threading.Tasks;

namespace Application.File
{
    public interface IFileService
    {
        Task<long> SaveAsync(Stream stream, string contentType);

        Task<FileInfoDTO> GetAsync(long id);
    }
}