using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.DAO;


namespace SAHL.TestWTF.DAOHelpers
{
    /// <summary>
    /// A class to help with standard creation and deletion of BulkBatchParameter_DAO object for testing purposes.
    /// </summary>
    public class BulkBatchParameterHelper : BaseHelper<BulkBatchParameter_DAO>
    {
        /// <summary>
        /// Creates a new <see cref="BulkBatchParameter_DAO "/> entity.
        /// </summary>
        /// <returns>A new BulkBatchParameter_DAO  entity (not yet persisted).</returns>
        public BulkBatchParameter_DAO CreateBulkBatchParameter()
        {
            BulkBatchParameter_DAO BulkBatchParameter = new BulkBatchParameter_DAO();

            BulkBatchParameter.BulkBatch = BulkBatch_DAO.FindFirst();
            BulkBatchParameter.ParameterName = "Test Param";
            BulkBatchParameter.ParameterValue = "Test Value";

            CreatedEntities.Add(BulkBatchParameter);

            return BulkBatchParameter;
        }

        /// <summary>
        /// Ensures that all addresses created are deleted from the database.
        /// </summary>
        public override void Dispose()
        {
            foreach (BulkBatchParameter_DAO BulkBatchParameter in CreatedEntities)
            {
                if (BulkBatchParameter.Key > 0)
                    TestBase.DeleteRecord("BulkBatchParameter", "BulkBatchParameterKey", BulkBatchParameter.Key.ToString());
            }

            CreatedEntities.Clear();
        }
    }
}
