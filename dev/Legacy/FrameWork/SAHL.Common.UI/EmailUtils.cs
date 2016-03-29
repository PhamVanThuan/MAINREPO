using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using System.Net.Mail;
using SAHL.Common.BusinessModel;
using System.Net;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.Collections.Interfaces;
using System.IO;
using System.Net.Mime;

namespace SAHL.Common.UI
{
    public static class EmailUtils
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="emailFrom"></param>
        /// <param name="emailTo"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public static bool SendEmailInternal(string emailFrom, string emailTo, string subject, string body)
        {
            return SendEmailInternal(emailFrom, emailTo, subject, body, true);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="emailFrom"></param>
        /// <param name="emailTo"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="isBodyHTML"></param>
        /// <returns></returns>
        public static bool SendEmailInternal(string emailFrom, string emailTo, string subject, string body, bool isBodyHTML)
        {
            return SendEmailInternal(emailFrom, emailTo, subject, body, isBodyHTML, "");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="emailFrom"></param>
        /// <param name="emailTo"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="isBodyHTML"></param>
        /// <param name="attachment"></param>
        /// <returns></returns>
        public static bool SendEmailInternal(string emailFrom, string emailTo, string subject, string body, bool isBodyHTML, string attachment)
        {
            IList<string> attachments = new List<string>(1);
            if (!String.IsNullOrEmpty(attachment))
                attachments.Add(attachment);
            return SendEmailInternal(emailFrom, emailTo, subject, body, isBodyHTML, attachments);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="emailFrom"></param>
        /// <param name="emailTo"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="isBodyHTML"></param>
        /// <param name="attachments"></param>
        /// <returns></returns>
        public static bool SendEmailInternal(string emailFrom, string emailTo, string subject, string body, bool isBodyHTML, IList<string> attachments)
        {
            bool emailSent = true;
            // make sure there is an email address
            if (String.IsNullOrEmpty(emailTo))
            {
                throw new Exception("Email cannot be sent - No email address exists");
            }


            // only allow sending to sahomeloans.com domain
            if (!emailTo.EndsWith("sahomeloans.com"))
            {
                throw new Exception("This method can only be used to send emails to 'sahomeloans.com' domain");
            }

            ILookupRepository lookupRepository = RepositoryFactory.GetRepository<ILookupRepository>();

            // get the smtp details
            string smtpServerHost = lookupRepository.Controls.ObjectDictionary[SAHL.Common.Constants.ControlTable.SMTPHostname].ControlText;
            int smtpServerPort = Convert.ToInt32(lookupRepository.Controls.ObjectDictionary[SAHL.Common.Constants.ControlTable.SMTPPort].ControlNumeric);

            // setup the email
            MailMessage email = new MailMessage();
            email.IsBodyHtml = isBodyHTML;
            email.From = new MailAddress(emailFrom, emailFrom);
            email.To.Add(emailTo);
            email.Subject = subject;
            email.Body = body;
            email.BodyEncoding = Encoding.Default;

            // handle attachements
            if (attachments != null && attachments.Count > 0)
            {
                foreach (string attachement in attachments)
                {
                    if (!String.IsNullOrEmpty(attachement))
                        email.Attachments.Add(new Attachment(attachement));
                }
            }

            // setup the smtp object
            SmtpClient smtpMail = new SmtpClient(smtpServerHost, smtpServerPort);
            //smtpMail.Credentials = new NetworkCredential();
            //smtpMail.EnableSsl = false;

            try
            {
                smtpMail.Send(email);
            }
            catch (SmtpFailedRecipientsException ex)
            {
                emailSent = false;
                throw new Exception("The internal email was not sent !" + ex.ToString());
            }

            return emailSent;
        }

        /// <summary>
        /// Inserts and entry into the Client Email table which handles the sending of emails to clients
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
        /// <param name="sendFax"></param>
        public static bool SendEmailExternal(int genericKey, string from, string to, string cc, string bcc, string subject, string body, string cellphone, string attachment1, string attachment2, string attachment3, bool sendFax)
        {
            bool emailSent = true;

            try
            {
                // get a new client email object
                IClientEmail clientEmail = new ClientEmail(new ClientEmail_DAO());

                // populate the client email object
                clientEmail.LoanNumber = genericKey;
                clientEmail.EmailFrom = from;

                if (sendFax)
                    clientEmail.EmailTo = formatFaxNumber(to);
                else
                    clientEmail.EmailTo = to;

                clientEmail.EmailCC = cc;
                clientEmail.EmailBCC = bcc;
                clientEmail.EmailSubject = subject;
                clientEmail.EmailBody = body;
                clientEmail.EmailAttachment1 = attachment1;
                clientEmail.EmailAttachment2 = attachment2;
                clientEmail.EmailAttachment3 = attachment3;
                clientEmail.Cellphone = cellphone;
                clientEmail.EMailInsertDate = System.DateTime.Now;


                // save the client email object
                IDAOObject IDAO = clientEmail as IDAOObject;
                ClientEmail_DAO dao = (ClientEmail_DAO)IDAO.GetDAOObject();
                dao.SaveAndFlush();
            }
            catch (Exception ex)
            {
                emailSent = false;
                throw new Exception("The client email was not sent !" + ex.ToString());
            }

            return emailSent;
        }

        private static string formatFaxNumber(string faxNumber)
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
