using Machine.Fakes;
using Machine.Specifications;
using Rhino.Mocks;
using SAHL.Common.Logging;
using SAHL.Communication;
using SAHL.Shared.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAHL.Common.BusinessModel.Specs.Logging
{
    public class When_LogWarning_IsCalled_Publish_Message_Onto_MessageBus : LogFakeBase
    {
        Establish Context = () =>
        {
            messageBus = An<IMessageBus>();
            messageBusLoggerConfiguration = new MessageBusLoggerConfiguration();
            methodName = "myMethod";
            message = "my Message";

            logMessage = new LogMessage(LogMessageType.Warning, "", message, methodName);
            logger = new MessageBusLogger(messageBus, messageBusLoggerConfiguration);
        };
        Because of = () =>
        {
            logger.LogWarningMessage(methodName, message);
        };
        It Should_Publish_A_LogMessage = () =>
        {
            // check the log emssage is type warning
            messageBus.WasToldTo(x => x.Publish(Arg<LogMessage>.Matches(y => y.Message == message && y.MethodName == methodName && y.LogMessageTypeCompare(LogMessageType.Warning))));
        };
    }
}
