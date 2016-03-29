using SAHL.Common.Attributes;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Logging;
using SAHL.Common.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Security.Principal;
using System.Text;

namespace SAHL.Common.Service
{
    /// <summary>
    /// Concrete implementation of an <see href="IMessageService"/>
    /// </summary>
    [FactoryType(typeof(IMessageService))]
    public class MessagingService : IMessageService
    {
        private enum CorrespondenceType { Email, Fax, SMS }

        #region IMessageService Members

        public bool SendEmailInternal(string from, string to, string cc, string bcc, string subject, string body)
        {
            return SendEmailInternal(from, to, cc, bcc, subject, body, true);
        }

        public bool SendEmailInternal(string from, string to, string cc, string bcc, string subject, string body, bool isBodyHTML)
        {
            return SendEmailInternal(from, to, cc, bcc, subject, body, isBodyHTML, "");
        }

        public bool SendEmailInternal(string from, string to, string cc, string bcc, string subject, string body, bool isBodyHTML, string attachment)
        {
            IList<string> attachments = new List<string>(1);
            if (!String.IsNullOrEmpty(attachment))
                attachments.Add(attachment);
            return SendEmailInternal(from, to, cc, bcc, subject, body, isBodyHTML, attachments);
        }

        public bool SendEmailInternal(string from, string to, string cc, string bcc, string subject, string body, bool isBodyHTML, IList<string> attachments)
        {
            // make sure there is an email address
            if (String.IsNullOrEmpty(to))
                throw new SmtpFailedRecipientsException("Email cannot be sent - No email address exists");

            // only allow sending to sahomeloans.com domain
            if (!to.EndsWith("sahomeloans.com") && !to.EndsWith("sahomeloans.co.za"))
                throw new SmtpFailedRecipientsException("This method can only be used to send emails to 'sahomeloans' domain");

            try
            {
                ILookupRepository lookupRepository = RepositoryFactory.GetRepository<ILookupRepository>();

                // get the smtp details
                string smtpServerHost = lookupRepository.Controls.ObjectDictionary[SAHL.Common.Constants.ControlTable.SMTPHostname].ControlText;
                int smtpServerPort = Convert.ToInt32(lookupRepository.Controls.ObjectDictionary[SAHL.Common.Constants.ControlTable.SMTPPort].ControlNumeric);

                string serviceUser = lookupRepository.Controls.ObjectDictionary[SAHL.Common.Constants.ControlTable.ServiceUser].ControlText;
                string servicePassword = lookupRepository.Controls.ObjectDictionary[SAHL.Common.Constants.ControlTable.ServicePassword].ControlText;
                string serviceDomain = lookupRepository.Controls.ObjectDictionary[SAHL.Common.Constants.ControlTable.ServiceDomain].ControlText;

                // setup the email
                MailMessage email = new MailMessage();
                email.IsBodyHtml = isBodyHTML;
                if (String.IsNullOrEmpty(from))
                    from = "sqlmail@sahomeloans.com";
                email.From = new MailAddress(from, from);

                // if there is header text, prepend it to the body
                StringBuilder sbBody = new StringBuilder();

                // Email will be sent if :-
                // 1. There is no 'InternalTestEmailRecipients' setting or the 'InternalTestEmailRecipients' list in the SAHL.Common.Service.config file is empty
                // 2. There is an 'InternalTestEmailRecipients' setting and the 'InternalTestEmailRecipients' is not empty and the email address to which we are sending is contained in the list
                // 3. The email body does not contain any of the phrases contained in the 'InternalTestEmailFilterPhrases' list in the SAHL.Common.Service.config file
                if (CheckInternalEmailFilter(ref to, ref cc, ref bcc, body) == false)
                    return false;

                sbBody.Append(body);

                email.To.Add(to);
                if (!String.IsNullOrEmpty(cc))
                    email.CC.Add(cc);
                if (!String.IsNullOrEmpty(bcc))
                    email.Bcc.Add(bcc);
                email.Subject = subject;

                email.Body = sbBody.ToString();
                email.BodyEncoding = Encoding.Default;

                // start impersonation
                ISecurityService securityService = ServiceFactory.GetService<ISecurityService>();
                WindowsImpersonationContext wic = securityService.BeginImpersonation();

                // handle attachements
                try
                {
                    if (attachments != null && attachments.Count > 0)
                    {
                        foreach (string attachement in attachments)
                        {
                            if (!String.IsNullOrEmpty(attachement))
                                email.Attachments.Add(new Attachment(attachement));
                        }
                    }
                }
                catch
                {
                    throw;
                }
                finally
                {
                    // end impersonation
                    securityService.EndImpersonation(wic);
                }

                // setup the smtp object
                SmtpClient smtpMail = new SmtpClient(smtpServerHost, smtpServerPort);
                smtpMail.Credentials = new NetworkCredential(serviceUser, servicePassword, serviceDomain);

                smtpMail.Send(email);
            }
            catch (SmtpException smtpex)
            {
                //swallow, cause we dont use a q
                try
                {
                    var methodBase = MethodBase.GetCurrentMethod();
                    LogPlugin.Logger.LogErrorMessageWithException(methodBase.Name, smtpex.Message, smtpex);
                }
                catch (Exception)
                {
                    //if there is an error logging this, swallow...
                }
            }
            return true;
        }

