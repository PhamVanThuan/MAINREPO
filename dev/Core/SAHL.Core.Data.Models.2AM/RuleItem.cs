using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class RuleItemDataModel :  IDataModel
    {
        public RuleItemDataModel(int ruleItemKey, string name, string description, string assemblyName, string typeName, bool enforceRule, int generalStatusKey, string generalStatusReasonDescription)
        {
            this.RuleItemKey = ruleItemKey;
            this.Name = name;
            this.Description = description;
            this.AssemblyName = assemblyName;
            this.TypeName = typeName;
            this.EnforceRule = enforceRule;
            this.GeneralStatusKey = generalStatusKey;
            this.GeneralStatusReasonDescription = generalStatusReasonDescription;
		
        }		

        public int RuleItemKey { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string AssemblyName { get; set; }

        public string TypeName { get; set; }

        public bool EnforceRule { get; set; }

        public int GeneralStatusKey { get; set; }

        public string GeneralStatusReasonDescription { get; set; }
    }
}