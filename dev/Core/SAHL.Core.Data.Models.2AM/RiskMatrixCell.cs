using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class RiskMatrixCellDataModel :  IDataModel
    {
        public RiskMatrixCellDataModel(int riskMatrixCellKey, int riskMatrixRevisionKey, int creditScoreDecisionKey, int? ruleItemKey, int generalStatusKey, string designation)
        {
            this.RiskMatrixCellKey = riskMatrixCellKey;
            this.RiskMatrixRevisionKey = riskMatrixRevisionKey;
            this.CreditScoreDecisionKey = creditScoreDecisionKey;
            this.RuleItemKey = ruleItemKey;
            this.GeneralStatusKey = generalStatusKey;
            this.Designation = designation;
		
        }		

        public int RiskMatrixCellKey { get; set; }

        public int RiskMatrixRevisionKey { get; set; }

        public int CreditScoreDecisionKey { get; set; }

        public int? RuleItemKey { get; set; }

        public int GeneralStatusKey { get; set; }

        public string Designation { get; set; }
    }
}