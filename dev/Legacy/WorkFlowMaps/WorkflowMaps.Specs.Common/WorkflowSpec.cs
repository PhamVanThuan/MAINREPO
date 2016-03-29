using DomainService2;
using DomainService2.IOC;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Core.Data;
using SAHL.Core.Data.Context.Configuration;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.SystemMessages;
using SAHL.Core.X2;
using SAHL.Core.X2.Providers;
using SAHL.X2.Framework.Common;
using SAHL.X2Engine2.Factories;

namespace WorkflowMaps.Specs.Common
{
    public class WorkflowSpec<T, V> : WithFakes
        where T : IX2Map
        where V : IX2ContextualDataProvider
    {
        protected static T workflow;
        protected static ISystemMessageCollection messages; // ISystemMessageCollection
        protected static InstanceDataModel instanceData; // InstanceDataModel
        protected static SAHL.Core.X2.IX2Params paramsData; //SAHL.Core.X2.IX2Params
        protected static V workflowData;
        protected static TestDomainServiceLoader domainServiceLoader;
        protected static IocRegistry ioc;

        public WorkflowSpec()
        {
            DbContextConfiguration.Instance.RepositoryFactory = An<ISqlRepositoryFactory>();
            DbContextConfiguration.Instance.RepositoryFactory.WhenToldTo(x => x.UIStatementProvider).Return(An<IUIStatementProvider>());
            messages = new MessageCollection();
            instanceData = new InstanceDataStub();
            paramsData = new ParamsDataStub();
            domainServiceLoader = new TestDomainServiceLoader();
            DomainServiceLoader.Instance = domainServiceLoader;
            ioc = new IocRegistry();
        }
    }

}
