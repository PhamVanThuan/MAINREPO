using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// This is derived from Valuation_DAO. When instantiated it represents an RCS Physical Valuation.
    /// </summary>
    public partial interface IValuationDiscriminatedRCSPhysical : IEntityValidation, IBusinessModelObject, IValuation
    {
    }
}