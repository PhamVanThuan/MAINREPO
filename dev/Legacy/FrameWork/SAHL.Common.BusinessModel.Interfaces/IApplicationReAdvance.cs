using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// Derived from the Application_DAO and instantiated to represent a ReAdvance Application.
    /// This object is used to facilitate the origination of a re advance on a mortgage loan account.
    /// </summary>
    public partial interface IApplicationReAdvance : IEntityValidation, IBusinessModelObject, IApplication
    {
    }
}