using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// Derived from the Application_DAO and instantiated to represent an Unsecured Lending Application.
    /// DiscriminatorValue = "11"
    /// </summary>
    public partial class ApplicationUnsecuredLending : Application, IApplicationUnsecuredLending
    {
        protected new SAHL.Common.BusinessModel.DAO.ApplicationUnsecuredLending_DAO _DAO;

        public ApplicationUnsecuredLending(SAHL.Common.BusinessModel.DAO.ApplicationUnsecuredLending_DAO ApplicationUnsecuredLending)
            : base(ApplicationUnsecuredLending)
        {
            this._DAO = ApplicationUnsecuredLending;
        }
    }
}