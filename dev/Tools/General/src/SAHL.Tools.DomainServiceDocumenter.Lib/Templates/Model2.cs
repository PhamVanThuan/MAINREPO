using SAHL.Tools.DomainServiceDocumenter.Lib.Models;
using System.Collections.Generic;

namespace SAHL.Tools.DomainServiceDocumenter.Lib.Templates
{
    public partial class Model
    {
        public Model(List<ModelModel> models, ModelModel model, ServiceModel service)
        {
            this.ModelModels = models;

            this.ModelModel = model;

            this.Service = service;
        }

        public ServiceModel Service { get; protected set; }

        public ModelModel ModelModel { get; protected set; }

        public List<ModelModel> ModelModels { get; protected set; }
    }
}