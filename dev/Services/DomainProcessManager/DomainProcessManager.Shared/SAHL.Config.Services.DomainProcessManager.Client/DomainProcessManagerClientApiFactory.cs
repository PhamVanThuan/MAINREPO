using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using SAHL.Services.Interfaces.DomainProcessManager;

namespace SAHL.Config.Services.DomainProcessManager.Client
{
    public class DomainProcessManagerClientApiFactory : IDomainProcessManagerClientApiFactory
    {
        private IDomainProcessManagerClient domainProcessManagerClient;

        public DomainProcessManagerClientApiFactory(IDomainProcessManagerClient domainProcessManagerClient)
        {
            this.domainProcessManagerClient = domainProcessManagerClient;
        }

        public DomainProcessManagerClientApi Create()
        {
            return new DomainProcessManagerClientApi(domainProcessManagerClient);
        }
    }
}