using SAHL.Core.Services;
using SAHL.Core.Services.Attributes;
using System;

namespace SAHL.Core.Web.Specs.CommandAuthorisersSpecs
{
    [AuthorisedCommand(Roles = "Me, You")]
    public class TestCommandWithAttributeWithRoles : IServiceCommand
    {
        public Guid Id
        {
            get;
            set;
        }
    }
}