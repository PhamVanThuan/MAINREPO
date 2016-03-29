using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// Derived from the Employment_DAO and is used to represent an Employment type of Salaried.
    /// </summary>
    public partial interface IEmploymentSalaried : IEntityValidation, IBusinessModelObject, IEmployment
    {
    }
}