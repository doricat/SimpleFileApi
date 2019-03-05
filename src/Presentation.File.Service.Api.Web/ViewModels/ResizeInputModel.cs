using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Presentation.File.Service.Api.Web.ViewModels
{
    public class ResizeInputModel
    {
        [Display(Name = "宽度")]
        [Range(1, 4096, ErrorMessage = "{0}的取值在{1}和{2}之间。")]
        public int Width { get; set; }

        [Display(Name = "高度")]
        [Range(1, 4096, ErrorMessage = "{0}的取值在{1}和{2}之间。")]
        public int Height { get; set; }

        public IList<string> ToExp()
        {
            return new List<string> {Height.ToString(), Width.ToString(), "resize"};
        }
    }
}