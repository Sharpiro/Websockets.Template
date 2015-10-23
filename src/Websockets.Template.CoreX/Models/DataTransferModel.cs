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
        public string Data { get; set; }
        [JsonProperty("socketId")]
        public string SocketId { get; set; }
        [JsonProperty("socketNumber")]
        public int SocketNumber { get; set; }
        [JsonProperty("applicationId")]
        public string ApplicationId { get; set; }
    }
}
