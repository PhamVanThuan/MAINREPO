using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.DAO;


namespace SAHL.Test.DAOHelpers
{
    /// <summary>
    /// A class to help with standard creation and deletion of BulkBatchLog_DAO object for testing purposes.
    /// </summary>
    public class BulkBatchLogHelper : BaseHelper<BulkBatchLog_DAO>
    {
        /// <summary>
        /// Creates a new <see cref="BulkBatchLog_DAO "/> entity.
        /// </summary>
        /// <returns>A new BulkBatchLog_DAO  entity (not yet persisted).</returns>
        public BulkBatchLog_DAO CreateBulkBatchLog()
        {
            BulkBatchLog_DAO BulkBatchLog = new BulkBatchLog_DAO();

            BulkBatchLog.BulkBatch = BulkBatch_DAO.FindFirst();
            BulkBatchLog.Description = "Test Bulk Batch Log Description";
            BulkBatchLog.MessageType = MessageType_DAO.FindFirst();

            CreatedEntities.Add(BulkBatchLog);

            return BulkBatchLog;
        }

        /// <summary>
        /// Ensures that all addresses created are deleted from the database.
        /// </summary>
        public override void Dispose()
        {
            foreach (BulkBatchLog_DAO BulkBatchLog in CreatedEntities)
            {
                if (BulkBatchLog.Key > 0)
                    TestBase.DeleteRecord("BulkBatchLog", "BulkBatchLogKey", BulkBatchLog.Key.ToString());
            }

            CreatedEntities.Clear();
        }
    }
}
