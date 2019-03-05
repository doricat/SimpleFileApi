using System;
using System.Collections.Generic;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;

namespace Application.File
{
    public class DefaultImageFileProcessor : IImageFileProcessor
    {
        public DefaultImageFileProcessor(IImageSharpProcessActionAdapter adapter)
        {
            Adapter = adapter ?? throw new ArgumentNullException(nameof(adapter));
        }

        public IImageSharpProcessActionAdapter Adapter { get; }

        public Stream Process(Stream stream, string contentType, IList<string> expr)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (expr == null) throw new ArgumentNullException(nameof(expr));

            var result = new MemoryStream();
            stream.Seek(0, SeekOrigin.Begin);
            using (var image = Image.Load(stream))
            {
                var action = Adapter.Build(expr);
                if (action != null)
                {
                    image.Mutate(action);

                    var encoder = GetEncoder(contentType);
                    image.Save(result, encoder);
                }
                else
                {
                    stream.Seek(0, SeekOrigin.Begin);
                    stream.CopyTo(result);
                }
            }

            return result;
        }

        private static IImageEncoder GetEncoder(string contentType)
        {
            var type = contentType.ToLower();
            switch (type)
            {
                case "image/jpeg":
                case "image/jpg":
                    return new JpegEncoder();

                case "image/png":
                    return new PngEncoder();

                default:
                    throw new ArgumentOutOfRangeException(nameof(contentType)); // 暂不支持其他格式
            }
        }
    }
}