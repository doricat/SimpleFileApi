using System.IO;

namespace Application.File
{
    public class FileInfoDTO
    {
        public string ContentType { get; set; }

        public Stream Stream { get; set; }
    }
}