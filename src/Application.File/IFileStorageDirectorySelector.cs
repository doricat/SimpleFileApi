using System.Threading.Tasks;

namespace Application.File
{
    public interface IFileStorageDirectorySelector
    {
        Task<string> SelectDirectoryAsync();
    }
}