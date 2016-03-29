using SAHL.Core.Rules;
using SAHL.Services.ClientDomain.Rules.Models;
using SAHL.Services.Interfaces.ClientDomain.Models;
using StructureMap.Configuration.DSL;

namespace SAHL.Config.Services.ClientDomain.Server
{
    public class IocRegistry : Registry
    {
        public IocRegistry()
        {
            For<IDomainRuleManager<EmployerModel>>().Use<DomainRuleManager<EmployerModel>>();
            For<IDomainRuleManager<SalariedEmploymentModel>>().Use<DomainRuleManager<SalariedEmploymentModel>>();
            For<IDomainRuleManager<SalaryDeductionEmploymentModel>>().Use<DomainRuleManager<SalaryDeductionEmploymentModel>>();
            For<IDomainRuleManager<UnemployedEmploymentModel>>().Use<DomainRuleManager<UnemployedEmploymentModel>>();
            For<IDomainRuleManager<ClientAddressAsPendingDomiciliumModel>>().Use<DomainRuleManager<ClientAddressAsPendingDomiciliumModel>>();
            For<IDomainRuleManager<FixedPropertyAssetModel>>().Use<DomainRuleManager<FixedPropertyAssetModel>>();
            For<IDomainRuleManager<NaturalPersonClientModel>>().Use<DomainRuleManager<NaturalPersonClientModel>>();
            For<IDomainRuleManager<INaturalPersonClientModel>>().Use<DomainRuleManager<INaturalPersonClientModel>>();
            For<IDomainRuleManager<NaturalPersonClientRuleModel>>().Use<DomainRuleManager<NaturalPersonClientRuleModel>>();
            For<IDomainRuleManager<ActiveNaturalPersonClientModel>>().Use<DomainRuleManager<ActiveNaturalPersonClientModel>>();
        }
    }
}