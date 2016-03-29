using SAHL.Tools.DomainServiceDocumenter.Lib.Models;
using System.Collections.Generic;

namespace SAHL.Tools.DomainServiceDocumenter.Lib.Templates
{
    public partial class DomainChecks
    {
        public DomainChecks(List<DomainCheckModel> domainCheckModels, ServiceModel service)
        {
            this.DomainCheckModels = domainCheckModels;
            this.Service = service;
        }

        public ServiceModel Service { get; protected set; }
        public List<DomainCheckModel> DomainCheckModels { get; protected set; }
    }
}