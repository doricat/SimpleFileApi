using System.Collections.Generic;

namespace Presentation.Seedwork
{
    public class ContentTypes
    {
        public static readonly IDictionary<string, string> ImageContentTypes = new Dictionary<string, string>
        {
            {"image/jpg", "jpg"},
            {"image/jpeg", "jpeg"},
            {"image/png", "png"}
        };
    }
}