using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.AccountQuickCash_DAO
    /// </summary>
    public partial class AccountQuickCash : Account, IAccountQuickCash
    {
        protected new SAHL.Common.BusinessModel.DAO.AccountQuickCash_DAO _DAO;

        public AccountQuickCash(SAHL.Common.BusinessModel.DAO.AccountQuickCash_DAO AccountQuickCash)
            : base(AccountQuickCash)
        {
            this._DAO = AccountQuickCash;
        }
    }
}