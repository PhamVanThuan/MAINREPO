using SAHL.Tools.DomainServiceDocumenter.Lib.Models;
using System.Collections.Generic;

namespace SAHL.Tools.DomainServiceDocumenter.Lib.Templates
{
    public partial class Enumerations
    {
        public Enumerations(List<EnumerationModel> enumerations, ServiceModel service)
        {
            this.EnumerationModels = enumerations;
            this.Service = service;
        }
        public ServiceModel Service { get; protected set; }
        public List<EnumerationModel> EnumerationModels { get; protected set; }
    }
}