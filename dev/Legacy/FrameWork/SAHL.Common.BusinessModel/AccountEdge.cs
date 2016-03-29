using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// Derived from Account_DAO and is instantiated to represent new Edge accounts.
    /// </summary>
    public partial class AccountEdge : Account, IAccountEdge
    {
        protected new SAHL.Common.BusinessModel.DAO.AccountEdge_DAO _DAO;

        public AccountEdge(SAHL.Common.BusinessModel.DAO.AccountEdge_DAO AccountEdge)
            : base(AccountEdge)
        {
            this._DAO = AccountEdge;
        }
    }
}