using System;
using System.IO;
using System.Threading.Tasks;
using Domain.File;
using Domain.Seedwork;

namespace Application.File
{
    public class LocalFileService : IFileService
    {
        public LocalFileService(ILocalFileStorageService localFileStorageService, IFileMetadataRepository fileMetadataRepository, IIdentityGenerator<long> identityGenerator)
        {
            LocalFileStorageService = localFileStorageService ?? throw new ArgumentNullException(nameof(localFileStorageService));
            FileMetadataRepository = fileMetadataRepository ?? throw new ArgumentNullException(nameof(fileMetadataRepository));
            IdentityGenerator = identityGenerator ?? throw new ArgumentNullException(nameof(identityGenerator));
        }

        public ILocalFileStorageService LocalFileStorageService { get; }

        public IFileMetadataRepository FileMetadataRepository { get; }

        public IIdentityGenerator<long> IdentityGenerator { get; }

        public async Task<long> SaveAsync(Stream stream, string contentType)
        {
            var fileName = await LocalFileStorageService.SaveAsync(stream);
            var metadata = new FileMetadata
            {
                ContentType = contentType.ToLower(),
                Size = (int) stream.Length,
                Filename = fileName
            };
            await metadata.GenerateIdAsync(IdentityGenerator);
            await FileMetadataRepository.SaveAsync(metadata);

            return metadata.Id;
        }

        public async Task<FileInfoDTO> GetAsync(long id)
        {
            var metadata = await FileMetadataRepository.GetAsync(id);
            if (metadata == null) return null;
            using (var file = System.IO.File.Open(metadata.Filename, FileMode.Open))
            {
                var result = new FileInfoDTO
                {
                    ContentType = metadata.ContentType,
                    Stream = new MemoryStream()
                };
                await file.CopyToAsync(result.Stream);

                return result;
            }
        }
    }
}