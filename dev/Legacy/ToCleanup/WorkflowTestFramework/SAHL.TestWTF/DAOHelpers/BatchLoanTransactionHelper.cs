using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.DAO;


namespace SAHL.TestWTF.DAOHelpers
{
    /// <summary>
    /// A class to help with standard creation and deletion of BatchLoanTransaction_DAO object for testing purposes.
    /// </summary>
    public class BatchLoanTransactionHelper : BaseHelper<BatchLoanTransaction_DAO>
    {
        /// <summary>
        /// Creates a new <see cref="BatchLoanTransaction_DAO "/> entity.
        /// </summary>
        /// <returns>A new BatchLoanTransaction_DAO  entity (not yet persisted).</returns>
        public BatchLoanTransaction_DAO CreateBatchTransaction()
        {
            BatchLoanTransaction_DAO BatchLoanTransaction = new BatchLoanTransaction_DAO();

            BatchLoanTransaction.BatchTransaction = BatchTransaction_DAO.FindFirst();
            BatchLoanTransaction.LoanTransactionNumber = 4242; // fictitious;

            CreatedEntities.Add(BatchLoanTransaction);

            return BatchLoanTransaction;
        }

        /// <summary>
        /// Ensures that all addresses created are deleted from the database.
        /// </summary>
        public override void Dispose()
        {
            foreach (BatchLoanTransaction_DAO BatchTransaction in CreatedEntities)
            {
                if (BatchTransaction.Key > 0)
                    TestBase.DeleteRecord("BatchLoanTransaction", "BatchLoanTransactionKey", BatchTransaction.Key.ToString());
            }

            CreatedEntities.Clear();
        }
    }
}
