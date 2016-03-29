using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// Derived from the Account_DAO and is instantiated to represent new Super Lo Loan Accounts.
    /// </summary>
    public partial class AccountSuperLo : Account, IAccountSuperLo
    {
        protected new SAHL.Common.BusinessModel.DAO.AccountSuperLo_DAO _DAO;

        public AccountSuperLo(SAHL.Common.BusinessModel.DAO.AccountSuperLo_DAO AccountSuperLo)
            : base(AccountSuperLo)
        {
            this._DAO = AccountSuperLo;
        }
    }
}