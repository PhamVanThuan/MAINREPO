using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ValuationRoofTypeDataModel :  IDataModel
    {
        public ValuationRoofTypeDataModel(int valuationRoofTypeKey, string description)
        {
            this.ValuationRoofTypeKey = valuationRoofTypeKey;
            this.Description = description;
		
        }		

        public int ValuationRoofTypeKey { get; set; }

        public string Description { get; set; }
    }
}