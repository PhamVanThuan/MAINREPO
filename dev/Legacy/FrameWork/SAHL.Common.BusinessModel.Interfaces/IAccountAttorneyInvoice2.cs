using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.AccountAttorneyInvoice_DAO
    /// </summary>
    public partial interface IAccountAttorneyInvoice : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// Returns the Registered Name for the LegalEntity on the Attorney.
        /// </summary>
        string AttorneyRegisteredName { get; }
    }
}