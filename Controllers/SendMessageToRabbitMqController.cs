﻿using livil_mq_microservice.Models;
using livil_mq_microservice.RabibitMq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


//This could be done in a minimal Approach API but for readability i chose to do a full blown api
namespace livil_mq_microservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SendMessageToRabbitMqController : ControllerBase
    {
        private readonly IRabbitMq _rabbitMq;
        //The RabbitMq Dependency will be injected here
        public SendMessageToRabbitMqController(IRabbitMq rabbitMq)
        {
            _rabbitMq = rabbitMq;
        }

        [HttpPost]
        public IActionResult Post([FromBody] RqInputMessage rqInputMessage)
        {
            try
            {
                _rabbitMq.PushMessageToQueue(rqInputMessage.Content,rqInputMessage.ReplyQueue ?? throw new ArgumentNullException(nameof(rqInputMessage.ReplyQueue)));
                return Ok(rqInputMessage.Content);
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
           
        }
    }
}
