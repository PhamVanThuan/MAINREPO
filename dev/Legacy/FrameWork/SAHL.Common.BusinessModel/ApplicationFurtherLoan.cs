using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// Derived from the Application_DAO. Instantiated to represent a Further Loan Application. This object is used to facilitate
    /// the origination of a Further Loan on a Mortgage Loan Account.
    /// </summary>
    public partial class ApplicationFurtherLoan : Application, IApplicationFurtherLoan
    {
        protected new SAHL.Common.BusinessModel.DAO.ApplicationFurtherLoan_DAO _DAO;

        public ApplicationFurtherLoan(SAHL.Common.BusinessModel.DAO.ApplicationFurtherLoan_DAO ApplicationFurtherLoan)
            : base(ApplicationFurtherLoan)
        {
            this._DAO = ApplicationFurtherLoan;
            OnConstruction();
        }
    }
}