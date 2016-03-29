using Automation.DataModels;
using System.Collections.Generic;
using Dapper;
using Common.Constants;

namespace Automation.DataAccess.DataHelper
{
    public partial class _2AMDataHelper
    {

        /// <summary>
        /// Gets an affordability assessment by workflow state
        /// </summary>
        /// <param name="genericKey"></param>
        /// <returns></returns>
        public IEnumerable<AffordabilityAssessment> GetffordabilityAssessmentByWorkflowState(string workflowState)
        {
            var query = string.Format(@"SELECT
	                                        aa.GenericKey,
	                                        x2s.Name
                                        FROM [2AM].[dbo].AffordabilityAssessment aa
	                                        INNER JOIN [X2].[X2DATA].Application_Capture x2ac ON x2ac.ApplicationKey = aa.GenericKey
	                                        INNER JOIN [X2].[X2].Instance x2i ON x2i.ID = x2ac.InstanceID AND x2i.ParentInstanceID IS NULL
	                                        INNER JOIN [X2].[X2].State x2s ON x2s.ID = x2i.StateID AND x2s.Name = '{0}'
                                        WHERE aa.AffordabilityAssessmentStatusKey = 1
                                        AND aa.GeneralStatusKey = 1", workflowState);
            return dataContext.Query<AffordabilityAssessment>(query);
        }

        /// <summary>
        /// Gets an affordability assessment by generickey
        /// </summary>
        /// <param name="genericKey"></param>
        /// <returns></returns>
        public IEnumerable<AffordabilityAssessment> GetAffordabilityAssessmentByGenericKeyAndAssessmentStatus(int genericKey, int affordabilityAssessmentStatusKey)
        {
            var query = string.Format(@"SELECT *
                                        FROM [2AM].[dbo].[AffordabilityAssessment]
                                        WHERE [GenericKey] = {0}
                                        AND [AffordabilityAssessmentStatusKey] = {1}", genericKey, affordabilityAssessmentStatusKey);
            return dataContext.Query<AffordabilityAssessment>(query);
        }

        /// <summary>
        /// Gets affordability assessment items by key
        /// </summary>
        /// <param name="affordabilityAssessmentKey"></param>
        /// <returns></returns>
        public IEnumerable<AffordabilityAssessmentItem> GetAffordabilityAssessmentItemsByAffordabilityAssessmentKey(int affordabilityAssessmentKey)
        {
            var query = string.Format(@"SELECT 
	                                        aai.*,
	                                        aait.AffordabilityAssessmentItemCategoryKey
                                        FROM [2AM].[dbo].[AffordabilityAssessmentItem] aai
	                                        INNER JOIN [2AM].[dbo].[AffordabilityAssessmentItemType] aait ON aait.AffordabilityAssessmentItemTypeKey = aai.AffordabilityAssessmentItemTypeKey
                                        WHERE [AffordabilityAssessmentKey] = {0}", affordabilityAssessmentKey);
            return dataContext.Query<AffordabilityAssessmentItem>(query);
        }

        /// <summary>
        /// Gets affordability assessment legal entities by generickey
        /// </summary>
        /// <param name="genericKey"></param>
        /// <returns></returns>
        public QueryResults GetAffordabilityAssessmentContributorsByAffordabilityAssessmentStatus(int genericKey, int affordabilityAssessmentStatusKey)
        {
            string query = string.Empty;
            SQLStatement statement = new SQLStatement();

            query = string.Format(@"SELECT 
	                                    [2AM].[dbo].fGetAffordabilityAssessmentContributors(aa.AffordabilityAssessmentKey, 0, 1) AS 'AffordabilityAssessmentContributors'
                                    FROM [2AM].[dbo].AffordabilityAssessment aa 
                                    WHERE aa.GenericKey = {0}
                                    AND aa.AffordabilityAssessmentStatusKey = {1}", genericKey, affordabilityAssessmentStatusKey);

            statement.StatementString = query;

            return dataContext.ExecuteSQLQuery(statement);
        }   
     
        public void DeleteLegalEntityAffordabilityAssessment(int genericKey)
        {
            string sql = string.Empty;
            var statement = new SQLStatement();

            sql = string.Format(@"  DELETE FROM [2AM].[dbo].AffordabilityAssessmentItem WHERE AffordabilityAssessmentKey IN (SELECT AffordabilityAssessmentKey FROM [2AM].[dbo].AffordabilityAssessment where GenericKey = {0})
                                    DELETE FROM [2AM].[dbo].AffordabilityAssessmentLegalEntity WHERE AffordabilityAssessmentKey IN (SELECT AffordabilityAssessmentKey FROM [2AM].[dbo].AffordabilityAssessment where GenericKey  = {1})
                                    DELETE FROM [2AM].[dbo].AffordabilityAssessment WHERE AffordabilityAssessmentKey IN (SELECT AffordabilityAssessmentKey FROM [2AM].[dbo].AffordabilityAssessment where GenericKey = {2})", genericKey, genericKey, genericKey);
            statement = new SQLStatement { StatementString = sql };
            dataContext.ExecuteNonSQLQuery(statement);
        }
    }
}