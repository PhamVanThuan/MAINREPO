using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Globals;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    public partial interface IApplicationRole : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// Gets/sets the Application to which the role applies.
        /// </summary>
        IApplication Application { get; set; }

        /// <summary>
        /// Gets/sets the LegalEntity to which the role applies.
        /// </summary>
        ILegalEntity LegalEntity { get; set; }

        IApplicationRoleDomicilium ApplicationRoleDomicilium { get; set; }

        bool HasAttribute(OfferRoleAttributeTypes OfferRoleAttributeType);
    }
}