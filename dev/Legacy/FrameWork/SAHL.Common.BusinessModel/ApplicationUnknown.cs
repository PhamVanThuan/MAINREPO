using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// Derived from the Application_DAO and instantiated to represent a Unknown Application.
    /// </summary>
    public partial class ApplicationUnknown : Application, IApplicationUnknown
    {
        protected new SAHL.Common.BusinessModel.DAO.ApplicationUnknown_DAO _DAO;

        public ApplicationUnknown(SAHL.Common.BusinessModel.DAO.ApplicationUnknown_DAO ApplicationUnknown)
            : base(ApplicationUnknown)
        {
            this._DAO = ApplicationUnknown;
        }
    }
}