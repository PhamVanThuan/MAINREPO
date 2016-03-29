using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ReasonTypeDataModel :  IDataModel
    {
        public ReasonTypeDataModel(int reasonTypeKey, string description, int genericKeyTypeKey, int reasonTypeGroupKey)
        {
            this.ReasonTypeKey = reasonTypeKey;
            this.Description = description;
            this.GenericKeyTypeKey = genericKeyTypeKey;
            this.ReasonTypeGroupKey = reasonTypeGroupKey;
		
        }		

        public int ReasonTypeKey { get; set; }

        public string Description { get; set; }

        public int GenericKeyTypeKey { get; set; }

        public int ReasonTypeGroupKey { get; set; }
    }
}