using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// LegalEntityUnknown_DAO is derived from the LegalEntity_DAO class. It is used to instantiate a Legal Entity of type "Unknown".
    /// </summary>
    public partial interface ILegalEntityUnknown : IEntityValidation, IBusinessModelObject, ILegalEntity
    {
    }
}