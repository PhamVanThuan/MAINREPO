using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Identity;
using SAHL.Core.Logging;
using SAHL.Core.Services.CommandPersistence;
using SAHL.Core.Web.Services;
using System;
using System.Collections.Generic;
using System.Data;

namespace SAHL.Core.Web.Specs.Services
{
    public class When_Command_ReRun_Fails_Again : WithFakes
    {
        private static HttpCommandReRun httpCommandReRun;
        private static IHostContextHelper hostContextHelper;
        private static IIocContainer iocContainer;
        private static ILoggerSource loggerSource;
        private static ILogger logger;
        private static ICommandSession commandSession;
        private static IHttpCommandRun httpCommandRun;
        private static Exception exception;

        private Establish context = () =>
        {
            hostContextHelper = An<IHostContextHelper>();
            iocContainer = An<IIocContainer>();
            loggerSource = An<ILoggerSource>();
            logger = An<ILogger>();
            commandSession = An<ICommandSession>();
            httpCommandRun = An<IHttpCommandRun>();
            exception = new Exception("Test Exception");
            iocContainer.WhenToldTo(x => x.GetInstance<IHttpCommandRun, IHostContext>(Arg.Any<IHostContext>())).Return(httpCommandRun);
            httpCommandRun.WhenToldTo(x => x.RunCommand(Arg.Any<ServiceCommandResult>(), commandSession)).Throw(exception);
            httpCommandReRun = new HttpCommandReRun(hostContextHelper, iocContainer, loggerSource, logger);
        };

        private Because of = () =>
        {
            Catch.Exception(() => httpCommandReRun.TryRunCommand(commandSession));
        };

        private It should_call_create_hosted_context = () =>
        {
            hostContextHelper.WasToldTo(
                x => x.CreateHostContextFromUser(Arg.Any<string>(), Arg.Any<IEnumerable<KeyValuePair<string, string>>>()));
        };

        private It should_call_HttpCommandRun_toRunCommand = () =>
        {
            httpCommandRun.WasToldTo(x => x.RunCommand(Arg.Any<ServiceCommandResult>(), commandSession));
        };

        private It should_fall_into_catch_and_be_logged = () =>
        {
            logger.WasToldTo(x => x.LogErrorWithException(loggerSource, Arg.Any<string>(), "HttpCommandReRun", "Exception while performing an http command", exception, null));
        };
    }
}