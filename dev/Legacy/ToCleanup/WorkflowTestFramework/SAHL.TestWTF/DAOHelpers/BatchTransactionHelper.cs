using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.DAO;


namespace SAHL.TestWTF.DAOHelpers
{
    public class BatchTransactionHelper : BaseHelper<BatchTransaction_DAO>
    {
        /// <summary>
        /// Creates a new <see cref="BatchTransaction_DAO"/> entity.
        /// </summary>
        /// <returns>A new BatchTransaction_DAO entity (not yet persisted).</returns>
        public BatchTransaction_DAO CreateBatchTransaction()
        {
            BatchTransaction_DAO BatchTransaction = new BatchTransaction_DAO();
            
            BatchTransaction.Amount = 42000;
            BatchTransaction.Reference = "Test Ref";
            BatchTransaction.UserID = "TestUser";
            BatchTransaction.TransactionTypeNumber = 911; // Void
            BatchTransaction.EffectiveDate = DateTime.Now;
            BatchTransaction.Account = Account_DAO.FindFirst();
            BatchTransactionStatus_DAO[] EveryBody = BatchTransactionStatus_DAO.FindAll();
            for (int i = 0; i < EveryBody.Length; i++)
            { 
                if (EveryBody[i].Key == 0)
                {
                    BatchTransaction.BatchTransactionStatus = EveryBody[i];
                    break;
                }
            }

            BatchTransaction.BulkBatch = BulkBatch_DAO.FindFirst();

            CreatedEntities.Add(BatchTransaction);

            return BatchTransaction;
        }

        /// <summary>
        /// Ensures that all addresses created are deleted from the database.
        /// </summary>
        public override void Dispose()
        {
            foreach (BatchTransaction_DAO BatchTransaction in CreatedEntities)
            {
                if (BatchTransaction.Key > 0)
                    TestBase.DeleteRecord("BatchTransaction", "BatchTransactionKey", BatchTransaction.Key.ToString());
            }

            CreatedEntities.Clear();
        }
    }    
}
