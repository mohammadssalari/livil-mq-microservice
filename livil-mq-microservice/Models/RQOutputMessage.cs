using Newtonsoft.Json;

namespace livil_mq_microservice.Models
{
    public class RQOutputMessage
    {
        [JsonProperty("synopsis")]
        public string Synopsis { get; set; }
    }
}
