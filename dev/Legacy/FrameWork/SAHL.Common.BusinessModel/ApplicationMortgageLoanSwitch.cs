using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ApplicationMortgageLoanSwitch_DAO
    /// </summary>
    public partial class ApplicationMortgageLoanSwitch : Application, IApplicationMortgageLoanSwitch
    {
        protected new SAHL.Common.BusinessModel.DAO.ApplicationMortgageLoanSwitch_DAO _DAO;

        public ApplicationMortgageLoanSwitch(SAHL.Common.BusinessModel.DAO.ApplicationMortgageLoanSwitch_DAO ApplicationMortgageLoanSwitch)
            : base(ApplicationMortgageLoanSwitch)
        {
            this._DAO = ApplicationMortgageLoanSwitch;
            OnConstruction();
        }
    }
}