using Machine.Fakes;
using Machine.Specifications;
using Machine.Specifications.Utility;
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
    public class When_Command_ReRun_Occures : WithFakes
    {
        private static HttpCommandReRun httpCommandReRun;
        private static IHostContextHelper hostContextHelper;
        private static IIocContainer iocContainer;
        private static ILoggerSource loggerSource;
        private static ILogger logger;
        private static ICommandSession commandSession;
        private static IHttpCommandRun httpCommandRun;

        private Establish context = () =>
        {
            hostContextHelper = An<IHostContextHelper>();
            iocContainer = An<IIocContainer>();
            loggerSource = An<ILoggerSource>();
            logger = An<ILogger>();
            commandSession = An<ICommandSession>();
            httpCommandRun = An<IHttpCommandRun>();
            iocContainer.WhenToldTo(x => x.GetInstance<IHttpCommandRun, IHostContext>(Arg.Any<IHostContext>())).Return(httpCommandRun);
            httpCommandReRun = new HttpCommandReRun(hostContextHelper, iocContainer, loggerSource, logger);
        };

        private Because of = () =>
        {
            httpCommandReRun.TryRunCommand(commandSession);
        };

        private It should_call_create_hosted_context = () =>
        {
            hostContextHelper.WasToldTo(
                x => x.CreateHostContextFromUser(Arg.Any<string>(), Arg.Any<IEnumerable<KeyValuePair<string, string>>>()));
        };

        private It should_call_HttpCommandRun_To_Run_Command = () =>
        {
            httpCommandRun.WasToldTo(x => x.RunCommand(Arg.Any<ServiceCommandResult>(), commandSession));
        };
    }
}