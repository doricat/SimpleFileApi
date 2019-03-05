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
            if (!Directory.Exists(Options.RootDirectory))
                Directory.CreateDirectory(Options.RootDirectory);
            return Task.FromResult(Options.RootDirectory);
        }
    }
}