using System.Collections.Generic;

namespace Presentation.File.Service.Api.Web.ViewModels
{
    public class ImagePatchInputModel
    {
        /// <summary>
        /// 剪裁参数，优先级1
        /// </summary>
        public CropInputModel Crop { get; set; }

        /// <summary>
        /// 缩放参数，优先级0
        /// </summary>
        public ResizeInputModel Resize { get; set; }

        public IList<string> ToExp()
        {
            var result = new List<string>();
            if (Resize != null)
                result.AddRange(Resize.ToExp());

            if (Crop != null)
                result.AddRange(Crop.ToExp());

            return result;
        }
    }
}