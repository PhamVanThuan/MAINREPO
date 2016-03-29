using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ApplicationMortgageLoanRefinance_DAO
    /// </summary>
    public partial class ApplicationMortgageLoanRefinance : Application, IApplicationMortgageLoanRefinance
    {
        protected new SAHL.Common.BusinessModel.DAO.ApplicationMortgageLoanRefinance_DAO _DAO;

        public ApplicationMortgageLoanRefinance(SAHL.Common.BusinessModel.DAO.ApplicationMortgageLoanRefinance_DAO ApplicationMortgageLoanRefinance)
            : base(ApplicationMortgageLoanRefinance)
        {
            this._DAO = ApplicationMortgageLoanRefinance;
            OnConstruction();
        }
    }
}