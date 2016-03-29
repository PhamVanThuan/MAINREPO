using SAHL.Core;
using SAHL.Core.Logging;
using SAHL.Core.SystemMessages;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.CommandHandlers;
using SAHL.X2Engine2.Communication;
using SAHL.X2Engine2.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.X2Engine2.Node
{
    public class X2NodeManagementMessageProcessor : IX2NodeManagementMessageProcessor
    {
        private IManagementCommandFactory commandFactory;
        private IIocContainer iocContainer;
        private IRawLogger logger;
        private ILoggerSource loggerSource;
        private ILoggerAppSource loggerAppSource;

        public X2NodeManagementMessageProcessor(IManagementCommandFactory commandFactory, IIocContainer iocContainer, IRawLogger logger, ILoggerSource loggerSource, ILoggerAppSource loggerAppSource)
        {
            this.commandFactory = commandFactory;
            this.iocContainer = iocContainer;
            this.logger = logger;
            this.loggerSource = loggerSource;
            this.loggerAppSource = loggerAppSource;
        }

        public ISystemMessageCollection ProcessMessage(IX2NodeManagementMessage message)
        {
            var messages = SystemMessageCollection.Empty();
            IX2ServiceCommandRouter commandHandler = iocContainer.GetInstance<IX2ServiceCommandRouter>();
            var commands = this.commandFactory.CreateCommands(message);
            foreach (var command in commands)
            {
                try
                {
                    messages.Aggregate(commandHandler.HandleCommand((dynamic)command, null));
                }
                catch (Exception ex)
                {
                    logger.LogErrorWithException(this.loggerSource.LogLevel, this.loggerSource.Name, this.loggerAppSource.ApplicationName, "X2 User", "X2NodeManagementMessageProcessor", "Error while handling message", ex);
                }
            }

            return messages;
        }
    }
}
