using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class MortgageLoanPurposeGroupDataModel :  IDataModel
    {
        public MortgageLoanPurposeGroupDataModel(int mortgageLoanPurposeGroupKey, string description)
        {
            this.MortgageLoanPurposeGroupKey = mortgageLoanPurposeGroupKey;
            this.Description = description;
		
        }		

        public int MortgageLoanPurposeGroupKey { get; set; }

        public string Description { get; set; }
    }
}