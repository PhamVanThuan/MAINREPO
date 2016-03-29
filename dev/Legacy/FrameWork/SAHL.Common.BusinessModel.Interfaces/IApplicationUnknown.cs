using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// Derived from the Application_DAO and instantiated to represent a Unknown Application.
    /// </summary>
    public partial interface IApplicationUnknown : IEntityValidation, IBusinessModelObject, IApplication
    {
    }
}