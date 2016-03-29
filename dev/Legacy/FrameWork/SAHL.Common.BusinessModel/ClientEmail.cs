using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ClientEmail_DAO
    /// </summary>
    public partial class ClientEmail : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ClientEmail_DAO>, IClientEmail
    {
        public ClientEmail(SAHL.Common.BusinessModel.DAO.ClientEmail_DAO ClientEmail)
            : base(ClientEmail)
        {
            this._DAO = ClientEmail;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ClientEmail_DAO.EmailTo
        /// </summary>
        public String EmailTo
        {
            get { return _DAO.EmailTo; }
            set { _DAO.EmailTo = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ClientEmail_DAO.EmailCC
        /// </summary>
        public String EmailCC
        {
            get { return _DAO.EmailCC; }
            set { _DAO.EmailCC = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ClientEmail_DAO.EmailBCC
        /// </summary>
        public String EmailBCC
        {
            get { return _DAO.EmailBCC; }
            set { _DAO.EmailBCC = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ClientEmail_DAO.EmailSubject
        /// </summary>
        public String EmailSubject
        {
            get { return _DAO.EmailSubject; }
            set { _DAO.EmailSubject = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ClientEmail_DAO.EmailBody
        /// </summary>
        public String EmailBody
        {
            get { return _DAO.EmailBody; }
            set { _DAO.EmailBody = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ClientEmail_DAO.EmailAttachment1
        /// </summary>
        public String EmailAttachment1
        {
            get { return _DAO.EmailAttachment1; }
            set { _DAO.EmailAttachment1 = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ClientEmail_DAO.EmailAttachment2
        /// </summary>
        public String EmailAttachment2
        {
            get { return _DAO.EmailAttachment2; }
            set { _DAO.EmailAttachment2 = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ClientEmail_DAO.EmailAttachment3
        /// </summary>
        public String EmailAttachment3
        {
            get { return _DAO.EmailAttachment3; }
            set { _DAO.EmailAttachment3 = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ClientEmail_DAO.Cellphone
        /// </summary>
        public String Cellphone
        {
            get { return _DAO.Cellphone; }
            set { _DAO.Cellphone = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ClientEmail_DAO.LoanNumber
        /// </summary>
        public Decimal LoanNumber
        {
            get { return _DAO.LoanNumber; }
            set { _DAO.LoanNumber = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ClientEmail_DAO.EmailFrom
        /// </summary>
        public String EmailFrom
        {
            get { return _DAO.EmailFrom; }
            set { _DAO.EmailFrom = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ClientEmail_DAO.EMailInsertDate
        /// </summary>
        public DateTime EMailInsertDate
        {
            get { return _DAO.EMailInsertDate; }
            set { _DAO.EMailInsertDate = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ClientEmail_DAO.ContentTypeKey
        /// </summary>
        public Int32 ContentTypeKey
        {
            get { return _DAO.ContentTypeKey; }
            set { _DAO.ContentTypeKey = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ClientEmail_DAO.Key
        /// </summary>
        public Decimal Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ClientEmail_DAO.AdditionalAttachmentsDelimited
        /// </summary>
        public String AdditionalAttachmentsDelimitered
        {
            get { return _DAO.AdditionalAttachmentsDelimited; }
            set { _DAO.AdditionalAttachmentsDelimited = value; }
        }
    }
}