        private bool CheckInternalEmailFilter(ref string to, ref string cc, ref string bcc, string body)
        {
            bool sendEmail = false;

            // if we have no recipients in the 'InternalTestEmailRecipients' list then return true
            if (Properties.Settings.Default.InternalTestEmailRecipients == null || Properties.Settings.Default.InternalTestEmailRecipients.Count <= 0)
                return true;

            // check that the recipient(s) are in the 'InternalTestEmailRecipients' list in the SAHL.Common.Service.config file
            // if any of the recipients are not in the list then filter them out

            // filter the 'to' recipients
            to = BuildFilteredRecipientsList(to);

            if (!String.IsNullOrEmpty(to))
            {
                sendEmail = true;

                // filter the 'cc' recipients
                if (!String.IsNullOrEmpty(cc))
                    cc = BuildFilteredRecipientsList(cc);
                // filter the 'bcc' recipients
                if (!String.IsNullOrEmpty(bcc))
                    bcc = BuildFilteredRecipientsList(bcc);
            }

            if (sendEmail
                && Properties.Settings.Default.InternalTestEmailFilterPhrases != null
                && Properties.Settings.Default.InternalTestEmailFilterPhrases.Count > 0)
            {
                // check that the email body does not contain any of the phrases contained
                // in the 'InternalTestEmailFilterPhrases' list in the SAHL.Common.Service.config file
                foreach (string filterPhrase in Properties.Settings.Default.InternalTestEmailFilterPhrases)
                {
                    if (body.Contains(filterPhrase))
                    {
                        sendEmail = false;
                        break;
                    }
                }
            }

            return sendEmail;
        }

        private string BuildFilteredRecipientsList(string recipients)
        {
            string newRecipientsList = "";
            string[] recipientsList = recipients.Split(';');
            int recipientCount = 0;
            foreach (string recipient in recipientsList)
            {
                foreach (string allowedRecipient in Properties.Settings.Default.InternalTestEmailRecipients)
                {
                    if (recipient.ToLower() == allowedRecipient.ToLower())
                    {
                        recipientCount++;
                        if (recipientCount == 1)
                            newRecipientsList = recipient;
                        else
                            newRecipientsList += ";" + recipient;

                        break;
                    }
                }
            }

            return newRecipientsList;
        }

        /// <summary>
        /// Sends Emails to External Clients using ClientEmail table
        /// </summary>
        /// <param name="genericKey"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="cc"></param>
        /// <param name="bcc"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="attachments"></param>
        public bool SendEmailExternal(int genericKey, string from, string to, string cc, string bcc, string subject, string body, params string[] attachments)
        {
            return SendExternalCorrespondence(genericKey, from, to, cc, bcc, subject, body, null, new List<string>(attachments), CorrespondenceType.Email, ContentTypes.StandardText);
        }

        /// <summary>
        /// Sends Emails to External Clients using ClientEmail table
        /// </summary>
        /// <param name="genericKey"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="cc"></param>
        /// <param name="bcc"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="attachment1"></param>
        /// <param name="attachment2"></param>
        /// <param name="attachment3"></param>
        /// <param name="contentType"></param>
        public bool SendEmailExternal(int genericKey, string from, string to, string cc, string bcc, string subject, string body, string attachment1, string attachment2, string attachment3, ContentTypes contentType)
        {
            return SendExternalCorrespondence(genericKey, from, to, cc, bcc, subject, body, null, new List<string> { attachment1, attachment2, attachment3 }, CorrespondenceType.Email, contentType);
        }

        /// <summary>
        /// Sends Faxes to External Clients using ClientEmail table
        /// </summary>
        /// <param name="genericKey"></param>
        /// <param name="from"></param>
        /// <param name="faxNumber"></param>
        /// <param name="faxAttachment"></param>
        public bool SendFax(int genericKey, string from, string faxNumber, string faxAttachment)
        {
            return SendExternalCorrespondence(genericKey, from, faxNumber, null, null, null, null, null, new List<string> { faxAttachment }, CorrespondenceType.Fax, ContentTypes.StandardText);
        }

