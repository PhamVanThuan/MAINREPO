using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// Derived from the Application_DAO and instantiated to represent a ReAdvance Application.
    /// This object is used to facilitate the origination of a re advance on a mortgage loan account.
    /// </summary>
    public partial class ApplicationReAdvance : Application, IApplicationReAdvance
    {
        protected new SAHL.Common.BusinessModel.DAO.ApplicationReAdvance_DAO _DAO;

        public ApplicationReAdvance(SAHL.Common.BusinessModel.DAO.ApplicationReAdvance_DAO ApplicationReAdvance)
            : base(ApplicationReAdvance)
        {
            this._DAO = ApplicationReAdvance;
            OnConstruction();
        }
    }
}