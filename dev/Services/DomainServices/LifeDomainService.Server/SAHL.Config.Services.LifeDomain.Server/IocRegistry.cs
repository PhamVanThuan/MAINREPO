using SAHL.Core.Rules;
using SAHL.Services.Interfaces.LifeDomain.Models;
using StructureMap.Configuration.DSL;

namespace SAHL.Config.Services.LifeDomain.Server
{
    public class IocRegistry : Registry
    {
        public IocRegistry()
        {
            For<IDomainRuleManager<DisabilityClaimModel>>().Use<DomainRuleManager<DisabilityClaimModel>>();
            For<IDomainRuleManager<IDisabilityClaimApproveModel>>().Use<DomainRuleManager<IDisabilityClaimApproveModel>>();
            For<IDomainRuleManager<IDisabilityClaimRuleModel>>().Use<DomainRuleManager<IDisabilityClaimRuleModel>>();
            For<IDomainRuleManager<IDisabilityClaimLifeAccountModel>>().Use<DomainRuleManager<IDisabilityClaimLifeAccountModel>>();
        }
    }
}