using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// LegalEntityCompany_DAO is derived from the LegalEntity_DAO class and is used to instantiate a Legal Entity of type Company.
    /// </summary>
    public partial interface ILegalEntityCompany : IEntityValidation, IBusinessModelObject, ILegalEntity
    {
        ///// <summary>
        ///// The name under which the Company trades, this is not always the same as the Registered Name.
        ///// </summary>
        //System.String TradingName
        //{
        //    get;
        //    set;
        //}
    }
}