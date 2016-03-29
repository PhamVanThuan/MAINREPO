using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class DisbursementStatusDataModel :  IDataModel
    {
        public DisbursementStatusDataModel(int disbursementStatusKey, string description)
        {
            this.DisbursementStatusKey = disbursementStatusKey;
            this.Description = description;
		
        }		

        public int DisbursementStatusKey { get; set; }

        public string Description { get; set; }
    }
}