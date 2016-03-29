using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class DisbursementTypeDataModel :  IDataModel
    {
        public DisbursementTypeDataModel(int disbursementTypeKey, string description)
        {
            this.DisbursementTypeKey = disbursementTypeKey;
            this.Description = description;
		
        }		

        public int DisbursementTypeKey { get; set; }

        public string Description { get; set; }
    }
}