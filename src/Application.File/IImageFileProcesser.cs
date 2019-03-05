using System.Collections.Generic;
using System.IO;

namespace Application.File
{
    public interface IImageFileProcessor
    {
        Stream Process(Stream stream, string contentType, IList<string> expr);
    }
}