using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class DebitOrderDayDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public DebitOrderDayDataModel(int debitOrderDay)
        {
            this.DebitOrderDay = debitOrderDay;
		
        }
		[JsonConstructor]
        public DebitOrderDayDataModel(int debitOrderDayKey, int debitOrderDay)
        {
            this.DebitOrderDayKey = debitOrderDayKey;
            this.DebitOrderDay = debitOrderDay;
		
        }		

        public int DebitOrderDayKey { get; set; }

        public int DebitOrderDay { get; set; }

        public void SetKey(int key)
        {
            this.DebitOrderDayKey =  key;
        }
    }
}