using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Services;

namespace SAHL.Web.Services
{
    /// <summary>
    /// Summary description for Service1
    /// </summary>
    [WebService(Namespace = "http://webservices.sahomeloans.com/",
     Description = "This is the SA Homeloans Send Mail and Fax Service.")]

    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class SendMail : System.Web.Services.WebService
    {

        [WebMethod(Description = "This method sends an email Sends ")]
        public bool SendMailorFax()
        {
            return true;
        }




        [WebMethod(Description = " Sends Emails to Internal SAHL Staff using SMTP Email service")]
        public bool SendTextEmailInternal(string from, string to, string cc, string bcc, string subject, string body)
        {
            return MailBase.SendEmailInternal(from, to, cc, bcc, subject, body);
        }


        [WebMethod(Description = "Sends Emails to Internal SAHL Staff using SMTP Email service with an html body")]
        public bool SendEmailInternal(string from, string to, string cc, string bcc, string subject, string body, bool isBodyHTML)
        {

            return MailBase.SendEmailInternal(from, to, cc, bcc, subject, body, isBodyHTML);
        }



        [WebMethod(Description = " Sends Emails to Internal SAHL Staff using SMTP Email service with  an attachment")]
        public bool SendEmailWithAttachmentInternal(string from, string to, string cc, string bcc, string subject, string body, bool isBodyHTML, string attachment)
        {

            return MailBase.SendEmailInternal(from, to, cc, bcc, subject, body, isBodyHTML, attachment);
            ;
        }



        //[WebMethod(Description = " Sends Emails to Internal SAHL Staff using SMTP Email service with  multiple attachments")]
        //public bool SendEmailWithAttachmentsInternal(string from, string to, string cc, string bcc, string subject, string body, bool isBodyHTML, IList<string> attachments)
        //{

        //    return MailBase.SendEmailInternal(from, to, cc, bcc, subject, body, isBodyHTML, attachments);
        //}



        [WebMethod(Description = "Sends Emails to External Clients using ClientEmail table")]
        public bool SendEmailWithAttachmentsExternal(int genericKey, string from, string to, string cc, string bcc, string subject, string body, string attachment1, string attachment2, string attachment3)
        {

            return MailBase.SendEmailExternal(genericKey, from, to, cc, bcc, subject, body, attachment1, attachment2, attachment3);

        }



        [WebMethod(Description = "Sends Emails to External Clients using ClientEmail table")]
        public bool SendHTMLEmailWithAttachmentsExternal(int genericKey, string from, string to, string cc, string bcc, string subject, string body, string attachment1, string attachment2, string attachment3, int contentType)
        {

            return MailBase.SendEmailExternal(genericKey, from, to, cc, bcc, subject, body, attachment1, attachment2, attachment3, contentType);

        }



        [WebMethod(Description = " Sends Faxes to External Clients using ClientEmail table")]
        public bool SendFax(int genericKey, string from, string faxNumber, string faxAttachment)
        {

            return MailBase.SendFax(genericKey, from, faxNumber, faxAttachment);

        }



        [WebMethod(Description = " Sends Faxes with Multiple Attachments to External Clients using ClientEmail table")]
        public bool SendFaxMultipleDocs(int genericKey, string from, string to, string cc, string bcc, string subject, string body, string attachment1, string attachment2, string attachment3)
        {

            return MailBase.SendFaxMultipleDocs(genericKey, from, to, cc, bcc, subject, body, attachment1, attachment2, attachment3);

        }



        [WebMethod(Description = " Sends SMS's to External Clients using ClientEmail table")]
        public bool SendSMS(int genericKey, string message, string cellphoneNumber)
        {

            return MailBase.SendSMS(genericKey, message, cellphoneNumber);

        }
    }
}
