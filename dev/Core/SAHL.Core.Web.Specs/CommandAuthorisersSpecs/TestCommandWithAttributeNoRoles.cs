using SAHL.Core.Services;
using SAHL.Core.Services.Attributes;
using System;

namespace SAHL.Core.Web.Specs.CommandAuthorisersSpecs
{
    [AuthorisedCommand()]
    public class TestCommandWithAttributeNoRoles : IServiceCommand
    {
        public Guid Id
        {
            get;
            set;
        }
    }
}