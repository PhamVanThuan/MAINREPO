using Common.Enums;
using System;
using System.Collections.Generic;

namespace Automation.DataAccess.DataHelper
{
    public partial class _2AMDataHelper
    {
        /// <summary>
        /// Removes all of the Detail Types on an account
        /// </summary>
        /// <param name="accountKey"></param>
        /// <param name="detailType"></param>
        public void RemoveDetailTypes(int accountKey, DetailTypeEnum detailType)
        {
            string query = string.Empty;
            if (detailType != 0)
            {
                query = string.Format(@"delete from [2am].dbo.Detail where accountKey = {0} and detailtypekey = {1}", accountKey, (int)detailType);
            }
            else
            {
                query =
                    string.Format(@"delete from [2am].dbo.Detail where accountKey = {0}", accountKey);
            }

            SQLStatement statement = new SQLStatement { StatementString = query };
            dataContext.ExecuteNonSQLQuery(statement);
        }

        /// <summary>
        /// Inserts a detail type on an account
        /// </summary>
        /// <param name="detailType">detail type to insert</param>
        /// <param name="accountKey">account number</param>
        public void InsertDetailType(DetailTypeEnum detailType, int accountKey)
        {
            string query =
                string.Format(@"insert into [2am].dbo.Detail
                                (DetailTypeKey, AccountKey, DetailDate, UserID, ChangeDate)
                                values ({0},{1}, getdate(), 'SAHL\TestUser', getdate())", (int)detailType, accountKey);
            SQLStatement statement = new SQLStatement { StatementString = query };
            dataContext.ExecuteNonSQLQuery(statement);
        }

        /// <summary>
        /// This will get one or more loan details.
        /// </summary>
        public IEnumerable<Automation.DataModels.LoanDetail> GetLoanDetailRecords(int accountkey = 0)
        {
            IEnumerable<Automation.DataModels.LoanDetail> loanDetail;
            string query = accountkey == 0 ? "select * from dbo.detail" : String.Format("select * from dbo.detail where accountkey = {0}", accountkey);
            loanDetail = dataContext.Query<Automation.DataModels.LoanDetail>(query);
            return loanDetail;
        }

        /// <summary>
        /// This will get one or more loan detail type.
        /// </summary>
        public IEnumerable<Automation.DataModels.DetailType> GetLoanDetailTypeRecords(int detailTypeKey = 0)
        {
            IEnumerable<Automation.DataModels.DetailType> loanDetail;
            string query = detailTypeKey == 0 ? "select * from dbo.detailtype" : String.Format("select * from dbo.detailtype where detailtypekey = {0}", detailTypeKey);
            loanDetail = dataContext.Query<Automation.DataModels.DetailType>(query);
            return loanDetail;
        }

        /// <summary>
        /// This will get one or more loan detail class.
        /// </summary>
        public IEnumerable<Automation.DataModels.DetailClass> GetDetailClassRecords(int detailclasskey = 0)
        {
            IEnumerable<Automation.DataModels.DetailClass> loanDetailClass;
            string query = detailclasskey == 0 ? "select * from dbo.detailclass" : String.Format("select * from dbo.detailclass where detailclasskey = {0}", detailclasskey);
            loanDetailClass = dataContext.Query<Automation.DataModels.DetailClass>(query);
            return loanDetailClass;
        }
    }
}