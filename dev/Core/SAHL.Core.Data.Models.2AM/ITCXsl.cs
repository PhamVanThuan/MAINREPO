using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ITCXslDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public ITCXslDataModel(DateTime effectiveDate, string styleSheet)
        {
            this.EffectiveDate = effectiveDate;
            this.StyleSheet = styleSheet;
		
        }
		[JsonConstructor]
        public ITCXslDataModel(int iTCXslKey, DateTime effectiveDate, string styleSheet)
        {
            this.ITCXslKey = iTCXslKey;
            this.EffectiveDate = effectiveDate;
            this.StyleSheet = styleSheet;
		
        }		

        public int ITCXslKey { get; set; }

        public DateTime EffectiveDate { get; set; }

        public string StyleSheet { get; set; }

        public void SetKey(int key)
        {
            this.ITCXslKey =  key;
        }
    }
}