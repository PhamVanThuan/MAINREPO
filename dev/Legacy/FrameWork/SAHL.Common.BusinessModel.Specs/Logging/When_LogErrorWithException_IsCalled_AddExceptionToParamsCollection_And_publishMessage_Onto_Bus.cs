using Machine.Specifications;
using Machine.Fakes;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAHL.Shared.Messages;
using SAHL.Communication;
using SAHL.Common.Logging;

namespace SAHL.Common.BusinessModel.Specs.Logging
{
	public class When_LogErrorWithException_IsCalled_AddExceptionToParamsCollection_And_publishMessage_Onto_Bus : LogFakeBase
	{
		protected static Exception ex;
		protected static Dictionary<string, object> parameters;
		Establish Context = () =>
		{
			messageBus = An<IMessageBus>();
			messageBusLoggerConfiguration = new MessageBusLoggerConfiguration();
			methodName = "myMethod";
			message = "my Message";
			ex = new ArgumentNullException("test");

			logMessage = new LogMessage(LogMessageType.Error, "", message, methodName);
			logger = new MessageBusLogger(messageBus, messageBusLoggerConfiguration);
		};
		Because of = () =>
		{
			parameters = new Dictionary<string, object>();
			logger.LogErrorMessageWithException(methodName, message, ex, parameters);
		};
		It Should_Push_Ex_Into_Params = () =>
		{
			parameters.ContainsKey(Logger.EXCEPTION).ShouldBeTrue();
		};
		It Should_Publish_A_LogMessage = () =>
		{
			// check the log emssage is type info
			messageBus.WasToldTo(x => x.Publish(Arg<LogMessage>.Matches(y => y.Message == message && y.MethodName == methodName && y.LogMessageTypeCompare(LogMessageType.Error))));
		};

	}
}
