using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.AccountSequence_DAO
    /// </summary>
    public partial class AccountSequence : BusinessModelBase<SAHL.Common.BusinessModel.DAO.AccountSequence_DAO>, IAccountSequence
    {
        public AccountSequence(SAHL.Common.BusinessModel.DAO.AccountSequence_DAO AccountSequence)
            : base(AccountSequence)
        {
            this._DAO = AccountSequence;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountSequence_DAO.IsUsed
        /// </summary>
        public Boolean IsUsed
        {
            get { return _DAO.IsUsed; }
            set { _DAO.IsUsed = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountSequence_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }
    }
}