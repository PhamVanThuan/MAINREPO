using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ApplicationMortgageLoanNewPurchase_DAO
    /// </summary>
    public partial class ApplicationMortgageLoanNewPurchase : Application, IApplicationMortgageLoanNewPurchase
    {
        protected new SAHL.Common.BusinessModel.DAO.ApplicationMortgageLoanNewPurchase_DAO _DAO;

        public ApplicationMortgageLoanNewPurchase(SAHL.Common.BusinessModel.DAO.ApplicationMortgageLoanNewPurchase_DAO ApplicationMortgageLoanNewPurchase)
            : base(ApplicationMortgageLoanNewPurchase)
        {
            this._DAO = ApplicationMortgageLoanNewPurchase;
            OnConstruction();
        }
    }
}