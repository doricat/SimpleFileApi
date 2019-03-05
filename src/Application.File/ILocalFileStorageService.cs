using System.IO;
using System.Threading.Tasks;

namespace Application.File
{
    public interface ILocalFileStorageService
    {
        Task<string> SaveAsync(Stream stream);
    }
}