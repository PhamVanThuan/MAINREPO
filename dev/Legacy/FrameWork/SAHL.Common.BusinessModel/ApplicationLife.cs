using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// This object is used to facilitate the origination of a life insurance policy.
    /// </summary>
    public partial class ApplicationLife : Application, IApplicationLife
    {
        protected new SAHL.Common.BusinessModel.DAO.ApplicationLife_DAO _DAO;

        public ApplicationLife(SAHL.Common.BusinessModel.DAO.ApplicationLife_DAO ApplicationLife)
            : base(ApplicationLife)
        {
            this._DAO = ApplicationLife;
            OnConstruction();
        }
    }
}