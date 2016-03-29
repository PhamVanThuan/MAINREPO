using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.AccountStatus_DAO
    /// </summary>
    public partial class AccountStatus : BusinessModelBase<SAHL.Common.BusinessModel.DAO.AccountStatus_DAO>, IAccountStatus
    {
        public AccountStatus(SAHL.Common.BusinessModel.DAO.AccountStatus_DAO AccountStatus)
            : base(AccountStatus)
        {
            this._DAO = AccountStatus;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountStatus_DAO.Description
        /// </summary>
        public String Description
        {
            get { return _DAO.Description; }
            set { _DAO.Description = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountStatus_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }
    }
}