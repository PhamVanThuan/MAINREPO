using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.BatchTransactionStatus_DAO
    /// </summary>
    public partial class BatchTransactionStatus : BusinessModelBase<SAHL.Common.BusinessModel.DAO.BatchTransactionStatus_DAO>, IBatchTransactionStatus
    {
        public BatchTransactionStatus(SAHL.Common.BusinessModel.DAO.BatchTransactionStatus_DAO BatchTransactionStatus)
            : base(BatchTransactionStatus)
        {
            this._DAO = BatchTransactionStatus;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BatchTransactionStatus_DAO.Description
        /// </summary>
        public String Description
        {
            get { return _DAO.Description; }
            set { _DAO.Description = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BatchTransactionStatus_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }
    }
}