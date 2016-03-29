using SAHL.Core.Rules;
using SAHL.Services.FrontEndTest.Managers.Mailbox;
using SAHL.Shared.BusinessModel.Models;
using StructureMap.Configuration.DSL;
using System.Collections.Specialized;
using System.Configuration;
using SAHL.Core.Services;

namespace SAHL.Config.Services.FrontEndTest.Server
{
    public class IocRegistry : Registry
    {
        public IocRegistry()
        {
            For<IMailboxManager>().Use<MailboxManager>()
               .Ctor<NameValueCollection>("nameValueCollection")
               .Is(ConfigurationManager.AppSettings);
            For<IDomainRuleManager<PostTransactionModel>>().Use<DomainRuleManager<PostTransactionModel>>();
            For<IDomainRuleManager<ServiceRequestMetadata>>().Use<DomainRuleManager<ServiceRequestMetadata>>();
            For<IDomainRuleManager<TransactionRuleModel>>().Use<DomainRuleManager<TransactionRuleModel>>();
        }
    }
}