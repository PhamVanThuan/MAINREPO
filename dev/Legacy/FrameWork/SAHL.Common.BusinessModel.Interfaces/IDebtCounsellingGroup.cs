using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.DebtCounsellingGroup_DAO
    /// </summary>
    public partial interface IDebtCounsellingGroup : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DebtCounsellingGroup_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DebtCounsellingGroup_DAO.CreatedDate
        /// </summary>
        System.DateTime CreatedDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DebtCounsellingGroup_DAO.DebtCounsellingCases
        /// </summary>
        IEventList<IDebtCounselling> DebtCounsellingCases
        {
            get;
        }
    }
}