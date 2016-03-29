using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.LifeCommissionSecurity_DAO
    /// </summary>
    public partial interface ILifeCommissionSecurity : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LifeCommissionSecurity_DAO.UserID
        /// </summary>
        System.String UserID
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LifeCommissionSecurity_DAO.Administrator
        /// </summary>
        System.Boolean Administrator
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.LifeCommissionSecurity_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }
    }
}