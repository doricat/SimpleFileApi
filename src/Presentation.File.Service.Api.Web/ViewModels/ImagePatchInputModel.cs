using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Presentation.File.Service.Api.Web.ViewModels
{
    public class ImagePatchInputModel : IValidatableObject
    {
        private Exp _exp;

        public string Action { get; set; }

        public IList<string> ToExp()
        {
            return _exp.ToExp();
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var result = new List<ValidationResult>();
            if (string.IsNullOrWhiteSpace(Action))
            {
                result.Add(new ValidationResult("action不能为空", new[] {"action"}));
            }
            else
            {
                _exp = new Exp(Action);
                var errors = _exp.Validate(validationContext).ToList();
                if (errors.Any())
                {
                    result.AddRange(errors);
                }
            }

            return result;
        }
    }
}