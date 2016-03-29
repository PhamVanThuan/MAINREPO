using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.AccountIndicationType_DAO
    /// </summary>
    public partial class AccountIndicationType : BusinessModelBase<SAHL.Common.BusinessModel.DAO.AccountIndicationType_DAO>, IAccountIndicationType
    {
        public AccountIndicationType(SAHL.Common.BusinessModel.DAO.AccountIndicationType_DAO AccountIndicationType)
            : base(AccountIndicationType)
        {
            this._DAO = AccountIndicationType;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountIndicationType_DAO.Description
        /// </summary>
        public String Description
        {
            get { return _DAO.Description; }
            set { _DAO.Description = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountIndicationType_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }
    }
}