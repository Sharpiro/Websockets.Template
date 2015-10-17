using Newtonsoft.Json;

namespace Websockets.Template.CoreX.Models
{
    public class DataTransferModel
    {
        [JsonProperty("dataType")]
        public string DataType { get; set; }
        [JsonProperty("dataTitle")]
        public string DataTitle { get; set; }
        [JsonProperty("data")]
        public dynamic Data { get; set; }
        public string ClientId { get; set; }
    }
}
