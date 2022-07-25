using Newtonsoft.Json;

namespace livil_mq_microservice.Models
{
    public class RQInputMessage
    {
        [JsonProperty("reply_queue")]
        public string Reply_Queue { get; set; }
        [JsonProperty("content")]
        public string Content { get; set; }
    }
}
