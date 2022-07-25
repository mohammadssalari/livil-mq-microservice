using System.Text;
using livil_mq_microservice.Models;
using livil_mq_microservice.RabibitMq;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;
using Serilog;

namespace livil_mq_microservice.RqReciever
{
    public class RqReciever : BackgroundService
    {
        private readonly IRabbitMq _rabbitMq;

        /// <summary>
        /// This is the Constructor wich recieves the Configured RabbitMq as Dependency Injection as a Container
        /// </summary>
        /// <param name="rabbitMq"></param>
        public RqReciever(IRabbitMq rabbitMq)
        {
            _rabbitMq = rabbitMq;
        }

        //This Function Grabs the Event of the RabbitMq Service 
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {

            _rabbitMq.OnDataRecieved += (model, ea) =>
            {
                var content = Encoding.UTF8.GetString(((BasicDeliverEventArgs)ea).Body.ToArray());
                HandleMessage(content);
            };
            return Task.CompletedTask;
        }

        //This function Handles the4 Message
        private void HandleMessage(string content)
        {
           //Show the RabbitMqMessage to The Terminal and Log
            Log.Information("Recieved This from RabbitMqChannel Not Parsed: {content}",content);
            Log.Information($"Recieved This from RabbitMqChannel Parsed: {content}",JsonConvert.DeserializeObject<RQInputMessage>(content));
        }
    }
}
