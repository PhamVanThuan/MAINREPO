using SAHL.Core.Rules;
using SAHL.Services.Interfaces.WorkflowAssignmentDomain.Commands;
using SAHL.Services.WorkflowAssignmentDomain.Rules;
using SAHL.Services.WorkflowAssignmentDomain.Rules.Models;
using StructureMap.Configuration.DSL;

namespace SAHL.Config.Services.WorkflowAssignmentDomain.Server
{
    public class IocRegistry : Registry
    {
        public IocRegistry()
        {
            For<IDomainRuleManager<UserHasCapabilityRuleModel>>()
                .Use<DomainRuleManager<UserHasCapabilityRuleModel>>();
            For<IDomainRuleManager<ReturnInstanceToLastUserInCapabilityCommand>>()
                .Use<DomainRuleManager<ReturnInstanceToLastUserInCapabilityCommand>>();
        }
    }
}
