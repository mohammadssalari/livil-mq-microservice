namespace livil_mq_microservice.RabibitMq
{
    /// <summary>
    ///     RabbitMQConfiguration
    /// </summary>
    public class RabbitMqConfig
    {
        /// <summary>
        ///     The Server on Witch the RabbitMq Service is Running
        /// </summary>
        public string HostName { get; set; }

        /// <summary>
        ///     UserName for Joining the RabbitMqServer
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        ///     The Used Password for Joining the RabbitMqServer
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        ///     VirtualHost
        /// </summary>
        public string VirtualHost { get; set; }

        //Sets the REcieving Channel
        public string RecieveChannel { get; set; }
        //Sets the Send Channel 
        public string SenderChannel { get; set; }
    }
}