        /// <summary>
        /// Sends Faxes with Multiple Attachments to External Clients using ClientEmail table
        /// </summary>
        /// <param name="genericKey"></param>
        /// <param name="from"></param>
        /// <param name="faxNumber"></param>
        /// <param name="cc"></param>
        /// <param name="bcc"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="attachment1"></param>
        /// <param name="attachment2"></param>
        /// <param name="attachment3"></param>
        /// <returns></returns>
        public bool SendFaxMultipleDocs(int genericKey, string from, string faxNumber, string cc, string bcc, string subject, string body, params string[] attachments)
        {
            return SendExternalCorrespondence(genericKey, from, faxNumber, null, null, null, null, null, new List<string>(attachments), CorrespondenceType.Fax, ContentTypes.StandardText);
        }

        /// <summary>
        /// Sends SMS's to External Clients using ClientEmail table
        /// </summary>
        /// <param name="genericKey"></param>
        /// <param name="message"></param>
        /// <param name="cellphoneNumber"></param>
        public bool SendSMS(int genericKey, string message, string cellphoneNumber)
        {
            return SendExternalCorrespondence(genericKey, null, null, null, null, null, message, cellphoneNumber, new List<string> { }, CorrespondenceType.SMS, ContentTypes.StandardText);
        }

        #endregion IMessageService Members

        /// <summary>
        /// Handles the sending of Email/Fax or SMS vis the ClientEmail table
        /// </summary>
        /// <param name="genericKey"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="cc"></param>
        /// <param name="bcc"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="cellphone"></param>
        /// <param name="attachment1"></param>
        /// <param name="attachment2"></param>
        /// <param name="attachment3"></param>
        /// <param name="correspondenceType"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        private bool SendExternalCorrespondence(int genericKey, string from, string to, string cc, string bcc, string subject, string body, string cellphone, List<string> attachments, CorrespondenceType correspondenceType, ContentTypes contentType)
        {
            try
            {
                // get a new client email object
                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                IClientEmail clientEmail = BMTM.GetMappedType<IClientEmail>(new ClientEmail_DAO());

                // populate the client email object
                clientEmail.LoanNumber = genericKey;

                switch (correspondenceType)
                {
                    case CorrespondenceType.Email:
                    case CorrespondenceType.Fax:
                        if (String.IsNullOrEmpty(from))
                            clientEmail.EmailFrom = "sqlmail@sahomeloans.com";
                        else
                            clientEmail.EmailFrom = from;

                        if (correspondenceType == CorrespondenceType.Fax)
                            clientEmail.EmailTo = formatFaxNumber(to);
                        else
                            clientEmail.EmailTo = to;

                        clientEmail.EmailCC = cc;
                        clientEmail.EmailBCC = bcc;
                        clientEmail.EmailSubject = subject;
                        clientEmail.EmailBody = body;

                        clientEmail.EmailAttachment1 = attachments.Count > 0 ? attachments[0] : null;
                        clientEmail.EmailAttachment2 = attachments.Count > 1 ? attachments[1] : null;
                        clientEmail.EmailAttachment3 = attachments.Count > 2 ? attachments[2] : null;

                        if (attachments.Count > 3)
                        {
                            var additionalAttachments = attachments.GetRange(3, attachments.Count - 3);
                            clientEmail.AdditionalAttachmentsDelimitered = string.Join(";", additionalAttachments);
                        }

                        clientEmail.ContentTypeKey = (int)contentType;

                        break;

                    case CorrespondenceType.SMS:
                        clientEmail.Cellphone = cellphone;
                        clientEmail.EmailAttachment1 = "SMS";
                        clientEmail.EmailBody = body;
                        break;

                    default:
                        break;
                }

                clientEmail.EMailInsertDate = System.DateTime.Now;

                // save the client email object
                IDAOObject IDAO = clientEmail as IDAOObject;
                ClientEmail_DAO dao = (ClientEmail_DAO)IDAO.GetDAOObject();
                dao.SaveAndFlush();
            }
            catch (Exception ex)
            {
                throw new Exception("The client " + correspondenceType.ToString() + "  was not sent : " + ex.ToString());
            }

            return true;
        }

        private string formatFaxNumber(string faxNumber)
        {
            if (String.IsNullOrEmpty(faxNumber))
                return "";

            ILookupRepository lookupRepository = RepositoryFactory.GetRepository<ILookupRepository>();

            //check to see if it is already formatted
            string vaxExtension = lookupRepository.Controls.ObjectDictionary[SAHL.Common.Constants.ControlTable.FaxEmailDomain].ControlText;
            if (faxNumber.EndsWith(vaxExtension))
                return faxNumber;
            else
                return "27" + faxNumber.TrimStart('0') + vaxExtension;
        }
    }
}