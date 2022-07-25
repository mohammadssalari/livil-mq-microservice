namespace livil_mq_microservice.Models
{
    public class RecievingApiConfig : IRecievingApiConfig
    {
        public string? BaseUrl { get; set; }
        public string? Resource { get; set; }

    }
}
