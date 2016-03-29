using System;

namespace Automation.DataAccess.DataHelper
{
    public partial class _2AMDataHelper
    {
        /// <summary>
        /// Update the control numeric in the dbo.control table in 2am by control description
        /// </summary>
        /// <param name="controldescription"></param>
        /// <param name="controlNumeric"></param>
        /// <param name="controlText"></param>
        public void SetControlNumericByControlDescription(string controldescription, int controlNumeric)
        {
            string updateQuery
                 = String.Format(@"update dbo.control
                                  set controlNumeric={0}
                                  where controldescription='{1}'", controlNumeric, controldescription);
            SQLStatement sqlStatement = new SQLStatement() { StatementString = updateQuery };
            dataContext.ExecuteNonSQLQuery(sqlStatement);
        }

        /// <summary>
        /// Get a row from the dbo.control table given a controldescription or controlnumber
        /// </summary>
        /// <param name="controlDescription"></param>
        /// <param name="controlNumber"></param>
        /// <returns></returns>
        public QueryResultsRow GetControlValue(string controlDescription = "", int controlNumber = 0)
        {
            string query = null;
            if (!String.IsNullOrEmpty(controlDescription))
                query = String.Format(@"select * from dbo.control
                                         where controlDescription='{0}'", controlDescription);
            if (controlNumber != 0)
                query = String.Format(@"select * from dbo.control
                                        where controlNumber={0}", controlNumber);
            SQLStatement statement = new SQLStatement() { StatementString = query };
            QueryResults results = dataContext.ExecuteSQLQuery(statement);
            return results.Rows(0);
        }
    }
}