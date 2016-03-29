using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Specifications;
using SAHL.Common.Logging;
using SAHL.Communication;
using SAHL.Shared.Messages;
using Rhino.Mocks;
using Machine.Fakes;

namespace SAHL.Common.BusinessModel.Specs.Logging
{
    public class When_LogOnExitMethod_IsCalled_Publish_Message_Onto_Bus : LogFakeBase
    {
        Establish Context = () =>
        {
            messageBus = An<IMessageBus>();
            messageBusLoggerConfiguration = new MessageBusLoggerConfiguration();
            methodName = "myMethod";
            message = String.Empty;

            logMessage = new LogMessage(LogMessageType.Warning, "", message, methodName);
            logger = new MessageBusLogger(messageBus, messageBusLoggerConfiguration);
        };
        Because of = () =>
        {
            logger.LogOnExitMethod(methodName);
        };
        It Should_Publish_A_LogMessage = () =>
        {
            // check the log message is type info
            messageBus.WasToldTo(x => x.Publish(Arg<LogMessage>.Matches(y => y.Message == message && y.MethodName == methodName && y.LogMessageTypeCompare(LogMessageType.Info))));
        };
    }
}
