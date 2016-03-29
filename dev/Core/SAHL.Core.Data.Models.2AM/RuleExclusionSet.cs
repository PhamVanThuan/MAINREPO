using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class RuleExclusionSetDataModel :  IDataModel
    {
        public RuleExclusionSetDataModel(int ruleExclusionSetKey, string description, string comment)
        {
            this.RuleExclusionSetKey = ruleExclusionSetKey;
            this.Description = description;
            this.Comment = comment;
		
        }		

        public int RuleExclusionSetKey { get; set; }

        public string Description { get; set; }

        public string Comment { get; set; }
    }
}