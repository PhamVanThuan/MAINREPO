using System.Collections.Generic;
using SAHL.Common.Globals;

namespace SAHL.Common.Service.Interfaces
{
    /// <summary>
    /// Represents a message service that can send Email/Fax or SMSs messages
    /// </summary>
    public interface IMessageService
    {
        /// <summary>
        /// Sends Emails to Internal SAHL Staff using SMTP Email service
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="cc"></param>
        /// <param name="bcc"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        bool SendEmailInternal(string from, string to, string cc, string bcc, string subject, string body);

        bool SendEmailInternal(string from, string to, string cc, string bcc, string subject, string body, bool isBodyHTML);

        bool SendEmailInternal(string from, string to, string cc, string bcc, string subject, string body, bool isBodyHTML, string attachment);

        bool SendEmailInternal(string from, string to, string cc, string bcc, string subject, string body, bool isBodyHTML, IList<string> attachments);

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
        bool SendEmailExternal(int genericKey, string from, string to, string cc, string bcc, string subject, string body, params string[] attachments);

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
        bool SendEmailExternal(int genericKey, string from, string to, string cc, string bcc, string subject, string body, string attachment1, string attachment2, string attachment3, ContentTypes contentType);

        /// <summary>
        /// Sends Faxes to External Clients using ClientEmail table
        /// </summary>
        /// <param name="genericKey"></param>
        /// <param name="from"></param>
        /// <param name="faxNumber"></param>
        /// <param name="faxAttachment"></param>
        bool SendFax(int genericKey, string from, string faxNumber, string faxAttachment);

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
        bool SendFaxMultipleDocs(int genericKey, string from, string to, string cc, string bcc, string subject, string body, params string[] attachments);

        /// <summary>
        /// Sends SMS's to External Clients using ClientEmail table
        /// </summary>
        /// <param name="genericKey"></param>
        /// <param name="message"></param>
        /// <param name="cellphoneNumber"></param>
        /// <returns></returns>
        bool SendSMS(int genericKey, string message, string cellphoneNumber);
    }
}