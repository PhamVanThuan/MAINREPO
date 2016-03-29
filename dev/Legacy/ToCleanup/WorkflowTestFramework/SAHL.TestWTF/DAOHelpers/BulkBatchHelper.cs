using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.DAO;


namespace SAHL.TestWTF.DAOHelpers
{
    /// <summary>
    /// A class to help with standard creation and deletion of BulkBatch_DAO object for testing purposes.
    /// </summary>
    public class BulkBatchHelper : BaseHelper<BulkBatch_DAO>
    {
        /// <summary>
        /// Creates a new <see cref="BulkBatch_DAO "/> entity.
        /// </summary>
        /// <returns>A new BulkBatch_DAO  entity (not yet persisted).</returns>
        public BulkBatch_DAO CreateBulkBatch()
        {
            BulkBatch_DAO BulkBatch = new BulkBatch_DAO();

            BulkBatch.BulkBatchStatus = BulkBatchStatus_DAO.FindFirst();
            BulkBatch.Description = "Test Description";
            BulkBatch.BulkBatchType = BulkBatchType_DAO.FindFirst();
            BulkBatch.EffectiveDate = DateTime.Now;
            BulkBatch.BulkBatchStatus = BulkBatchStatus_DAO.FindFirst();

            CreatedEntities.Add(BulkBatch);

            return BulkBatch;
        }

        /// <summary>
        /// Ensures that all addresses created are deleted from the database.
        /// </summary>
        public override void Dispose()
        {
            foreach (BulkBatch_DAO BulkBatch in CreatedEntities)
            {
                if (BulkBatch.Key > 0)
                    TestBase.DeleteRecord("BulkBatch", "BulkBatchKey", BulkBatch.Key.ToString());
            }

            CreatedEntities.Clear();
        }
    }
}
