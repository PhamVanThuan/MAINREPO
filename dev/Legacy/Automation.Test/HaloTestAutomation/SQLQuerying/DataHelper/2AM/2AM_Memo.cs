using Common.Enums;
using System;
using System.Collections.Generic;

namespace Automation.DataAccess.DataHelper
{
    public partial class _2AMDataHelper
    {
        /// <summary>
        /// This will retrieve a column value from the Memo table when provided with the GenericKey,
        /// GenericKeyTypeKey and the column to retrieve
        /// </summary>
        /// <param name = "genericKey">GenericKey</param>
        /// <param name = "genericKeyTypeKey">GenericKeyTypeKey</param>
        /// <param name="columnName">Column to return</param>
        /// <returns>Memo.*</returns>
        public string GetMemoColumn(int genericKey, int genericKeyTypeKey, string columnName)
        {
            var statement = new SQLStatement();
            string query =
                String.Format(
                        @"Select {0} from [2am].dbo.memo with (nolock)
                          where GenericKey = {1}
                          and GenericKeyTypeKey = {2}",
                        columnName, genericKey, genericKeyTypeKey);
            statement.StatementString = query;
            var results = dataContext.ExecuteSQLQuery(statement);
            return results.Rows(0).Column(columnName).Value;
        }

        /// <summary>
        /// This retrieve a column value from the most recent Memo record on the Memo table when provided with the GenericKey,
        /// GenericKeyTypeKey and the column to retrieve
        /// </summary>
        /// <param name = "genericKey">GenericKey</param>
        /// <param name = "genericKeyTypeKey">GenericKeyTypeKey</param>
        /// <param name="columnName">Column to return</param>
        /// <returns>Memo.*</returns>
        public Dictionary<int, string> GetLatestMemoColumn(int genericKey, GenericKeyTypeEnum genericKeyTypeKey, string columnName)
        {
            Dictionary<int, string> memo = new Dictionary<int, string>();
            var statement = new SQLStatement();
            string query =
                String.Format(
                        @"Select top 1 {0}, MemoKey from [2am].dbo.Memo with (nolock)
                          where GenericKey = {1}
                          and GenericKeyTypeKey = {2}
                          order by MemoKey desc",
                        columnName, genericKey, (int)genericKeyTypeKey);
            statement.StatementString = query;
            var results = dataContext.ExecuteSQLQuery(statement);
            memo.Add(results.Rows(0).Column("MemoKey").GetValueAs<int>(), results.Rows(0).Column(columnName).GetValueAs<string>());
            return memo;
        }

        /// <summary>
        ///   Returns the Memo record inserted after performing the Application Received action
        /// </summary>
        /// <param name = "accountKey">AccountKey</param>
        /// <param name = "offerKey">Further Lending OfferKey</param>
        /// <returns>memo.*</returns>
        public QueryResults GetFLAppReceivedMemo(int accountKey, int offerKey)
        {
            string query =
                string.Format(
                    @"select top 1 * from [2am].dbo.memo with (nolock)
                            where memo like 'Application: {0}%'
                            and GenericKey = {1} and GenericKeyTypeKey=1
                            order by 1 desc",
                    offerKey, accountKey);
            var statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Inserts a memo record
        /// </summary>
        /// <param name="genericKeyType">genericKeyType</param>
        /// <param name="genericKey">genericKey</param>
        /// <param name="memoText">memoText</param>
        /// <param name="adUserKey">adUserKey</param>
        /// <param name="generalStatus">generalStatus</param>
        public void InsertMemo(GenericKeyTypeEnum genericKeyType, int genericKey, string memoText, int adUserKey, GeneralStatusEnum generalStatus)
        {
            string query =
                string.Format(@"insert into [2am].dbo.Memo
                                (
                                genericKeyTypeKey, GenericKey, InsertedDate, Memo, AduserKey, ChangeDate,
                                generalStatusKey, reminderDate, expiryDate
                                )
                                values
                                ({0}, {1}, getdate(), '{2}',{3}, null, {4}, null, null)",
                                (int)genericKeyType, genericKey, memoText, adUserKey, (int)generalStatus);
            var statement = new SQLStatement { StatementString = query };
            dataContext.ExecuteNonSQLQuery(statement);
        }
    }
}