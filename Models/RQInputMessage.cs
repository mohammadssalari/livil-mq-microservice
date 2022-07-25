using Newtonsoft.Json;

namespace livil_mq_microservice.Models
{
    public class RqInputMessage
    {
        [JsonProperty("reply_queue")]
        public string? ReplyQueue { get; set; }
        [JsonProperty("content")]
        public string? Content { get; set; }
    }
}
