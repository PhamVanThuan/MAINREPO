using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// CapStatus_DAO is used to hold the different statuses that a CapOffer is assigned.
    /// </summary>
    public partial class CapStatus : BusinessModelBase<SAHL.Common.BusinessModel.DAO.CapStatus_DAO>, ICapStatus
    {
        public CapStatus(SAHL.Common.BusinessModel.DAO.CapStatus_DAO CapStatus)
            : base(CapStatus)
        {
            this._DAO = CapStatus;
        }

        /// <summary>
        /// The CapStatus Description e.g. Open CAP 2 Offer, Forms Sent
        /// </summary>
        public String Description
        {
            get { return _DAO.Description; }
            set { _DAO.Description = value; }
        }

        /// <summary>
        /// Primary Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }
    }
}