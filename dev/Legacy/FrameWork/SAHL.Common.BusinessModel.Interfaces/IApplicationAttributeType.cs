using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ApplicationAttributeType_DAO
    /// </summary>
    public partial interface IApplicationAttributeType : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationAttributeType_DAO.Description
        /// </summary>
        System.String Description
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationAttributeType_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationAttributeType_DAO.IsGeneric
        /// </summary>
        System.Boolean IsGeneric
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationAttributeType_DAO.OriginationSourceProducts
        /// </summary>
        IEventList<IOriginationSourceProduct> OriginationSourceProducts
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationAttributeType_DAO.ApplicationAttributeTypeGroup
        /// </summary>
        IApplicationAttributeTypeGroup ApplicationAttributeTypeGroup
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationAttributeType_DAO.UserEditable
        /// </summary>
        System.Boolean UserEditable
        {
            get;
            set;
        }
    }
}