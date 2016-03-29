using SAHL.Config.Process;
using SAHL.Config.Services;
using SAHL.Config.Services.Core;
using SAHL.Core;
using SAHL.Core.Data;
using SAHL.Core.Data.Context.Configuration;
using SAHL.Core.X2.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Workflow.Maps.Config
{
    public class WorkflowMapStartable : IDomainServiceStartable
    {
        private static IServiceBootstrapper _serviceBootstrapper;
        private static IIocContainer _container;

        public WorkflowMapStartable()
        {
            _serviceBootstrapper = new ProcessBootstrapper();
        }

        public void Start(string processName)
        {
            _container = _serviceBootstrapper.Initialise();
            if (_container == null) { throw new ArgumentNullException("IocContainer"); }
            DbContextConfiguration.Instance.RepositoryFactory = _container.GetInstance<ISqlRepositoryFactory>();
        }
    }
}
