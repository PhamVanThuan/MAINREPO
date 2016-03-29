using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class DebtCounsellingStatusDataModel :  IDataModel
    {
        public DebtCounsellingStatusDataModel(int debtCounsellingStatusKey, string description)
        {
            this.DebtCounsellingStatusKey = debtCounsellingStatusKey;
            this.Description = description;
		
        }		

        public int DebtCounsellingStatusKey { get; set; }

        public string Description { get; set; }
    }
}