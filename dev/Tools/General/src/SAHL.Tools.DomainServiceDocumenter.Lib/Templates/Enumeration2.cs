using SAHL.Tools.DomainServiceDocumenter.Lib.Models;
using System.Collections.Generic;

namespace SAHL.Tools.DomainServiceDocumenter.Lib.Templates
{
    public partial class Enumeration
    {
        public Enumeration(List<EnumerationModel> enumerations, EnumerationModel enumeration, ServiceModel service)
        {
            this.EnumerationModels = enumerations;
            this.EnumerationModel = enumeration;
            this.Service = service;
        }

        public ServiceModel Service { get; protected set; }
        public List<EnumerationModel> EnumerationModels { get; protected set; }
        public EnumerationModel EnumerationModel { get; protected set; }

    }
}