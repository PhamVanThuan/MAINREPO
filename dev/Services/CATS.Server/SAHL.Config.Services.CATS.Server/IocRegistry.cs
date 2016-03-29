using SAHL.Core.Rules;
using SAHL.Services.CATS.ConfigExtension;
using SAHL.Services.Interfaces.CATS.Models;
using StructureMap.Configuration.DSL;
using System.Collections.Specialized;
using System.Configuration;

namespace SAHL.Config.Services.CATS.Server
{
    public class IocRegistry : Registry
    {
        public IocRegistry()
        {
            For<IDomainRuleManager<PaymentBatch>>().Use<DomainRuleManager<PaymentBatch>>();
            For<IDomainRuleManager<CatsPaymentBatchRuleModel>>().Use<DomainRuleManager<CatsPaymentBatchRuleModel>>();

            For<ICatsAppConfigSettings>().Use<CatsAppConfigSettings>()
               .Ctor<NameValueCollection>("nameValueCollection")
               .Is(ConfigurationManager.AppSettings);

            For<System.IO.Abstractions.IFileSystem>().Use<System.IO.Abstractions.FileSystem>();
        }
    }
}
