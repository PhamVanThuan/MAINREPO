using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ApplicationRoleType_DAO
    /// </summary>
    public partial interface IApplicationRoleType : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// The description of the Application Role Type
        /// </summary>
        System.String Description
        {
            get;
            set;
        }

        /// <summary>
        /// Primary Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// This is the relationship between the OrganisationStructure and the ApplicationRoleType as defined in the
        /// OfferRoleTypeOrganisationStructureMapping. An ApplicationRoleType can have many OrganisationStructures and thus an
        /// OrganisationStructure can be related to many ApplicationRoleTypes.
        /// </summary>
        IEventList<IOrganisationStructure> OfferRoleTypeOrganisationStructures
        {
            get;
        }

        /// <summary>
        /// Determines the Application Role Type Group to which the Application Role Type belongs.
        /// </summary>
        IApplicationRoleTypeGroup ApplicationRoleTypeGroup
        {
            get;
            set;
        }
    }
}