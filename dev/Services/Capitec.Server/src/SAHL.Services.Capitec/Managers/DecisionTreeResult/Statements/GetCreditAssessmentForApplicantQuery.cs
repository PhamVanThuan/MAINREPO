using SAHL.Core.Attributes;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.Capitec;
using System;

namespace SAHL.Services.Capitec.Managers.DecisionTreeResult.Statements
{
    [NolockConventionExclude]
    public class GetCreditAssessmentForApplicantQuery : ISqlStatement<CreditAssessmentTreeResultDataModel>
    {
        public Guid ApplicantID { get; protected set; }

        public GetCreditAssessmentForApplicantQuery(Guid applicantID)
        {
            this.ApplicantID = applicantID;
        }

        public string GetStatement()
        {
            return String.Format("SELECT * FROM [Capitec].dbo.CreditAssessmentTreeResult (NOLOCK) WHERE ApplicantID = @ApplicantID");
        }
    }
}