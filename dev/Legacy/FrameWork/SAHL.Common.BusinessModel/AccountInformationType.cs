using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.AccountInformationType_DAO
    /// </summary>
    public partial class AccountInformationType : BusinessModelBase<SAHL.Common.BusinessModel.DAO.AccountInformationType_DAO>, IAccountInformationType
    {
        public AccountInformationType(SAHL.Common.BusinessModel.DAO.AccountInformationType_DAO AccountInformationType)
            : base(AccountInformationType)
        {
            this._DAO = AccountInformationType;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountInformationType_DAO.Description
        /// </summary>
        public String Description
        {
            get { return _DAO.Description; }
            set { _DAO.Description = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountInformationType_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }
    }
}