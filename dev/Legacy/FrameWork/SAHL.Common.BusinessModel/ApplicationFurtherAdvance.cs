using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// Derived from the Application_DAO. Instantiated to represent a Further Advance Application. This object is used to facilitate
    /// the origination of a Further Advance on a Mortgage Loan Account.
    /// </summary>
    public partial class ApplicationFurtherAdvance : Application, IApplicationFurtherAdvance
    {
        protected new SAHL.Common.BusinessModel.DAO.ApplicationFurtherAdvance_DAO _DAO;

        public ApplicationFurtherAdvance(SAHL.Common.BusinessModel.DAO.ApplicationFurtherAdvance_DAO ApplicationFurtherAdvance)
            : base(ApplicationFurtherAdvance)
        {
            this._DAO = ApplicationFurtherAdvance;
            OnConstruction();
        }
    }
}