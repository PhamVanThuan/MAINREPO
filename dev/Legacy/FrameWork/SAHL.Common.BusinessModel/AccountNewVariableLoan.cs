using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// Derived from Account_DAO and is instantiated to represent new Variable Loan accounts.
    /// </summary>
    public partial class AccountNewVariableLoan : Account, IAccountNewVariableLoan
    {
        protected new SAHL.Common.BusinessModel.DAO.AccountNewVariableLoan_DAO _DAO;

        public AccountNewVariableLoan(SAHL.Common.BusinessModel.DAO.AccountNewVariableLoan_DAO AccountNewVariableLoan)
            : base(AccountNewVariableLoan)
        {
            this._DAO = AccountNewVariableLoan;
        }
    }
}