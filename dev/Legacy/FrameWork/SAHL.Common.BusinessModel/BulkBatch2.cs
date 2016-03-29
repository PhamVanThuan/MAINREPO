using System.Collections.Generic;
using System.Data.SqlClient;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    ///
    /// </summary>
    public partial class BulkBatch : BusinessModelBase<SAHL.Common.BusinessModel.DAO.BulkBatch_DAO>, IBulkBatch
    {
        public override void OnPopulateRules(List<string> Rules)
        {
            base.OnPopulateRules(Rules);
            Rules.Add("BulkBatchIdentifierReferenceKeyMandatory");
            Rules.Add("BulkBatchExportArrearBalanceParameterMandatory");
            Rules.Add("BulkBatchExportSPVParameterMandatory");
            Rules.Add("BulkBatchImportFileParameterMandatory");
            Rules.Add("BulkBatchEffectiveDateMandatory");
        }

        //         DeleteBatchAndTransactionsByBatchKey?
        //DELETE FROM [2AM].[dbo].[BulkBatchLog?] WHERE BulkBatchKey? = @BulkBatchKey?;
        //DELETE FROM [2AM].[dbo].[BatchTransaction?] WHERE BulkBatchKey? = @BulkBatchKey?;
        //DELETE FROM [2AM].[dbo].[BulkBatchParameter?] WHERE BulkBatchKey? = @BulkBatchKey?;
        //DELETE FROM [2AM].[dbo].[BulkBatch?] WHERE BulkBatchKey? = @BulkBatchKey?;

        public static void DeleteBatchAndTransactions(IDomainMessageCollection messages, int BulkBatchKey)
        {
            // Get Query
            string query = "DELETE FROM [2AM].[dbo].[BulkBatchLog] WHERE BulkBatchKey = @BulkBatchKey; "
                + "DELETE FROM [2AM].[dbo].[BatchTransaction] WHERE BulkBatchKey = @BulkBatchKey; "
                + "DELETE FROM [2AM].[dbo].[BulkBatchParameter] WHERE BulkBatchKey = @BulkBatchKey; "
                + "DELETE FROM [2AM].[dbo].[BulkBatch] WHERE BulkBatchKey = @BulkBatchKey;";

            // Add the required parameters
            ParameterCollection parameters = new ParameterCollection();
            parameters.Add(new SqlParameter("@BulkBatchKey", BulkBatchKey));

            // Execute Query
            CastleTransactionsServiceHelper.ExecuteNonQueryOnCastleTran(query, typeof(GeneralStatus_DAO), parameters);

            //BulkBatchLog_DAO bb;
            //bb.BulkBatch.Key;

            //string HQL = "from BulkBatchLog_DAO bb where bb.BulkBatch.Key = ?";
            //SimpleQuery<BulkBatchLog_DAO> q1 = new SimpleQuery<BulkBatchLog_DAO>(HQL, BatchKey);
            //BulkBatchLog_DAO[] res1 = q1.Execute();

            //for (int i = 0; i < res1.Length; i++)
            //{
            //    res1[i].Delete();
            //}

            //HQL = "from BatchTransaction_DAO bt where bt.BulkBatch.Key = ?";
            //SimpleQuery<BatchTransaction_DAO> q2 = new SimpleQuery<BatchTransaction_DAO>(HQL, BatchKey);
            //BatchTransaction_DAO[] res2 = q2.Execute();

            //for (int i = 0; i < res2.Length; i++)
            //{
            //    res2[i].Delete();
            //}

            //HQL = "from BulkBatchParameter_DAO bt where bt.BulkBatch.Key = ?";
            //SimpleQuery<BulkBatchParameter_DAO> q3 = new SimpleQuery<BulkBatchParameter_DAO>(HQL, BatchKey);
            //BulkBatchParameter_DAO[] res3 = q3.Execute();

            //for (int i = 0; i < res3.Length; i++)
            //{
            //    res3[i].Delete();
            //}

            //HQL = "from BulkBatch_DAO bt where bt.BulkBatch.Key = ?";
            //SimpleQuery<BulkBatch_DAO> q4 = new SimpleQuery<BulkBatch_DAO>(HQL, BatchKey);
            //BulkBatch_DAO[] res4 = q4.Execute();

            //for (int i = 0; i < res4.Length; i++)
            //{
            //    res4[i].Delete();
            //}
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnBatchTransactions_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnBatchTransactions_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnBulkBatchLogs_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnBulkBatchLogs_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnBulkBatchParameters_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnBulkBatchParameters_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnBatchTransactions_AfterAdd(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnBatchTransactions_AfterRemove(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnBulkBatchLogs_AfterAdd(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnBulkBatchLogs_AfterRemove(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnBulkBatchParameters_AfterAdd(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnBulkBatchParameters_AfterRemove(ICancelDomainArgs args, object Item)
        {
        }
    }
}