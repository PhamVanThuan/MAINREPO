using SAHL.Core.Rules;
using SAHL.Services.Interfaces.BankAccountDomain.Models;
using StructureMap.Configuration.DSL;

namespace SAHL.Config.Services.BankAccountDomain.Server
{
    public class IocRegistry : Registry
    {
        public IocRegistry()
        {
            For<IDomainRuleManager<BankAccountModel>>().Use<DomainRuleManager<BankAccountModel>>();
        }
    }
}