using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// Derived from Account_DAO and is instantiated to represent Variable Loan accounts.
    /// </summary>
    public partial class AccountVariableLoan : Account, IAccountVariableLoan
    {
        protected new SAHL.Common.BusinessModel.DAO.AccountVariableLoan_DAO _DAO;

        public AccountVariableLoan(SAHL.Common.BusinessModel.DAO.AccountVariableLoan_DAO AccountVariableLoan)
            : base(AccountVariableLoan)
        {
            this._DAO = AccountVariableLoan;
        }
    }
}