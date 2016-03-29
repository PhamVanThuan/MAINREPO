namespace SAHL.Core.Messaging
{
    public class MessageRoute : IMessageRoute
    {
        public MessageRoute(string exchangeName, string queueName)
        {
            this.ExchangeName = exchangeName;
            this.QueueName = queueName;
        }

        public string ExchangeName { get; protected set; }
        public string QueueName { get; protected set; }
    }
}