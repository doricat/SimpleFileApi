using System.Threading.Tasks;

namespace Domain.File
{
    public interface IFileMetadataRepository
    {
        Task SaveAsync(FileMetadata file);

        Task<FileMetadata> GetAsync(long id);
    }
}