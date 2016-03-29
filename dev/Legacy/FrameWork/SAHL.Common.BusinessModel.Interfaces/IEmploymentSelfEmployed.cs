using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// Derived from Employment_DAO and is used to represent an Employment type of Self Employed.
    /// </summary>
    public partial interface IEmploymentSelfEmployed : IEntityValidation, IBusinessModelObject, IEmployment
    {
    }
}