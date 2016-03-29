using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.BalanceType_DAO
    /// </summary>
    public partial class BalanceType : BusinessModelBase<SAHL.Common.BusinessModel.DAO.BalanceType_DAO>, IBalanceType
    {
        public BalanceType(SAHL.Common.BusinessModel.DAO.BalanceType_DAO BalanceType)
            : base(BalanceType)
        {
            this._DAO = BalanceType;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BalanceType_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BalanceType_DAO.Description
        /// </summary>
        public String Description
        {
            get { return _DAO.Description; }
            set { _DAO.Description = value; }
        }
    }
}