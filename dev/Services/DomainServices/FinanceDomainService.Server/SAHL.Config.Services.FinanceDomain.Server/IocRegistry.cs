using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using SAHL.Shared.BusinessModel.Models;
using StructureMap.Configuration.DSL;

namespace SAHL.Config.Services.FinanceDomain.Server
{
    public class IocRegistry : Registry
    {
        public IocRegistry()
        {
            For<IDomainRuleManager<IAccountRuleModel>>().Use<DomainRuleManager<IAccountRuleModel>>();
            For<IDomainRuleManager<IThirdPartyInvoiceRuleModel>>().Use<DomainRuleManager<IThirdPartyInvoiceRuleModel>>();
            For<IDomainRuleManager<ThirdPartyInvoiceModel>>().Use<DomainRuleManager<ThirdPartyInvoiceModel>>();
            For<IDomainRuleManager<TransactionRuleModel>>().Use<DomainRuleManager<TransactionRuleModel>>();
            For<IDomainRuleManager<GetThirdPartyInvoiceQueryResult>>().Use<DomainRuleManager<GetThirdPartyInvoiceQueryResult>>();
            For<IDomainRuleManager<ServiceRequestMetadata>>().Use<DomainRuleManager<ServiceRequestMetadata>>();
            For<IDomainRuleManager<PostTransactionModel>>().Use<DomainRuleManager<PostTransactionModel>>();
        }
    }
}