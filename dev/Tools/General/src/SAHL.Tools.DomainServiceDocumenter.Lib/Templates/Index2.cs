using SAHL.Tools.DomainServiceDocumenter.Lib.Models;

namespace SAHL.Tools.DomainServiceDocumenter.Lib.Templates
{
    public partial class Index
    {
        public Index(ServiceModel service)
        {
            this.Service = service;
        }

        public ServiceModel Service { get; protected set; }
    }
}