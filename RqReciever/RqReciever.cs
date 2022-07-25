using System.Text;
using livil_mq_microservice.Models;
using livil_mq_microservice.RabibitMq;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;
using RestSharp;
using Serilog;

namespace livil_mq_microservice.RqReciever
{
    public class RqReciever : BackgroundService
    {
        private readonly IRabbitMq _rabbitMq;
        private readonly IRecievingApiConfig _apiconfig;

        /// <summary>
        /// This is the Constructor wich recieves the Configured RabbitMq as Dependency Injection as a Container
        /// </summary>
        /// <param name="rabbitMq"></param>
        /// <param name="apiconfig"></param>
        public RqReciever(IRabbitMq rabbitMq, IRecievingApiConfig apiconfig)
        {
            _rabbitMq = rabbitMq;
            _apiconfig = apiconfig;
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
            Log.Information("Recieved This from RabbitMqChannel Not Parsed: {content}", content);
            Log.Information($"Recieved This from RabbitMqChannel Parsed: {content}", JsonConvert.DeserializeObject<RqInputMessage>(content));
            SendDataToApi(content);
        }

        private void SendDataToApi(string content)
        {
            var _baseurl = "http://irgendwas";
            string _recource = "rew";
            var client = new RestClient(_apiconfig.BaseUrl);
            Method _method = Method.Post;
            var request = new RestRequest(_apiconfig.Resource, _method);
            request.AddBody(JsonConvert.DeserializeObject<RqInputMessage>(content)!);
            var res = client.PostAsync(request);
            if (res.Result.IsSuccessful)
                Log.Information("Sent Data to APi Successful");
            else
                Log.Fatal("Sending Data to Api Unsuccessful");


        }
    }
}
