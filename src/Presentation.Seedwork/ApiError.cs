using System.Collections.Generic;
using Newtonsoft.Json;

namespace Presentation.Seedwork
{
    public class ApiError
    {
        public ApiError(string code, string message)
        {
            Code = code;
            Message = message;
        }

        [JsonRequired]
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonRequired]
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("target")]
        public string Target { get; set; }

        [JsonProperty("details")]
        public IList<ApiError> Details { get; set; }

        [JsonProperty("innererror")]
        public InnerError InnerError { get; set; }
    }
}