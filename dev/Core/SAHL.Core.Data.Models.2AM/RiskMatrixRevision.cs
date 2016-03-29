using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class RiskMatrixRevisionDataModel :  IDataModel
    {
        public RiskMatrixRevisionDataModel(int riskMatrixRevisionKey, int riskMatrixKey, string description, DateTime revisionDate)
        {
            this.RiskMatrixRevisionKey = riskMatrixRevisionKey;
            this.RiskMatrixKey = riskMatrixKey;
            this.Description = description;
            this.RevisionDate = revisionDate;
		
        }		

        public int RiskMatrixRevisionKey { get; set; }

        public int RiskMatrixKey { get; set; }

        public string Description { get; set; }

        public DateTime RevisionDate { get; set; }
    }
}