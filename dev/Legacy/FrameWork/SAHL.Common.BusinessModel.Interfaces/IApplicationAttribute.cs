using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// Application attributes.
    /// </summary>
    public partial interface IApplicationAttribute : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationAttribute_DAO.Application
        /// </summary>
        IApplication Application
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationAttribute_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationAttribute_DAO.ApplicationAttributeType
        /// </summary>
        IApplicationAttributeType ApplicationAttributeType
        {
            get;
            set;
        }
    }
}