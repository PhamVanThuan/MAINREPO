using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class OriginationSourceProductExpenseTypeDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public OriginationSourceProductExpenseTypeDataModel(int originationSourceProductKey, int expenseTypeKey)
        {
            this.OriginationSourceProductKey = originationSourceProductKey;
            this.ExpenseTypeKey = expenseTypeKey;
		
        }
		[JsonConstructor]
        public OriginationSourceProductExpenseTypeDataModel(int originationSourceProductExpenseTypeKey, int originationSourceProductKey, int expenseTypeKey)
        {
            this.OriginationSourceProductExpenseTypeKey = originationSourceProductExpenseTypeKey;
            this.OriginationSourceProductKey = originationSourceProductKey;
            this.ExpenseTypeKey = expenseTypeKey;
		
        }		

        public int OriginationSourceProductExpenseTypeKey { get; set; }

        public int OriginationSourceProductKey { get; set; }

        public int ExpenseTypeKey { get; set; }

        public void SetKey(int key)
        {
            this.OriginationSourceProductExpenseTypeKey =  key;
        }
    }
}