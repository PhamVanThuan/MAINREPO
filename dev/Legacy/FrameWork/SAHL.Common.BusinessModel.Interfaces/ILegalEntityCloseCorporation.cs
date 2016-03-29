using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// LegalEntityCloseCorporation_DAO is derived from LegalEntity_DAO and is used to instantiate a Legal Entity of type Close
    /// Corporation.
    /// </summary>
    public partial interface ILegalEntityCloseCorporation : IEntityValidation, IBusinessModelObject, ILegalEntity
    {
        ///// <summary>
        ///// The name under which the Close Corporation trades, this is not always the same as the Registered Name.
        ///// </summary>
        //System.String TradingName
        //{
        //    get;
        //    set;
        //}
    }
}