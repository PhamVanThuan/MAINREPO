namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// This Interface is not part of the Domain.
    /// It acts a place holder to the following Intefaces.
    /// ILegalEntityCompany
    /// ILegalEntityTrust
    /// ILegalEntityCloseCorporation
    /// Implementation example can be found in the rules plugin - LegalEntityCompanyCCTrust.cs
    /// </summary>

    public interface ILegalEntityGenericCompany : ILegalEntity
    {
        /// <summary>
        ///
        /// </summary>
        System.String RegistrationNumber
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        System.String RegisteredName
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        System.String TradingName
        {
            get;
            set;
        }
    }
}