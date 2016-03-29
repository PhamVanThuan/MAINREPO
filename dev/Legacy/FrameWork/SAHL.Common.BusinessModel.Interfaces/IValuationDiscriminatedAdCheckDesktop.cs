using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// This is derived from Valuation_DAO. When instantiated it represents a AdCheck Desktop Valuation.
    /// </summary>
    public partial interface IValuationDiscriminatedAdCheckDesktop : IEntityValidation, IBusinessModelObject, IValuation
    {
    }
}