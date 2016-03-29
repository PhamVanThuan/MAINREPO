using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// Derived from the Account_DAO and is used to instantiate a Defending Discount Rate Account.
    /// </summary>
    public partial class AccountDefendingDiscountRate : Account, IAccountDefendingDiscountRate
    {
        protected new SAHL.Common.BusinessModel.DAO.AccountDefendingDiscountRate_DAO _DAO;

        public AccountDefendingDiscountRate(SAHL.Common.BusinessModel.DAO.AccountDefendingDiscountRate_DAO AccountDefendingDiscountRate)
            : base(AccountDefendingDiscountRate)
        {
            this._DAO = AccountDefendingDiscountRate;
        }
    }
}