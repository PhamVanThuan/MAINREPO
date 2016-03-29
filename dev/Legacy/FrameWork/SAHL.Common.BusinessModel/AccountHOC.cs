using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// Derived from Account_DAO and is instantiated to represent HOC accounts.
    /// </summary>
    public partial class AccountHOC : Account, IAccountHOC
    {
        protected new SAHL.Common.BusinessModel.DAO.AccountHOC_DAO _DAO;

        public AccountHOC(SAHL.Common.BusinessModel.DAO.AccountHOC_DAO AccountHOC)
            : base(AccountHOC)
        {
            this._DAO = AccountHOC;
        }
    }
}