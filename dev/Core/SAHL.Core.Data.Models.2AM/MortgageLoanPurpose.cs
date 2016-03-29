using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class MortgageLoanPurposeDataModel :  IDataModel
    {
        public MortgageLoanPurposeDataModel(int mortgageLoanPurposeKey, string description, int mortgageLoanPurposeGroupKey)
        {
            this.MortgageLoanPurposeKey = mortgageLoanPurposeKey;
            this.Description = description;
            this.MortgageLoanPurposeGroupKey = mortgageLoanPurposeGroupKey;
		
        }		

        public int MortgageLoanPurposeKey { get; set; }

        public string Description { get; set; }

        public int MortgageLoanPurposeGroupKey { get; set; }
    }
}