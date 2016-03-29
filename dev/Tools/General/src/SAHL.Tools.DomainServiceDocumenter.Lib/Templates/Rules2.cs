using SAHL.Tools.DomainServiceDocumenter.Lib.Models;
using System.Collections.Generic;

namespace SAHL.Tools.DomainServiceDocumenter.Lib.Templates
{
    public partial class Rules
    {
        public Rules(List<RuleModel> rules, ServiceModel service)
        {
            this.RuleModels = rules;
            this.Service = service;
        }

        public ServiceModel Service { get; protected set; }

        public List<RuleModel> RuleModels { get; protected set; }
    }
}