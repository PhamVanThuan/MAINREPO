using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ExpenseTypeGroupDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public ExpenseTypeGroupDataModel(string description, bool? fee, bool? expense)
        {
            this.Description = description;
            this.Fee = fee;
            this.Expense = expense;
		
        }
		[JsonConstructor]
        public ExpenseTypeGroupDataModel(int expenseTypeGroupKey, string description, bool? fee, bool? expense)
        {
            this.ExpenseTypeGroupKey = expenseTypeGroupKey;
            this.Description = description;
            this.Fee = fee;
            this.Expense = expense;
		
        }		

        public int ExpenseTypeGroupKey { get; set; }

        public string Description { get; set; }

        public bool? Fee { get; set; }

        public bool? Expense { get; set; }

        public void SetKey(int key)
        {
            this.ExpenseTypeGroupKey =  key;
        }
    }
}