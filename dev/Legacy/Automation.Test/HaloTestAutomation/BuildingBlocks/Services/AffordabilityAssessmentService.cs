using Automation.DataAccess;
using Automation.DataAccess.DataHelper;
using Automation.DataModels;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using System.Collections.Generic;
namespace BuildingBlocks.Services
{
    public class AffordabilityAssessmentService : _2AMDataHelper, IAffordabilityAssessmentService
    {
        /// <summary>
        /// Gets an affordability assessment by workflow state
        /// </summary>
        /// <param name="genericKey"></param>
        /// <returns></returns>
        public IEnumerable<AffordabilityAssessment> GetffordabilityAssessmentByWorkflowState(string workflowState)
        {
            return base.GetffordabilityAssessmentByWorkflowState(workflowState);
        }

        /// <summary>
        /// Gets an affordability assessment by generickey
        /// </summary>
        /// <param name="genericKey"></param>
        /// <returns></returns>
        public IEnumerable<AffordabilityAssessment> GetAffordabilityAssessmentByGenericKeyAndAssessmentStatus(int genericKey, int affordabilityAssessmentStatusKey)
        {
            return base.GetAffordabilityAssessmentByGenericKeyAndAssessmentStatus(genericKey, affordabilityAssessmentStatusKey);
        }

        /// <summary>
        /// Gets an affordability assessment items by affordabilityAssessmentKey
        /// </summary>
        /// <param name="affordabilityAssessmentKey"></param>
        /// <returns></returns>
        public IEnumerable<AffordabilityAssessmentItem> GetAffordabilityAssessmentItemsByAffordabilityAssessmentKey(int affordabilityAssessmentKey)
        {
            return base.GetAffordabilityAssessmentItemsByAffordabilityAssessmentKey(affordabilityAssessmentKey);
        }

        /// <summary>
        /// Gets affordability assessment legal entities by generickey
        /// </summary>
        /// <param name="genericKey"></param>
        /// <returns></returns>
        public QueryResults GetAffordabilityAssessmentContributorsByAffordabilityAssessmentStatus(int genericKey, int affordabilityAssessmentStatusKey)
        {
            return base.GetAffordabilityAssessmentContributorsByAffordabilityAssessmentStatus(genericKey, affordabilityAssessmentStatusKey);
        }


        public void DeleteLegalEntityAffordabilityAssessment(int genericKey)
        {
            base.DeleteLegalEntityAffordabilityAssessment(genericKey);
        }
    }
}