using System;

namespace Automation.DataAccess.DataHelper
{
    public partial class _2AMDataHelper
    {
        /// <summary>
        /// Gets all of the Affordability records linked to a Legal Entity and an Application
        /// </summary>
        /// <param name="legalEntityKey">LegalEntityKey</param>
        /// <param name="offerKey">OfferKey</param>
        /// <returns></returns>
        public QueryResults GetLegalEntityAffordabilityByLegalEntityKeyAndOfferKey(int legalEntityKey, string offerKey)
        {
            string query =
                String.Format(@"select * from [2AM].[dbo].[LegalEntityAffordability] (nolock)
                                where OfferKey={0} and LegalEntityKey={1}", offerKey, legalEntityKey);
            var statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Gets the income records for the Affordability and Expenses linked to a Legal Entity and an Application
        /// </summary>
        /// <param name="legalEntityKey">LegalEntityKey</param>
        /// <param name="offerKey">OfferKey</param>
        /// <returns></returns>
        public QueryResults GetLegalEntityAffordabilityIncomeByLegalEntityKeyAndOfferKey(int legalEntityKey, int offerKey)
        {
            string query =
                String.Format(@"select * from [2AM].[dbo].[LegalEntityAffordability] lea (nolock)
                                join [2am]..AffordabilityType at (nolock) on at.AffordabilityTypeKey = lea.AffordabilityTypeKey
                                where OfferKey={0} and LegalEntityKey={1} and at.IsExpense = 0", offerKey, legalEntityKey);
            var statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Gets the expenses records for the Affordability and Expenses linked to a Legal Entity and an Application
        /// </summary>
        /// <param name="legalEntityKey">LegalEntityKey</param>
        /// <param name="offerKey">OfferKey</param>
        /// <returns></returns>
        public QueryResults GetLegalEntityAffordabilityExpensesByLegalEntityKeyAndOfferKey(int legalEntityKey, int offerKey)
        {
            string query =
                String.Format(@"select * from [2AM].[dbo].[LegalEntityAffordability] lea (nolock)
                                join [2am]..AffordabilityType at (nolock) on at.AffordabilityTypeKey = lea.AffordabilityTypeKey
                                where OfferKey={0} and LegalEntityKey={1} and at.IsExpense = 1", offerKey, legalEntityKey);
            var statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Gets a count of the number of records in the Affordability Type table
        /// </summary>
        /// <returns>int</returns>
        public int CountAffordabilityTypeRecords()
        {
            string query = "select count(*) from [2AM].[dbo].[AffordabilityType] (nolock)";
            var statement = new SQLStatement { StatementString = query };
            var results = dataContext.ExecuteSQLScalar(statement);
            return int.Parse(results.SQLScalarValue);
        }

        /// <summary>
        /// Gets a count of the number of records in the Affordability Type table where the type is an Income Type
        /// </summary>
        /// <returns>int</returns>
        public int CountAffordabilityTypeIncomeRecords()
        {
            string query = "select count(*) from [2AM].[dbo].[AffordabilityType] (nolock) where IsExpense = 0";
            var statement = new SQLStatement { StatementString = query };
            var results = dataContext.ExecuteSQLScalar(statement);
            return int.Parse(results.SQLScalarValue);
        }

        /// <summary>
        /// Gets a count of the number of records in the Affordability Type table where the type is an Expense Type
        /// </summary>
        /// <returns>int</returns>
        public int CountAffordabilityTypeExpenseRecords()
        {
            string query = "select count(*) from [2AM].[dbo].[AffordabilityType] (nolock) where IsExpense = 1";
            var statement = new SQLStatement { StatementString = query };
            var results = dataContext.ExecuteSQLScalar(statement);
            return int.Parse(results.SQLScalarValue);
        }
    }
}