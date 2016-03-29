using SAHL.Core.Services;
using SAHL.Core.Services.Attributes;
using System;

namespace SAHL.Core.Web.Specs.CommandAuthorisersSpecs
{
    [AuthorisedCommand(Roles = "Me, You")]
    public class TestQueryWithAttributeWithRoles : IServiceQuery
    {
        public Guid Id
        {
            get;
            set;
        }
    }
}