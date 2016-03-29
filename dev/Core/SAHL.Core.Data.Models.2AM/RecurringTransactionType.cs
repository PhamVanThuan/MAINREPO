using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class RecurringTransactionTypeDataModel :  IDataModel
    {
        public RecurringTransactionTypeDataModel(int recurringTransactionTypeKey, string description)
        {
            this.RecurringTransactionTypeKey = recurringTransactionTypeKey;
            this.Description = description;
		
        }		

        public int RecurringTransactionTypeKey { get; set; }

        public string Description { get; set; }
    }
}