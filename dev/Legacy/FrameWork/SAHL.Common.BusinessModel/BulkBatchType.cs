using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.BulkBatchType_DAO
    /// </summary>
    public partial class BulkBatchType : BusinessModelBase<SAHL.Common.BusinessModel.DAO.BulkBatchType_DAO>, IBulkBatchType
    {
        public BulkBatchType(SAHL.Common.BusinessModel.DAO.BulkBatchType_DAO BulkBatchType)
            : base(BulkBatchType)
        {
            this._DAO = BulkBatchType;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BulkBatchType_DAO.Description
        /// </summary>
        public String Description
        {
            get { return _DAO.Description; }
            set { _DAO.Description = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BulkBatchType_DAO.FilePath
        /// </summary>
        public String FilePath
        {
            get { return _DAO.FilePath; }
            set { _DAO.FilePath = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BulkBatchType_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }
    }
}