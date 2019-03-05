using System;
using System.Collections.Generic;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Application.File
{
    public interface IImageSharpProcessActionAdapter
    {
        Action<IImageProcessingContext<Rgba32>> Build(IList<string> expr);
    }
}