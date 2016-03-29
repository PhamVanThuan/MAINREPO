using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class StageDefinitionGroupDataModel :  IDataModel
    {
        public StageDefinitionGroupDataModel(int stageDefinitionGroupKey, string description, int genericKeyTypeKey, int generalStatusKey, int? parentKey)
        {
            this.StageDefinitionGroupKey = stageDefinitionGroupKey;
            this.Description = description;
            this.GenericKeyTypeKey = genericKeyTypeKey;
            this.GeneralStatusKey = generalStatusKey;
            this.ParentKey = parentKey;
		
        }		

        public int StageDefinitionGroupKey { get; set; }

        public string Description { get; set; }

        public int GenericKeyTypeKey { get; set; }

        public int GeneralStatusKey { get; set; }

        public int? ParentKey { get; set; }
    }
}