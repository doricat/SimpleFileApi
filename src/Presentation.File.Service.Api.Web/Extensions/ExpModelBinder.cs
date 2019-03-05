using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Presentation.File.Service.Api.Web.ViewModels;

namespace Presentation.File.Service.Api.Web.Extensions
{
    public class ExpModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null) throw new ArgumentNullException(nameof(bindingContext));

            var modelName = bindingContext.ModelName;
            var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);
            if (valueProviderResult == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }

            bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);
            var value = valueProviderResult.FirstValue;
            if (string.IsNullOrEmpty(value) || !value.StartsWith("$action=", StringComparison.OrdinalIgnoreCase))
            {
                return Task.CompletedTask;
            }

            var model = new Exp
            {
                Action = value.Replace("$action=", "")
            };
            bindingContext.Result = ModelBindingResult.Success(model);
            return Task.CompletedTask;
        }
    }
}