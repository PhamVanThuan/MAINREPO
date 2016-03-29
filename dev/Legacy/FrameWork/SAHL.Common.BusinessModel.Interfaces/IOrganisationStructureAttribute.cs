using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.OrganisationStructureAttribute_DAO
    /// </summary>
    public partial interface IOrganisationStructureAttribute : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.OrganisationStructureAttribute_DAO.AttributeValue
        /// </summary>
        System.String AttributeValue
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.OrganisationStructureAttribute_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.OrganisationStructureAttribute_DAO.OrganisationStructure
        /// </summary>
        IOrganisationStructure OrganisationStructure
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.OrganisationStructureAttribute_DAO.OrganisationStructureAttributeType
        /// </summary>
        IOrganisationStructureAttributeType OrganisationStructureAttributeType
        {
            get;
            set;
        }
    }
}