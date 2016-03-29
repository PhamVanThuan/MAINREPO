using SAHL.Common.Security;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.ApplicationDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.V3.Framework.DomainServices
{
    public abstract class DomainServiceClientBase<C>
    {
        private C client;

        public DomainServiceClientBase(C client)
        {
            this.client = client;
        }

        private IServiceRequestMetadata CreateMetaData()
        {
            var principal = SAHLPrincipal.GetCurrent();
            var metadata = new ServiceRequestMetadata();
            metadata.Add(ServiceRequestMetadata.HEADER_USERNAME, principal.Identity.Name);
            return metadata;
        }

        protected ISystemMessageCollection PerformQuery<T>(T query) where T : IServiceQuery
        {
            dynamic client = (dynamic)this.client;
            return client.PerformQuery(query);
        }

        protected ISystemMessageCollection PerformCommand<T>(T command) where T : IServiceCommand
        {
            dynamic client = (dynamic)this.client;

            return client.PerformCommand(command, CreateMetaData());
        }
    }
}
