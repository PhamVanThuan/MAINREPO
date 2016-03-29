using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.BulkBatchStatus_DAO
    /// </summary>
    public partial class BulkBatchStatus : BusinessModelBase<SAHL.Common.BusinessModel.DAO.BulkBatchStatus_DAO>, IBulkBatchStatus
    {
        public BulkBatchStatus(SAHL.Common.BusinessModel.DAO.BulkBatchStatus_DAO BulkBatchStatus)
            : base(BulkBatchStatus)
        {
            this._DAO = BulkBatchStatus;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BulkBatchStatus_DAO.Description
        /// </summary>
        public String Description
        {
            get { return _DAO.Description; }
            set { _DAO.Description = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BulkBatchStatus_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }
    }
}