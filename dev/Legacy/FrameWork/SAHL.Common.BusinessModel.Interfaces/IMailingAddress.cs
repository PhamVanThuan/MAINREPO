using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.MailingAddress_DAO
    /// </summary>
    public partial interface IMailingAddress : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// The Electronic Format they would like to receive their Loan Statement in.
        /// </summary>
        IOnlineStatementFormat OnlineStatementFormat
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.MailingAddress_DAO.OnlineStatement
        /// </summary>
        System.Boolean OnlineStatement
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.MailingAddress_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.MailingAddress_DAO.Account
        /// </summary>
        IAccount Account
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.MailingAddress_DAO.Address
        /// </summary>
        IAddress Address
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.MailingAddress_DAO.Language
        /// </summary>
        ILanguage Language
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.MailingAddress_DAO.LegalEntity
        /// </summary>
        ILegalEntity LegalEntity
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.MailingAddress_DAO.CorrespondenceMedium
        /// </summary>
        ICorrespondenceMedium CorrespondenceMedium
        {
            get;
            set;
        }
    }
}