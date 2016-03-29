using SAHL.Tools.DomainServiceDocumenter.Lib.Models;
using System.Collections.Generic;

namespace SAHL.Tools.DomainServiceDocumenter.Lib.Templates
{
    public partial class DomainCheck
    {
        public DomainCheck(List<DomainCheckModel> domainCheckModels, DomainCheckModel domainCheckModel, ServiceModel service)
        {
            this.DomainCheckModels = domainCheckModels;
            this.DomainCheckModel = domainCheckModel;
            this.Service = service;
        }

        public ServiceModel Service { get; protected set; }
        public List<DomainCheckModel> DomainCheckModels { get; protected set; }
        public DomainCheckModel DomainCheckModel { get; protected set; }
    }
}