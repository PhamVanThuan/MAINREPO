using SAHL.Core.Logging;
using SAHL.Core.Messaging;
using SAHL.Core.X2.Messages;
using SAHL.Core.X2.Messages.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SAHL.X2Engine2.Communication.EasyNetQ
{
    public class EasyNetQNodeManagementSubscriber : IX2NodeManagementSubscriber
    {
        private IRawLogger logger;
        private ILoggerSource loggerSource;
        private ILoggerAppSource loggerAppSource;
        private IMessageBusAdvanced messageBus;
        private IX2NodeManagementMessageProcessor messageProcessor;

        public EasyNetQNodeManagementSubscriber(IMessageBusAdvanced messageBus, IX2NodeManagementMessageProcessor messageProcessor, IRawLogger logger, ILoggerSource loggerSource, ILoggerAppSource loggerAppSource)
        {
            this.logger = logger;
            this.loggerSource = loggerSource;
            this.loggerAppSource = loggerAppSource;

            this.messageBus = messageBus;
            this.messageProcessor = messageProcessor;
        }

        public void Initialise()
        {
            Action<X2NodeManagementMessage> messageHandler = (message) =>
            {
                try
                {
                    this.messageProcessor.ProcessMessage(message);
                    Console.WriteLine(string.Format("NodeManagementSubscriber Handle - Thread {0}, {1}", Thread.CurrentThread.ManagedThreadId, message.ManagementType.ToString()));

                }
                catch (Exception ex)
                {
                    string exceptionMessage = ex.Message;
                    logger.LogErrorWithException(this.loggerSource.LogLevel, this.loggerSource.Name, this.loggerAppSource.ApplicationName, "X2 User", MethodInfo.GetCurrentMethod().Name, exceptionMessage, ex);   
                }
            };

            messageBus.Subscribe<X2NodeManagementMessage>(messageHandler);
        }

        public void Teardown()
        {
            if (messageBus != null)
            {
                messageBus.Dispose();
            }
        }
    }
}
