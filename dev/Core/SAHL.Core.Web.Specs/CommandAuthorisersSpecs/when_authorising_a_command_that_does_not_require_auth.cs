﻿using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Identity;
using SAHL.Core.Services;
using SAHL.Core.Web.Services;
using System.Security.Principal;

namespace SAHL.Core.Web.Specs.CommandAuthorisersSpecs
{
    public class when_authorising_a_command_that_does_not_require_auth : WithFakes
    {
        private static IHttpCommandAuthoriser commandAuthoriser;
        private static IHostContext hostContext;
        private static IServiceCommand commandToAuthorise;
        private static IPrincipal currentUser;
        private static SAHL.Core.Web.Services.HttpCommandAuthoriser.AuthToken authToken;

        private Establish context = () =>
        {
            commandToAuthorise = new TestCommandWithoutAttribute();
            hostContext = An<IHostContext>();

            currentUser = new GenericPrincipal(new GenericIdentity("someone"), new string[] { });
            hostContext.WhenToldTo(x => x.GetUser())
                .Return(currentUser);
            commandAuthoriser = new HttpCommandAuthoriser(hostContext);
        };

        private Because of = () =>
        {
            authToken = commandAuthoriser.AuthoriseCommand(commandToAuthorise);
        };

        private It should_authorise_the_command = () =>
        {
            authToken.IsAuthorised().ShouldBeTrue();
        };
    }
}