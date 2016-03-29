using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class CAP_NewInterestProgramDataModel :  IDataModel
    {
        public CAP_NewInterestProgramDataModel(int financialServiceKey)
        {
            this.FinancialServiceKey = financialServiceKey;
		
        }		

        public int FinancialServiceKey { get; set; }
    }
}