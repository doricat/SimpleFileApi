using Newtonsoft.Json;

namespace Presentation.Seedwork
{
    public class ApiResult<T>
    {
        public ApiResult()
        {
        }

        public ApiResult(T value)
        {
            Value = value;
        }

        [JsonProperty("value")]
        public T Value { get; set; }
    }
}