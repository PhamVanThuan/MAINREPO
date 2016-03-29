using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class BatchTypeDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public BatchTypeDataModel(string calculationFormula, string description, DateTime? changeDate, string changeUser)
        {
            this.CalculationFormula = calculationFormula;
            this.Description = description;
            this.ChangeDate = changeDate;
            this.ChangeUser = changeUser;
		
        }
		[JsonConstructor]
        public BatchTypeDataModel(int batchTypeKey, string calculationFormula, string description, DateTime? changeDate, string changeUser)
        {
            this.BatchTypeKey = batchTypeKey;
            this.CalculationFormula = calculationFormula;
            this.Description = description;
            this.ChangeDate = changeDate;
            this.ChangeUser = changeUser;
		
        }		

        public int BatchTypeKey { get; set; }

        public string CalculationFormula { get; set; }

        public string Description { get; set; }

        public DateTime? ChangeDate { get; set; }

        public string ChangeUser { get; set; }

        public void SetKey(int key)
        {
            this.BatchTypeKey =  key;
        }
    }
}