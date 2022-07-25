#nullable enable
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace livil_mq_microservice.RabibitMq
{
    /// <summary>
    ///     a Wrapper fór RabbitMQ
    /// </summary>
    public class RabbitMq : IRabbitMq
    {
        private readonly RabbitMqConfig _config;
        private readonly EventingBasicConsumer? consumer;

        private readonly ConnectionFactory factory;

        private readonly IModel? receivingChannel;

        private readonly IConnection? recievingConnection;

        private IConnection ConnectionFactory { get; } = null!;
        /// <summary>
        ///     For intializing via non DI
        /// </summary>
        /// <param name="config"></param>
        public RabbitMq(RabbitMqConfig config)
        {
            _config = config;
            factory = new ConnectionFactory
            {
                HostName = config.HostName
            };
            if (!string.IsNullOrEmpty(config.VirtualHost)) factory.VirtualHost = config.VirtualHost;
            if (!string.IsNullOrEmpty(config.Password)) factory.Password = config.Password;
            if (!string.IsNullOrEmpty(config.UserName)) factory.UserName = config.UserName;
            ConnectionFactory = factory.CreateConnection();
            recievingConnection = factory.CreateConnection();
            receivingChannel = recievingConnection.CreateModel();
            receivingChannel.QueueDeclare(config.RecieveChannel, false, false);
            consumer = new EventingBasicConsumer(receivingChannel);
            consumer.Received += (model, ea) => { OnDataRecieved?.Invoke(this, ea); };
            receivingChannel.BasicConsume(config.RecieveChannel,
                true,
                consumer);
        }
        /// <summary>
        ///     For intializing via non DI
        /// </summary>
        /// <param name="config"></param>
        public RabbitMq(IOptions<RabbitMqConfig> config)
        {
            _config = config.Value;
            factory = new ConnectionFactory
            {
                HostName = _config.HostName
            };
            if (!string.IsNullOrEmpty(_config.VirtualHost)) factory.VirtualHost = _config.VirtualHost;
            if (!string.IsNullOrEmpty(_config.Password)) factory.Password = _config.Password;
            if (!string.IsNullOrEmpty(_config.UserName)) factory.UserName = _config.UserName;
            ConnectionFactory = factory.CreateConnection();
            recievingConnection = factory.CreateConnection();
            receivingChannel = recievingConnection.CreateModel();
            receivingChannel.QueueDeclare(_config.RecieveChannel, false, false);
            consumer = new EventingBasicConsumer(receivingChannel);
            consumer.Received += (model, ea) => { OnDataRecieved?.Invoke(this, ea); };
            receivingChannel.BasicConsume(_config.RecieveChannel,
                true,
                consumer);
        }


        /// <inheritdoc />
        public event EventHandler? OnDataRecieved;


        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="messages"></param>
        /// <exception cref="ArgumentNullException">messages,queuename</exception>
        public void PushMessagesToQueue<T>(IEnumerable<T> messages)
        {
            if (string.IsNullOrEmpty(_config.SenderChannel)) throw new ArgumentNullException(nameof(_config.SenderChannel));
            foreach (var message in messages) PushMessageToQueue(message);
        }

        /// <summary>
        ///     pusges a single Message to the queueName
        /// </summary>
        /// <param name="message"></param>
        /// <typeparam name="T"></typeparam>
        /// <exception cref="ArgumentNullException"></exception>
        public void PushMessageToQueue<T>(T message)
        {
            if (string.IsNullOrEmpty(_config.SenderChannel)) throw new ArgumentNullException(nameof(_config.SenderChannel));
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare(_config.SenderChannel, false, false);
            var serialzedMessage = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(serialzedMessage);
            channel.BasicPublish("", _config.SenderChannel, true, null, body);
        }
    }
}