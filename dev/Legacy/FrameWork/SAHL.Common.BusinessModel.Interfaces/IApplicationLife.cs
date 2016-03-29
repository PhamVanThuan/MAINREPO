using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// This object is used to facilitate the origination of a life insurance policy.
    /// </summary>
    public partial interface IApplicationLife : IEntityValidation, IBusinessModelObject, IApplication
    {
    }
}