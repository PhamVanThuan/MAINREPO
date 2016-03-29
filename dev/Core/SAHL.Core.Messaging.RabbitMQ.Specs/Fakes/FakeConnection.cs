using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace SAHL.Core.Messaging.RabbitMQ.Specs.Fakes
{
    public class FakeConnection : IQueueConnection
    {
        public Guid ConnectionId
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void Connect()
        {
            throw new NotImplementedException();
        }

        public IModel CreateModel()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public bool IsConnected()
        {
            throw new NotImplementedException();
        }

        public bool Reconnect()
        {
            throw new NotImplementedException();
        }

        IRabbitMQModel IQueueConnection.CreateModel()
        {
            throw new NotImplementedException();
        }
    }
}
