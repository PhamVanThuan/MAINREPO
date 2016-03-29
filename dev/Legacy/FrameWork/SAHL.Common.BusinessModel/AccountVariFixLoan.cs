using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// Derived from account. Instantiated to represent VariFix Loan Accounts.
    /// </summary>
    public partial class AccountVariFixLoan : Account, IAccountVariFixLoan
    {
        protected new SAHL.Common.BusinessModel.DAO.AccountVariFixLoan_DAO _DAO;

        public AccountVariFixLoan(SAHL.Common.BusinessModel.DAO.AccountVariFixLoan_DAO AccountVariFixLoan)
            : base(AccountVariFixLoan)
        {
            this._DAO = AccountVariFixLoan;
        }
    }
}