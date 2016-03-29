using SAHL.Core.Data.Models.Capitec;
using System;

namespace SAHL.Services.Capitec.Managers.DecisionTreeResult
{
    public interface IDecisionTreeResultDataManager
    {
        void SaveCreditPricingTreeResult(Guid decisionTreeResultID, string decisionTreeQuery, string decisionTreeMessages, Guid applicationID, DateTime queryDate);

        void SaveCreditAssessmentTreeResult(Guid decisionTreeResultID, string decisionTreeQuery, string decisionTreeMessages, Guid applicantID, DateTime queryDate);

        CreditAssessmentTreeResultDataModel GetCreditBureauAssessmentTreeResultForApplicant(Guid applicantID);

        CreditPricingTreeResultDataModel GetCreditPricingResultForApplication(Guid applicationID);
    }
}