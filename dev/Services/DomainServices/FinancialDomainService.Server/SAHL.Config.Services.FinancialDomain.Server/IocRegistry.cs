using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Rules;
using SAHL.Services.Interfaces.FinancialDomain.Models;
using StructureMap.Configuration.DSL;

namespace SAHL.Config.Services.FinancialDomain.Server
{
    public class IocRegistry : Registry
    {
        public IocRegistry()
        {
            For<IDomainRuleManager<OfferInformationDataModel>>().Use<DomainRuleManager<OfferInformationDataModel>>();
            For<IDomainRuleManager<IApplicationModel>>().Use<DomainRuleManager<IApplicationModel>>();
        }
    }
}