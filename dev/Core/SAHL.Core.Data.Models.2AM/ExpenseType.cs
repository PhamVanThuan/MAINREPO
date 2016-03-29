using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ExpenseTypeDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public ExpenseTypeDataModel(int? expenseTypeGroupKey, int? paymentTypeKey, string description)
        {
            this.ExpenseTypeGroupKey = expenseTypeGroupKey;
            this.PaymentTypeKey = paymentTypeKey;
            this.Description = description;
		
        }
		[JsonConstructor]
        public ExpenseTypeDataModel(int expenseTypeKey, int? expenseTypeGroupKey, int? paymentTypeKey, string description)
        {
            this.ExpenseTypeKey = expenseTypeKey;
            this.ExpenseTypeGroupKey = expenseTypeGroupKey;
            this.PaymentTypeKey = paymentTypeKey;
            this.Description = description;
		
        }		

        public int ExpenseTypeKey { get; set; }

        public int? ExpenseTypeGroupKey { get; set; }

        public int? PaymentTypeKey { get; set; }

        public string Description { get; set; }

        public void SetKey(int key)
        {
            this.ExpenseTypeKey =  key;
        }
    }
}