using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ClientEmail_DAO
    /// </summary>
    public partial interface IClientEmail : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ClientEmail_DAO.EmailTo
        /// </summary>
        System.String EmailTo
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ClientEmail_DAO.EmailCC
        /// </summary>
        System.String EmailCC
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ClientEmail_DAO.EmailBCC
        /// </summary>
        System.String EmailBCC
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ClientEmail_DAO.EmailSubject
        /// </summary>
        System.String EmailSubject
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ClientEmail_DAO.EmailBody
        /// </summary>
        System.String EmailBody
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ClientEmail_DAO.EmailAttachment1
        /// </summary>
        System.String EmailAttachment1
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ClientEmail_DAO.EmailAttachment2
        /// </summary>
        System.String EmailAttachment2
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ClientEmail_DAO.EmailAttachment3
        /// </summary>
        System.String EmailAttachment3
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ClientEmail_DAO.Cellphone
        /// </summary>
        System.String Cellphone
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ClientEmail_DAO.LoanNumber
        /// </summary>
        System.Decimal LoanNumber
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ClientEmail_DAO.EmailFrom
        /// </summary>
        System.String EmailFrom
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ClientEmail_DAO.EMailInsertDate
        /// </summary>
        System.DateTime EMailInsertDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ClientEmail_DAO.ContentTypeKey
        /// </summary>
        System.Int32 ContentTypeKey
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ClientEmail_DAO.Key
        /// </summary>
        System.Decimal Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AdditionalAttachmentsDelimitered.Key
        /// </summary>
        System.String AdditionalAttachmentsDelimitered
        {
            get;
            set;
        }
    }
}