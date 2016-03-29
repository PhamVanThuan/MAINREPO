using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// LegalEntityTrust_DAO is derived from the LegalEntity_DAO class. It is used to instantiate a Legal Entity of type "Trust".
    /// </summary>
    public partial interface ILegalEntityTrust : IEntityValidation, IBusinessModelObject, ILegalEntity
    {
        ///// <summary>
        ///// The Trading Name of the Trust.
        ///// </summary>
        //System.String TradingName
        //{
        //    get;
        //    set;
        //}
    }
}