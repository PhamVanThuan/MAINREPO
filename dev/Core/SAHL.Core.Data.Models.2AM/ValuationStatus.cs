using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ValuationStatusDataModel :  IDataModel
    {
        public ValuationStatusDataModel(int valuationStatusKey, string description)
        {
            this.ValuationStatusKey = valuationStatusKey;
            this.Description = description;
		
        }		

        public int ValuationStatusKey { get; set; }

        public string Description { get; set; }
    }
}