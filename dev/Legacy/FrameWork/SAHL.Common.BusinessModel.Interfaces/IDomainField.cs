using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.DomainField_DAO
    /// </summary>
    public partial interface IDomainField : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DomainField_DAO.Description
        /// </summary>
        System.String Description
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DomainField_DAO.DisplayDescription
        /// </summary>
        System.String DisplayDescription
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DomainField_DAO.Key
        /// </summary>
        System.String Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DomainField_DAO.FormatType
        /// </summary>
        IFormatType FormatType
        {
            get;
            set;
        }
    }
}