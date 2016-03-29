using Managers;
using SAHL.Core.Data.Models._2AM.Managers;
using SAHL.Core.DomainProcess;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.ApplicationCreationProcess;
using SAHL.DomainProcessManager.DomainProcesses.Managers.Communication;
using StructureMap.Configuration.DSL;
using System.Collections.Specialized;
using System.Configuration;
using SAHL.Core.Services.CommandPersistence;
using SAHL.Core.Services.CommandPersistence.CommandRetryPolicy;
using SAHL.DomainProcessManager.Models;
using SAHL.Core.Rules;

namespace SAHL.Config.DomainManager.DomainProcesses
{
    public class IocRegistry : Registry
    {
        public IocRegistry()
        {
            this.Scan(scanner =>
            {
                scanner.AssembliesFromApplicationBaseDirectory(assembly => assembly.FullName.StartsWith("SAHL"));

                scanner.AddAllTypesOf(typeof(IDomainProcess)).NameBy(x => x.Name);
                scanner.AddAllTypesOf(typeof(IDomainProcessEvent<>)).NameBy(x => x.Name);

                scanner.Convention<DomainProcessConvention>();
                scanner.WithDefaultConventions();
            });

            this.For<ICommunicationManagerSettings>().Use<CommunicationManagerSettings>().Ctor<NameValueCollection>("nameValueCollection").Is(ConfigurationManager.AppSettings);
            this.For<ILinkedKeyManager>().Use<LinkedKeyManager>();
            this.For<ILinkedKeyDataManager>().Use<LinkedKeyDataManager>();
            this.For<ICommandSessionFactory>().Add<CommandSessionFactory>().Ctor<ICommandRetryPolicy>().Is<CommandRetryPolicyNone>().Named("DomainProcessRetryPolicy");
            this.For<ICommandSessionFactory>().Use<CommandSessionFactory>().Named("DomainProcessRetryPolicy");           
            this.For<IDomainRuleManager<ApplicationCreationModel>>().Use<DomainRuleManager<ApplicationCreationModel>>();
            this.For<IDomainRuleManager<PayThirdPartyInvoiceProcessModel>>().Use<DomainRuleManager<PayThirdPartyInvoiceProcessModel>>();
        }
    }
}