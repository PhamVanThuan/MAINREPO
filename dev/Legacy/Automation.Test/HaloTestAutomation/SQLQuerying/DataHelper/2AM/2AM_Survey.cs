namespace Automation.DataAccess.DataHelper
{
    public partial class _2AMDataHelper
    {
        /// <summary>
        /// Get Client Questionnaire GUID by generickey
        /// </summary>
        /// <param name="legalEntityKey"></param>
        /// <param name="bankAccountKey"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public string GetClientQuestionnaireGUID(int generickey)
        {
            string query =
                string.Format(@"select top 01 clientquestionnaire.guid from survey.clientquestionnaire
                                where clientquestionnaire.generickey={0}", generickey);
            SQLStatement statement = new SQLStatement { StatementString = query };
            QueryResults results = dataContext.ExecuteSQLScalar(statement);
            return results.SQLScalarValue;
        }

        /// <summary>
        /// Get all the client answers for the questionnaire provided the GUID.
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public QueryResults GetClientSurveyAnswersByGUID(string guid)
        {
            string query =
              string.Format(@"select * from survey.clientanswer
		                        inner join  survey.clientquestionnaire
			                        on clientanswer.clientquestionnairekey = clientquestionnaire.clientquestionnairekey
	                          where clientquestionnaire.guid='{0}'", guid);
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Get the a single clientquestionnair record from the clientquestionnaire table, provided the GUID
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public QueryResults GetClientQuestionnaireByGUID(string guid)
        {
            string query =
                string.Format(@"select top 01 * from survey.clientquestionnaire
                                where clientquestionnaire.guid='{0}'", guid);
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }
    }
}