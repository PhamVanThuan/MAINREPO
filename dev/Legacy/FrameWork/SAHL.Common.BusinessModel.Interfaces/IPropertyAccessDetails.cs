using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.PropertyAccessDetails_DAO
    /// </summary>
    public partial interface IPropertyAccessDetails : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.PropertyAccessDetails_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.PropertyAccessDetails_DAO.Property
        /// </summary>
        IProperty Property
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.PropertyAccessDetails_DAO.Contact1
        /// </summary>
        System.String Contact1
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.PropertyAccessDetails_DAO.Contact1Phone
        /// </summary>
        System.String Contact1Phone
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.PropertyAccessDetails_DAO.Contact1WorkPhone
        /// </summary>
        System.String Contact1WorkPhone
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.PropertyAccessDetails_DAO.Contact1MobilePhone
        /// </summary>
        System.String Contact1MobilePhone
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.PropertyAccessDetails_DAO.Contact2
        /// </summary>
        System.String Contact2
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.PropertyAccessDetails_DAO.Contact2Phone
        /// </summary>
        System.String Contact2Phone
        {
            get;
            set;
        }
    }
}