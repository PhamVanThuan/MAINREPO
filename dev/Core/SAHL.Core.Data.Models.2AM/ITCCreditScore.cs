using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ITCCreditScoreDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public ITCCreditScoreDataModel(int? iTCKey, int? scoreCardKey, int riskMatrixRevisionKey, double? empiricaScore, double? sBCScore, int? riskMatrixCellKey, int creditScoreDecisionKey, int generalStatusKey, DateTime scoreDate, string aDUserName, int legalEntityKey)
        {
            this.ITCKey = iTCKey;
            this.ScoreCardKey = scoreCardKey;
            this.RiskMatrixRevisionKey = riskMatrixRevisionKey;
            this.EmpiricaScore = empiricaScore;
            this.SBCScore = sBCScore;
            this.RiskMatrixCellKey = riskMatrixCellKey;
            this.CreditScoreDecisionKey = creditScoreDecisionKey;
            this.GeneralStatusKey = generalStatusKey;
            this.ScoreDate = scoreDate;
            this.ADUserName = aDUserName;
            this.LegalEntityKey = legalEntityKey;
		
        }
		[JsonConstructor]
        public ITCCreditScoreDataModel(int iTCCreditScoreKey, int? iTCKey, int? scoreCardKey, int riskMatrixRevisionKey, double? empiricaScore, double? sBCScore, int? riskMatrixCellKey, int creditScoreDecisionKey, int generalStatusKey, DateTime scoreDate, string aDUserName, int legalEntityKey)
        {
            this.ITCCreditScoreKey = iTCCreditScoreKey;
            this.ITCKey = iTCKey;
            this.ScoreCardKey = scoreCardKey;
            this.RiskMatrixRevisionKey = riskMatrixRevisionKey;
            this.EmpiricaScore = empiricaScore;
            this.SBCScore = sBCScore;
            this.RiskMatrixCellKey = riskMatrixCellKey;
            this.CreditScoreDecisionKey = creditScoreDecisionKey;
            this.GeneralStatusKey = generalStatusKey;
            this.ScoreDate = scoreDate;
            this.ADUserName = aDUserName;
            this.LegalEntityKey = legalEntityKey;
		
        }		

        public int ITCCreditScoreKey { get; set; }

        public int? ITCKey { get; set; }

        public int? ScoreCardKey { get; set; }

        public int RiskMatrixRevisionKey { get; set; }

        public double? EmpiricaScore { get; set; }

        public double? SBCScore { get; set; }

        public int? RiskMatrixCellKey { get; set; }

        public int CreditScoreDecisionKey { get; set; }

        public int GeneralStatusKey { get; set; }

        public DateTime ScoreDate { get; set; }

        public string ADUserName { get; set; }

        public int LegalEntityKey { get; set; }

        public void SetKey(int key)
        {
            this.ITCCreditScoreKey =  key;
        }
    }
}