using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ApplicationMailingAddress_DAO
    /// </summary>
    public partial interface IApplicationMailingAddress : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// The Mailing Address is associated to a particular application. This relationship is defined in the OfferMailingAddress
        /// table where the Offer.OfferKey = OfferMailingAddress.OfferKey.
        /// </summary>
        IApplication Application
        {
            get;
            set;
        }

        /// <summary>
        /// Each Address is associated an AddressKey. The Address details are retrieved from the Address table based on this key.
        /// </summary>
        IAddress Address
        {
            get;
            set;
        }

        /// <summary>
        /// An indicator as to whether the client would like to receive their Loan Statement electronically.
        /// </summary>
        System.Boolean OnlineStatement
        {
            get;
            set;
        }

        /// <summary>
        /// The Electronic Format they would like to receive their Loan Statement in.
        /// </summary>
        IOnlineStatementFormat OnlineStatementFormat
        {
            get;
            set;
        }

        /// <summary>
        /// This determines the Language preference correspondence sent to the client.
        /// </summary>
        ILanguage Language
        {
            get;
            set;
        }

        /// <summary>
        /// Primary Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationMailingAddress_DAO.LegalEntity
        /// </summary>
        ILegalEntity LegalEntity
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationMailingAddress_DAO.CorrespondenceMedium
        /// </summary>
        ICorrespondenceMedium CorrespondenceMedium
        {
            get;
            set;
        }
    }
}