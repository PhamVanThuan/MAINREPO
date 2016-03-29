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
    public class When_LogOnMethodException_IsCalled_Publish_Onto_Bus : LogFakeBase
    {
        protected static Exception ex;
        Establish Context = () =>
        {
            messageBus = An<IMessageBus>();
            messageBusLoggerConfiguration = new MessageBusLoggerConfiguration();
            methodName = "myMethod";
            message = String.Empty;
            ex = new ArgumentException("test");

            logMessage = new LogMessage(LogMessageType.Error, "", message, methodName);
            logger = new MessageBusLogger(messageBus, messageBusLoggerConfiguration);
        };
        Because of = () =>
        {
            logger.LogOnMethodException(methodName, ex);
        };
        It Should_Publish_A_LogMessage = () =>
        {
            // check the log message is type error
            messageBus.WasToldTo(x => x.Publish(Arg<LogMessage>.Matches(y => y.Message == message && y.MethodName == methodName && y.LogMessageTypeCompare(LogMessageType.Error))));
        };
    }
}
