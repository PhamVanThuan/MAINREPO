using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class StageDefinitionDataModel :  IDataModel
    {
        public StageDefinitionDataModel(int stageDefinitionKey, string description, int generalStatusKey, bool isComposite, string name, string compositeTypeName, bool? hasCompositeLogic)
        {
            this.StageDefinitionKey = stageDefinitionKey;
            this.Description = description;
            this.GeneralStatusKey = generalStatusKey;
            this.IsComposite = isComposite;
            this.Name = name;
            this.CompositeTypeName = compositeTypeName;
            this.HasCompositeLogic = hasCompositeLogic;
		
        }		

        public int StageDefinitionKey { get; set; }

        public string Description { get; set; }

        public int GeneralStatusKey { get; set; }

        public bool IsComposite { get; set; }

        public string Name { get; set; }

        public string CompositeTypeName { get; set; }

        public bool? HasCompositeLogic { get; set; }
    }
}