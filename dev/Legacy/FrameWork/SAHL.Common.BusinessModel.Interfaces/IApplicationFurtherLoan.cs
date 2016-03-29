using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// Derived from the Application_DAO. Instantiated to represent a Further Loan Application. This object is used to facilitate
    /// the origination of a Further Loan on a Mortgage Loan Account.
    /// </summary>
    public partial interface IApplicationFurtherLoan : IEntityValidation, IBusinessModelObject, IApplication
    {
    }
}