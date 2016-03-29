using System.Collections.Generic;

namespace Automation.DataAccess.DataHelper
{
    public partial class _2AMDataHelper
    {
        /// <summary>
        /// Get all the attorney invoices for an account.
        /// </summary>
        /// <param name="accountkey"></param>
        /// <returns></returns>
        public IEnumerable<Automation.DataModels.AttorneyInvoice> GetAttorneyInvoices(int accountkey)
        {
            var invoice =
                dataContext.Query<Automation.DataModels.AttorneyInvoice>
                (string.Format(@"Select * from [2am].dbo.accountattorneyinvoice with (nolock)
                                        where accountkey = {0} ", accountkey));
            return invoice;
        }

        /// <summary>
        /// Returns the AttorneyKey when provided with the LegalEntityKey on the Attorney table
        /// </summary>
        /// <param name="legalentitykey"></param>
        /// <returns></returns>
        public int GetAttorneyKeyByLegalEntityKey(int legalentitykey)
        {
            string query =
                string.Format(@"select * from [2am]..Attorney where legalEntityKey={0}", legalentitykey);
            SQLStatement statement = new SQLStatement { StatementString = query };
            var r = dataContext.ExecuteSQLQuery(statement);
            return r.Rows(0).Column("AttorneyKey").GetValueAs<int>();
        }

        /// <summary>
        /// Get the first deedsoffice name that has at least one active attorney
        /// </summary>
        /// <returns></returns>
        public string GetFirstDeedsOfficeNameWithActiveAttorneys()
        {
            string query = @"select top 01 deedsoffice.description from [2am].dbo.deedsoffice
	                            inner join dbo.attorney
		                            on deedsoffice.deedsofficekey = attorney.deedsofficekey
                             where attorney.generalstatuskey = 1";
            SQLStatement statement = new SQLStatement { StatementString = query };
            var r = dataContext.ExecuteSQLScalar(statement);
            return r.SQLScalarValue;
        }

        /// <summary>
        /// Get an AttorneyContact by Attorney LegalEntity
        /// </summary>
        /// <returns></returns>
        public QueryResultsRow GetAttorneyContactRecord(string firstnames, string surname)
        {
            string query = string.Format(@"select top 01 * from dbo.externalrole
	                                        inner join dbo.generickeytype
		                                        on externalrole.generickeytypekey = generickeytype.generickeytypekey
		                                        and generickeytype.generickeytypekey = 35
	                                        inner join dbo.legalentity
		                                        on externalrole.legalentitykey = legalentity.legalentitykey
                                       where legalentity.firstnames = '{0}' and legalentity.surname = '{1}'", firstnames, surname);
            SQLStatement statement = new SQLStatement { StatementString = query };
            QueryResults r = dataContext.ExecuteSQLQuery(statement);
            if (r.RowList.Count == 0)
                return default(QueryResultsRow);
            return r.Rows(0);
        }

        /// <summary>
        /// Get an Attorney Record by registeredname
        /// </summary>
        /// <returns></returns>
        public QueryResults GetAttorneys()
        {
            string query =
                string.Format(@"select attorney.*,legalentity.* from dbo.legalentity
                                inner join dbo.attorney
                                on legalentity.legalentitykey = attorney.legalentitykey");
            var statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        public void SetRegistrationInd(int attorneyKey)
        {
            string query =
                string.Format(@"update attorney
                               set Attorneyregistrationind = 1
                               where attorneykey = '{0}'", attorneyKey);
            var statement = new SQLStatement { StatementString = query };
            dataContext.ExecuteNonSQLQuery(statement);
        }
    }
}