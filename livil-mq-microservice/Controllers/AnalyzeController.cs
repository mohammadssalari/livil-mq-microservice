using livil_mq_microservice.Models;
using livil_mq_microservice.RabibitMq;
using Microsoft.AspNetCore.Mvc;

namespace livil_mq_microservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnalyzeController : ControllerBase
    {
        private readonly IRabbitMq _rabbitMq;

        public AnalyzeController(IRabbitMq rabbitMq)
        {
            _rabbitMq = rabbitMq;
        }

        /// <summary>
        /// One Word is Taken from the Body of Post and after proccessing its send to the SendChannel of RabbitMQ 
        /// </summary>
        /// <param name="PostText"></param>
        /// <returns></returns>
        public IActionResult Post([FromBody] string PostText)
        {
            var random = new System.Random();
            var splittetAndTrimmedText = PostText.Split(new[] {'.', ',', '?'}).Where(x => !string.IsNullOrWhiteSpace(x));
            var analyzedText = splittetAndTrimmedText.Skip(random.Next(splittetAndTrimmedText.Count()))
                .FirstOrDefault();
            
            return Ok(analyzedText);

        }
    }
}
