using SAHL.Core.SystemMessages;
using SAHL.Services.Capitec.Managers.DecisionTreeResult.Models;
using System;
using System.Linq;

namespace SAHL.Services.Capitec.Managers.DecisionTreeResult
{
    public interface IDecisionTreeResultManager
    {
        Guid SaveCreditAssessmentTreeResult(object treeQuery, ISystemMessageCollection decisionTreeMessages, Guid applicationID);

        Guid SaveCreditPricingTreeResult(object treeQuery, ISystemMessageCollection decisionTreeMessages, Guid applicantID);

        CreditBureauAssessmentResult GetITCResultForApplicant(Guid applicantID);

        CreditPricingResult GetCalculationResultForApplication(Guid applicationID);
    }
}