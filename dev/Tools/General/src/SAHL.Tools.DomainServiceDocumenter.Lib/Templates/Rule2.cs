using SAHL.Tools.DomainServiceDocumenter.Lib.Models;
using System.Collections.Generic;

namespace SAHL.Tools.DomainServiceDocumenter.Lib.Templates
{
    public partial class Rule
    {
        public Rule(List<RuleModel> models, RuleModel model, ServiceModel service)
        {
            this.RuleModels = models;

            this.RuleModel = model;

            this.Service = service;
        }

        public ServiceModel Service { get; protected set; }

        public RuleModel RuleModel { get; protected set; }

        public List<RuleModel> RuleModels { get; protected set; }
    }
}