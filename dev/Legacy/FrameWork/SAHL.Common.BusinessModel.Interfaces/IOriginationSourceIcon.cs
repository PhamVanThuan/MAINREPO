using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.OriginationSourceIcon_DAO
    /// </summary>
    public partial interface IOriginationSourceIcon : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.OriginationSourceIcon_DAO.OriginationSourceKey
        /// </summary>
        System.Int32 OriginationSourceKey
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.OriginationSourceIcon_DAO.Icon
        /// </summary>
        System.String Icon
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.OriginationSourceIcon_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }
    }
}