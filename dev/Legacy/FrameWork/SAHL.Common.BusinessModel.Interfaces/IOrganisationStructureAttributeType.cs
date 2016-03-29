using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.OrganisationStructureAttributeType_DAO
    /// </summary>
    public partial interface IOrganisationStructureAttributeType : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.OrganisationStructureAttributeType_DAO.Description
        /// </summary>
        System.String Description
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.OrganisationStructureAttributeType_DAO.Length
        /// </summary>
        System.Int32 Length
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.OrganisationStructureAttributeType_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.OrganisationStructureAttributeType_DAO.OrganisationStructureAttributes
        /// </summary>
        IEventList<IOrganisationStructureAttribute> OrganisationStructureAttributes
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.OrganisationStructureAttributeType_DAO.DataType
        /// </summary>
        IDataType DataType
        {
            get;
            set;
        }
    }
}