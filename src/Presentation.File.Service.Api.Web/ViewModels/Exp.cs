using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Presentation.File.Service.Api.Web.Extensions;

namespace Presentation.File.Service.Api.Web.ViewModels
{
    [ModelBinder(typeof(ExpModelBinder))]
    public class Exp : IValidatableObject
    {
        private string[] _operators;

        /// <summary>
        /// eg:@resize_w,_h@crop_x,_y,_w,_h@rotate_90
        /// </summary>
        public string Action { get; set; }

        public IList<string> ToExp()
        {
            var result = new List<string>();
            foreach (var s in _operators)
            {
                var array = s.Split('_');
                if (array.Any())
                {
                    for (var i = array.Length - 1; i >= 0; i--)
                    {
                        result.Add(array[i].Replace(",", ""));
                    }
                }
            }

            return result;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var result = new List<ValidationResult>();
            _operators = Action.Substring(1).Split('@');
            foreach (var s in _operators)
            {
                if (!Regex.IsMatch(s, @"^(@?resize_([1-9]|\d{2,3}|40[0-9][0-6]),_([1-9]|\d{2,3}|40[0-9][0-6]))|(@?crop_([1-9]|\d{2,3}|40[0-9][0-6]),_([1-9]|\d{2,3}|40[0-9][0-6]),_([1-9]|\d{2,3}|40[0-9][0-6]),_([1-9]|\d{2,3}|40[0-9][0-6]))|(@?rotate_(0|90|180|270))$"))
                {
                    result.Add(new ValidationResult("操作符不满足约束。", new[] {nameof(Action)}));
                    break;
                }
            }

            return result;
        }
    }
}