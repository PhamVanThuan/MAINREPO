using SAHL.Tools.DomainServiceDocumenter.Lib.Models;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Tools.DomainServiceDocumenter.Lib.TypeScanners
{
    public class RuleTypeScanner : ITypeScanner
    {
        private List<RuleModel> rules;
        private List<ModelModel> models;

        public RuleTypeScanner(List<RuleModel> rules, List<ModelModel> models)
        {
            this.rules = rules;
            this.models = models;
        }

        public bool ProcessTypeDefinition(Mono.Cecil.TypeDefinition typeToProcess)
        {
            if (typeToProcess.IsInterface == false && typeToProcess.HasInterfaces && typeToProcess.Interfaces.Any(x => x.Name.StartsWith("IDomainRule")))
            {
                RuleModel rule = new RuleModel();
                rule.Name = typeToProcess.Name;
                rule.FullType = typeToProcess.FullName;

                var domainRuleInterface = typeToProcess.Interfaces.Where(x => x.Name.StartsWith("IDomainRule`1")).SingleOrDefault() as Mono.Cecil.GenericInstanceType;
                var p = domainRuleInterface.GenericArguments[0];

                // find the appropriate model
                rule.Model = p.Name;

                this.rules.Add(rule);
                return true;
            }

            return false;
        }
    }
}