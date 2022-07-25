namespace livil_mq_microservice.Models;

public interface IRecievingApiConfig
{
    public string? BaseUrl { get; set; }
    public string? Resource { get; set; }
}