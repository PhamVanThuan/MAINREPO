using SAHL.Core;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Workflow.Maps.Config
{
    public class DomainProcess : IDomainProcess
    {
        private IIocContainer iocContainer;

        public DomainProcess(IIocContainer iocContainer)
        {
            this.iocContainer = iocContainer;
        }

        static IDomainProcess instance;
        static readonly object instanceLock = new object();

        public static IDomainProcess Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new DomainProcess(ObjectFactory.GetInstance<IIocContainer>());
                    }
                    return instance;
                }
            }
            set
            {
                lock (instanceLock)
                {
                    instance = value;
                }
            }
        }

        public T Get<T>() where T : IWorkflowService
        {
            return this.iocContainer.GetInstance<T>();
        }

    }
}
