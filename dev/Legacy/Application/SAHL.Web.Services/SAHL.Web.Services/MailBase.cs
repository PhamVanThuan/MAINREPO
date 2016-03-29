using System.Collections.Generic;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;


namespace SAHL.Web.Services
{
    public class MailBase
    {

        private static IMessageService _msgService;
        private static IMessageService MessageService
        {
            get { return _msgService ?? (_msgService = ServiceFactory.GetService<IMessageService>()); }
        }



        /// <summary>
        /// Sends Emails to Internal SAHL Staff using SMTP Email service
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="cc"></param>
        /// <param name="bcc"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        public static bool SendEmailInternal(string from, string to, string cc, string bcc, string subject, string body)
        {
            return MessageService.SendEmailInternal(from, to, cc, bcc, subject, body);
        }

        /// <summary>
        /// Sends Emails to Internal SAHL Staff using SMTP Email service with an html body
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="cc"></param>
        /// <param name="bcc"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="isBodyHtml"></param>
        /// <returns></returns>
        public static bool SendEmailInternal(string from, string to, string cc, string bcc, string subject, string body, bool isBodyHtml)
        {

            return MessageService.SendEmailInternal(from, to, cc, bcc, subject, body, isBodyHtml);
        }


        /// <summary>
        /// Sends Emails to Internal SAHL Staff using SMTP Email service with  an attachment
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="cc"></param>
        /// <param name="bcc"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="isBodyHtml"></param>
        /// <param name="attachment"></param>
        /// <returns></returns>
        public static bool SendEmailInternal(string from, string to, string cc, string bcc, string subject, string body, bool isBodyHtml, string attachment)
        {

            return MessageService.SendEmailInternal(from, to, cc, bcc, subject, body, isBodyHtml, attachment);
            
        }

        /// <summary>
        /// Sends Emails to Internal SAHL Staff using SMTP Email service with  multiple attachments
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="cc"></param>
        /// <param name="bcc"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="isBodyHtml"></param>
        /// <param name="attachments"></param>
        /// <returns></returns>
        public static bool SendEmailInternal(string from, string to, string cc, string bcc, string subject, string body, bool isBodyHtml, IList<string> attachments)
        {

            return MessageService.SendEmailInternal(from, to, cc, bcc, subject, body, isBodyHtml, attachments);
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
        public static bool SendEmailExternal(int genericKey, string from, string to, string cc, string bcc, string subject, string body, string attachment1, string attachment2, string attachment3)
        {

            return MessageService.SendEmailExternal(genericKey, from, to, cc, bcc, subject, body, attachment1, attachment2, attachment3);

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
        public static bool SendEmailExternal(int genericKey, string from, string to, string cc, string bcc, string subject, string body, string attachment1, string attachment2, string attachment3, int contentType)
        {
            ContentTypes ContentType = contentType == 1 ? ContentTypes.HTML : ContentTypes.StandardText;
            return MessageService.SendEmailExternal(genericKey, from, to, cc, bcc, subject, body, attachment1, attachment2, attachment3, ContentType);
        }

        /// <summary>
        /// Sends Faxes to External Clients using ClientEmail table
        /// </summary>
        /// <param name="genericKey"></param>
        /// <param name="from"></param>
        /// <param name="faxNumber"></param>
        /// <param name="faxAttachment"></param>
        public static bool SendFax(int genericKey, string from, string faxNumber, string faxAttachment)
        {

            return MessageService.SendFax(genericKey, from, faxNumber, faxAttachment);

        }

        /// <summary>
        /// Sends Faxes with Multiple Attachments to External Clients using ClientEmail table
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
        /// <returns></returns>
        public static bool SendFaxMultipleDocs(int genericKey, string from, string to, string cc, string bcc, string subject, string body, string attachment1, string attachment2, string attachment3)
        {

            return MessageService.SendFaxMultipleDocs(genericKey, from, to, cc, bcc, subject, body, attachment1, attachment2, attachment3);

        }

        /// <summary>
        /// Sends SMS's to External Clients using ClientEmail table
        /// </summary>
        /// <param name="genericKey"></param>
        /// <param name="message"></param>
        /// <param name="cellphoneNumber"></param>
        /// <returns></returns>
        public static bool SendSMS(int genericKey, string message, string cellphoneNumber)
        {

            return MessageService.SendSMS(genericKey, message, cellphoneNumber);

        }
        
    }
}
