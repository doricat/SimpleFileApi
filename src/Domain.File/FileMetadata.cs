using Domain.Seedwork;

namespace Domain.File
{
    public class FileMetadata : Entity<long>
    {
        public string Filename { get; set; }

        public string ContentType { get; set; }

        public int Size { get; set; }
    }
}