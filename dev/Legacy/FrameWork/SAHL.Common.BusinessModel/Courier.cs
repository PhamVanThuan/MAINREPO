using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.Courier_DAO
    /// </summary>
    public partial class Courier : BusinessModelBase<SAHL.Common.BusinessModel.DAO.Courier_DAO>, ICourier
    {
        public Courier(SAHL.Common.BusinessModel.DAO.Courier_DAO Courier)
            : base(Courier)
        {
            this._DAO = Courier;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Courier_DAO.CourierName
        /// </summary>
        public String CourierName
        {
            get { return _DAO.CourierName; }
            set { _DAO.CourierName = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Courier_DAO.EmailAddress
        /// </summary>
        public String EmailAddress
        {
            get { return _DAO.EmailAddress; }
            set { _DAO.EmailAddress = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Courier_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }
    }
}