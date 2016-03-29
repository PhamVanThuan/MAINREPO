using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.CapPaymentOption_DAO
    /// </summary>
    public partial class CapPaymentOption : BusinessModelBase<SAHL.Common.BusinessModel.DAO.CapPaymentOption_DAO>, ICapPaymentOption
    {
        public CapPaymentOption(SAHL.Common.BusinessModel.DAO.CapPaymentOption_DAO CapPaymentOption)
            : base(CapPaymentOption)
        {
            this._DAO = CapPaymentOption;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CapPaymentOption_DAO.Description
        /// </summary>
        public String Description
        {
            get { return _DAO.Description; }
            set { _DAO.Description = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CapPaymentOption_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }
    }
}