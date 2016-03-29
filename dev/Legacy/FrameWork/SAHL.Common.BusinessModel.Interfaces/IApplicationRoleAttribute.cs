using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ApplicationRoleAttribute_DAO
    /// </summary>
    public partial interface IApplicationRoleAttribute : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationRoleAttribute_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationRoleAttribute_DAO.OfferRole
        /// </summary>
        IApplicationRole OfferRole
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationRoleAttribute_DAO.OfferRoleAttributeType
        /// </summary>
        IApplicationRoleAttributeType OfferRoleAttributeType
        {
            get;
            set;
        }
    }
}