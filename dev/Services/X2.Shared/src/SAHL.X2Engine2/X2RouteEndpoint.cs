namespace SAHL.X2Engine2
{
    public class X2RouteEndpoint : IX2RouteEndpoint
    {
        public X2RouteEndpoint(string exchangeName, string queueName)
        {
            this.ExchangeName = exchangeName;
            this.QueueName = queueName;
        }

        public string ExchangeName
        {
            get;
            protected set;
        }

        public string QueueName
        {
            get;
            protected set;
        }
    }
}