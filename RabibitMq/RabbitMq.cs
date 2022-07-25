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
        //Globally Available in this Class 
        private readonly RabbitMqConfig _config;
        private readonly EventingBasicConsumer? _consumer;
        private readonly ConnectionFactory _factory;
        private readonly IModel? _receivingChannel;
        private readonly IConnection? _recievingConnection;

        private IConnection ConnectionFactory { get; } = null!;
        /// <summary>
        ///     For intializing via non DI
        /// </summary>
        /// <param name="config"></param>
        public RabbitMq(RabbitMqConfig config)
        {
            _config = config;
            _factory = new ConnectionFactory
            {
                HostName = config.HostName
            };
            if (!string.IsNullOrEmpty(config.VirtualHost)) _factory.VirtualHost = config.VirtualHost;
            if (!string.IsNullOrEmpty(config.Password)) _factory.Password = config.Password;
            if (!string.IsNullOrEmpty(config.UserName)) _factory.UserName = config.UserName;
            ConnectionFactory = _factory.CreateConnection();
            _recievingConnection = _factory.CreateConnection();
            _receivingChannel = _recievingConnection.CreateModel();
            _receivingChannel.QueueDeclare(config.RecieveChannel, false, false);
            _consumer = new EventingBasicConsumer(_receivingChannel);
            _consumer.Received += (model, ea) => { OnDataRecieved?.Invoke(this, ea); };
            _receivingChannel.BasicConsume(config.RecieveChannel,
                true,
                _consumer);
        }
        /// <summary>
        ///     For intializing via DI
        /// </summary>
        /// <param name="config"></param>
        public RabbitMq(IOptions<RabbitMqConfig> config)
        {
            _config = config.Value;
            _factory = new ConnectionFactory
            {
                HostName = _config.HostName
            };
            if (!string.IsNullOrEmpty(_config.VirtualHost)) _factory.VirtualHost = _config.VirtualHost;
            if (!string.IsNullOrEmpty(_config.Password)) _factory.Password = _config.Password;
            if (!string.IsNullOrEmpty(_config.UserName)) _factory.UserName = _config.UserName;
            ConnectionFactory = _factory.CreateConnection();
            _recievingConnection = _factory.CreateConnection();
            _receivingChannel = _recievingConnection.CreateModel();
            _receivingChannel.QueueDeclare(_config.RecieveChannel, false, false);
            _consumer = new EventingBasicConsumer(_receivingChannel);
            _consumer.Received += (model, ea) => { OnDataRecieved?.Invoke(this, ea); };
            _receivingChannel.BasicConsume(_config.RecieveChannel,
                true,
                _consumer);
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
            using var connection = _factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare(_config.SenderChannel, false, false);
            var serialzedMessage = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(serialzedMessage);
            channel.BasicPublish("", _config.SenderChannel, true, null, body);
        }

        /// <summary>
        ///     pusges a single Message to the queueName
        /// </summary>
        /// <param name="message"></param>
        /// <param name="queuename"></param>
        /// <typeparam name="T"></typeparam>
        /// <exception cref="ArgumentNullException"></exception>
        public void PushMessageToQueue<T>(T message,string queuename)
        {
            if (string.IsNullOrEmpty(queuename)) throw new ArgumentNullException(nameof(queuename));
            using var connection = _factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare(queuename, false, false);
            var serialzedMessage = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(serialzedMessage);
            channel.BasicPublish("", queuename, true, null, body);
        }
    }
}