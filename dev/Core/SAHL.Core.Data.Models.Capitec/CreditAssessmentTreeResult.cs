using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.Capitec
{
    [Serializable]
    public partial class CreditAssessmentTreeResultDataModel :  IDataModel
    {
        public CreditAssessmentTreeResultDataModel(string messages, string treeQuery, Guid applicantID, DateTime queryDate)
        {
            this.Messages = messages;
            this.TreeQuery = treeQuery;
            this.ApplicantID = applicantID;
            this.QueryDate = queryDate;
		
        }

        public CreditAssessmentTreeResultDataModel(Guid id, string messages, string treeQuery, Guid applicantID, DateTime queryDate)
        {
            this.Id = id;
            this.Messages = messages;
            this.TreeQuery = treeQuery;
            this.ApplicantID = applicantID;
            this.QueryDate = queryDate;
		
        }		

        public Guid Id { get; set; }

        public string Messages { get; set; }

        public string TreeQuery { get; set; }

        public Guid ApplicantID { get; set; }

        public DateTime QueryDate { get; set; }
    }
}