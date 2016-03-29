using Automation.DataAccess;
using Automation.DataModels;
using Common.Constants;
using System.Collections.Generic;
namespace BuildingBlocks.Services.Contracts
{
    public interface  IAffordabilityAssessmentService
    {
        IEnumerable<AffordabilityAssessment> GetffordabilityAssessmentByWorkflowState(string workflowState);
        IEnumerable<AffordabilityAssessment> GetAffordabilityAssessmentByGenericKeyAndAssessmentStatus(int genericKey, int affordabilityAssessmentStatusKey);
        IEnumerable<AffordabilityAssessmentItem> GetAffordabilityAssessmentItemsByAffordabilityAssessmentKey(int affordabilityAssessmentKey);
        QueryResults GetAffordabilityAssessmentContributorsByAffordabilityAssessmentStatus(int genericKey, int affordabilityAssessmentStatusKey);
        void DeleteLegalEntityAffordabilityAssessment(int genericKey);
    }
}