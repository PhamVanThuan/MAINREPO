using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.CancellationReason_DAO
    /// </summary>
    public partial class CancellationReason : BusinessModelBase<SAHL.Common.BusinessModel.DAO.CancellationReason_DAO>, ICancellationReason
    {
        public CancellationReason(SAHL.Common.BusinessModel.DAO.CancellationReason_DAO CancellationReason)
            : base(CancellationReason)
        {
            this._DAO = CancellationReason;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CancellationReason_DAO.Description
        /// </summary>
        public String Description
        {
            get { return _DAO.Description; }
            set { _DAO.Description = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CancellationReason_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }
    }
}