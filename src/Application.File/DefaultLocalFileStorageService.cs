using System;
using System.IO;
using System.Threading.Tasks;

namespace Application.File
{
    public class DefaultLocalFileStorageService : ILocalFileStorageService
    {
        public DefaultLocalFileStorageService(IFileStorageDirectorySelector directorySelector)
        {
            DirectorySelector = directorySelector ?? throw new ArgumentNullException(nameof(directorySelector));
        }

        public IFileStorageDirectorySelector DirectorySelector { get; }

        public async Task<string> SaveAsync(Stream stream)
        {
            var directory = await DirectorySelector.SelectDirectoryAsync();
            var fileName = Path.Combine(directory, Path.GetRandomFileName()).Replace("\\", "/");
            using (var file = System.IO.File.Create(fileName))
            {
                stream.Seek(0, SeekOrigin.Begin);
                await stream.CopyToAsync(file);
            }
            return fileName;
        }
    }
}