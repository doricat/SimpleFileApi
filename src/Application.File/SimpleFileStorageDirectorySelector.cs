using System;
using System.IO;
using System.Threading.Tasks;

namespace Application.File
{
    public class SimpleFileStorageDirectorySelector : IFileStorageDirectorySelector
    {
        public SimpleFileStorageDirectorySelector(LocalStorageFileServiceOptions options)
        {
            Options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public LocalStorageFileServiceOptions Options { get; }

        public Task<string> SelectDirectoryAsync()
        {
            var path = Options.RootDirectory;

            if (string.IsNullOrWhiteSpace(path))
            {
                throw new InvalidOperationException();
            }

            var dir = path.StartsWith(".") ? Path.GetFullPath(path) : path;

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            return Task.FromResult(dir);
        }
    }
}