using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Presentation.Seedwork;

namespace Presentation.File.Service.Api.Web.ViewModels
{
    public class ImageFileUploadModel : IValidatableObject
    {
        public IFormFile File { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var result = new List<ValidationResult>();
            do
            {
                if (File == null)
                {
                    result.Add(new ValidationResult("没有检测到上传的文件，请确保name=\"file\"。", new[] {nameof(File)}));
                    break;
                }

                const int size = 5;
                if (File.Length > size * 1024 * 1024) // 5MB
                {
                    result.Add(new ValidationResult($"当前接口限制最大文件为{size}MB", new[] {nameof(File)}));
                    break;
                }

                if (!ContentTypes.ImageContentTypes.ContainsKey(File.ContentType.ToLower()))
                {
                    result.Add(new ValidationResult($"当前接口支持的文件格式为：{string.Join(",", ContentTypes.ImageContentTypes.Select(x => x.Value))}", new[] {nameof(File)}));
                }
            } while (false);

            return result;
        }
    }
}