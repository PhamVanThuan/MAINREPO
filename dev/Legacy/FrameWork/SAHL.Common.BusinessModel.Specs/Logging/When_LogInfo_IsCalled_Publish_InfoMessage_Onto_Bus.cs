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
    public class When_LogInfo_IsCalled_Publish_InfoMessage_Onto_Bus : LogFakeBase
    {
        Establish Context = () =>
        {
            messageBus = An<IMessageBus>();
            messageBusLoggerConfiguration = new MessageBusLoggerConfiguration();
            methodName = "myMethod";
            message = "my Message";

            logMessage = new LogMessage(LogMessageType.Info, "", message, methodName);
            logger = new MessageBusLogger(messageBus, messageBusLoggerConfiguration);
        };
        Because of = () =>
        {
            logger.LogInfoMessage(methodName, message);
        };
        It Should_Publish_A_LogMessage = () =>
        {
            // check the log emssage is type info
            messageBus.WasToldTo(x => x.Publish(Arg<LogMessage>.Matches(y => y.Message == message && y.MethodName == methodName && y.LogMessageTypeCompare(LogMessageType.Info))));
        };
    }
}
