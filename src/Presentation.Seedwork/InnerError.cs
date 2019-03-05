using Newtonsoft.Json;

namespace Presentation.Seedwork
{
    public class InnerError
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("innererror")]
        public InnerError InnerErrorObj { get; set; }

        // 其他属性
    }
}