using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.CDCIM900Exceptions_DAO
    /// </summary>
    public partial class CDCIM900Exceptions : BusinessModelBase<SAHL.Common.BusinessModel.DAO.CDCIM900Exceptions_DAO>, ICDCIM900Exceptions
    {
        public CDCIM900Exceptions(SAHL.Common.BusinessModel.DAO.CDCIM900Exceptions_DAO CDCIM900Exceptions)
            : base(CDCIM900Exceptions)
        {
            this._DAO = CDCIM900Exceptions;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CDCIM900Exceptions_DAO.FileDate
        /// </summary>
        public DateTime FileDate
        {
            get { return _DAO.FileDate; }
            set { _DAO.FileDate = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CDCIM900Exceptions_DAO.RecordNumber
        /// </summary>
        public Int32 RecordNumber
        {
            get { return _DAO.RecordNumber; }
            set { _DAO.RecordNumber = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CDCIM900Exceptions_DAO.BranchNumber
        /// </summary>
        public Int32 BranchNumber
        {
            get { return _DAO.BranchNumber; }
            set { _DAO.BranchNumber = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CDCIM900Exceptions_DAO.Exception
        /// </summary>
        public String Exception
        {
            get { return _DAO.Exception; }
            set { _DAO.Exception = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CDCIM900Exceptions_DAO.ActionDate
        /// </summary>
        public DateTime ActionDate
        {
            get { return _DAO.ActionDate; }
            set { _DAO.ActionDate = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CDCIM900Exceptions_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }
    }
}