using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SAHL.Shared.Messages;

namespace SAHL.Communication.TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            //var serviceBus = new MassTransitMessageBus(new MassTransitMessageBusConfigurationProvider() { RabbitMQEndpoint = "msmq://localhost/exceptionthrower" });
            //Thread.Sleep(3000);

            //Console.WriteLine("Type 'q' to exit");
            //var quitMessage = String.Empty;
            //while (String.IsNullOrEmpty(quitMessage))
            //{
            //    Console.WriteLine("Press any key to send an exception or 'q' to quit");
            //    quitMessage = Console.ReadLine();
            //    serviceBus.Publish(new LogMessage("application", "source", "correlationid", "methodname", null, null));
            //}
            //Console.ReadLine();
        }
    }
}
