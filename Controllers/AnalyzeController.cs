using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace livil_mq_microservice.Controllers
{
    //This could be done in a minimal Approach API but for readability i chose to do a full blown api
    
    [Route("api/[controller]")]
    [ApiController]
    public class AnalyzeController : ControllerBase
    {
        

        /// <summary>
        /// One Word is Taken from the Body of Post and after proccessing its send to the SendChannel of RabbitMQ 
        /// </summary>
        /// <param name="postText"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post([FromBody] string postText)
        {
           Log.Information("Post Method Called");
            var random = new Random();
            var splittetAndTrimmedText = postText.Split(new char[] {'.', ',', '?'}).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
            var analyzedText = splittetAndTrimmedText.Skip(random.Next(splittetAndTrimmedText.Count()))
                .FirstOrDefault();
            Log.Information("Analyzed Text = {text}",analyzedText);
            return Ok(analyzedText);

        }
    }
}
