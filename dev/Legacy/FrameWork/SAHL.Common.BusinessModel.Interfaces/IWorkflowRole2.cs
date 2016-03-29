using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    public partial interface IWorkflowRole : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// Gets/sets the LegalEntity to which the role applies.
        /// </summary>
        ILegalEntity LegalEntity { get; set; }
    }
}