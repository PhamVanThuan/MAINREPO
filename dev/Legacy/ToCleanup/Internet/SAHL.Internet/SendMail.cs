using System;
using System.Net.Mail;


namespace SAHL.Internet
{
    /// <summary>
    /// 
    /// </summary>
    public class SendMail
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public static bool Send(string from, string to, string subject, string body)
        {

            SmtpClient client = new SmtpClient(System.Configuration.ConfigurationManager.AppSettings["SmtpHost"],
                                      Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SmtpPort"]));

            try
            {
                client.Send(from, to, subject, body);
                return true;
            }
            catch (Exception)
            {
                return false;
            }



        }

        /// <summary>
        /// Sends an Email 
        /// </summary>
        /// <param name="mailMessage"></param>
        /// <returns></returns>
        public static bool Send(MailMessage mailMessage)
        {


            SmtpClient client = new SmtpClient(System.Configuration.ConfigurationManager.AppSettings["SmtpHost"], Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SmtpPort"]));
            client.Send(mailMessage);
            return true;

        }
    }
}
