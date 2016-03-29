using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.CreditScoreDecision_DAO
    /// </summary>
    public partial interface ICreditScoreDecision : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CreditScoreDecision_DAO.Description
        /// </summary>
        System.String Description
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CreditScoreDecision_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }
    }
}