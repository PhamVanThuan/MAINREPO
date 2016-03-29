using SAHL.Workflow.Maps.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.WorkflowMaps.Specs.Common
{
    public class TestDomainServiceLoader : IDomainProcess
    {
        public Dictionary<Type, IWorkflowService> mocks;

        public TestDomainServiceLoader()
        {
            this.mocks = new Dictionary<Type, IWorkflowService>();
        }

        public T Get<T>() where T : IWorkflowService
        {
            return (T)mocks[typeof(T)];
        }

        public void RegisterMockForType<T>(T mock) where T : IWorkflowService
        {
            mocks.Add(typeof(T), mock);
        }
    }
}
