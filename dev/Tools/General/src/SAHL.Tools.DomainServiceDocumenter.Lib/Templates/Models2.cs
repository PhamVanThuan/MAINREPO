using SAHL.Tools.DomainServiceDocumenter.Lib.Models;
using System.Collections.Generic;

namespace SAHL.Tools.DomainServiceDocumenter.Lib.Templates
{
    public partial class Models
    {
        public Models(List<ModelModel> models, ServiceModel service)
        {
            this.ModelModels = models;
            this.Service = service;
        }

        public ServiceModel Service { get; protected set; }

        public List<ModelModel> ModelModels { get; protected set; }
    }
}