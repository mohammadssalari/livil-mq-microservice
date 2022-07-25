namespace livil_mq_microservice.RabibitMq
{
    /// <summary>
    ///     RabbitMQ Wrapper Interface
    /// </summary>
    public interface IRabbitMq
    {
        /// <summary>
        ///     Pushes Messages to QueueName ack is ON
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="messages"></param>
        /// <param name="queueName"></param>
        /// <returns></returns>
        void PushMessagesToQueue<T>(IEnumerable<T> messages);

        /// <summary>
        ///     send a Meesage to the QueName
        /// </summary>
        /// <param name="message"></param>
        /// <param name="queueName"></param>
        /// <typeparam name="T"></typeparam>
        void PushMessageToQueue<T>(T message);

        /// <summary>
        ///     is Raised if the Constructor Channelname recieves a Message
        /// </summary>
        event EventHandler OnDataRecieved;
    }
}