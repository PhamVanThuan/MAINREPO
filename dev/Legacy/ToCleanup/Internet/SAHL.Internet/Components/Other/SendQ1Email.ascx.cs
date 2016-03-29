using System;
using System.Web.UI;
using SAHL.Internet.SAHL.Web.Services.SendMail;


/* 
 * This control has been written to send an internal email specifically for the Q1 Marketing Campaign Doorway Page
 * The Page posts a JSON request to the page that sends the mail.
 * This control uses the web service to send the mail
 * The purpose of this page is to create a layer to shield our internal architecture
*/

namespace SAHL.Internet.Components.Other
{

    ///<summary>
    ///</summary>
    public partial class SendQ1Email : UserControl
    {

        private SAHL.Web.Services.SendMail.SendMail _sendMail = new SAHL.Web.Services.SendMail.SendMail();

        /// <summary>
        /// 
        /// </summary>
        public SAHL.Web.Services.SendMail.SendMail SendMailService
        {
            get { return _sendMail; }
            set { _sendMail = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Request.QueryString != null)
            {
                try
                {
                    var firstName = Request.QueryString["first_name"];
                    var surname = Request.QueryString["surname"];
                    var telNum = Request.QueryString["tel_num"];
                    var emailAddress = Request.QueryString["email_address"];

                    var phoneCheckbox = Request.QueryString["phone_checkbox"];
                    var emailCheckbox = Request.QueryString["email_checkbox"];
                    var newHomeCheckbox = Request.QueryString["new_home_checkbox"];
                    var optionsCheckbox = Request.QueryString["options_checkbox"];


                    var message = new System.Text.StringBuilder();
                    message = CompileBodyText(firstName, surname, telNum, emailAddress, phoneCheckbox, emailCheckbox, newHomeCheckbox, optionsCheckbox);

                    var mailRecipients = System.Configuration.ConfigurationManager.AppSettings["Q1CampaignMailRecipients"];

                    SendMail.Send("WebmasterH@sahomeloans.com", mailRecipients, "Q1 Website Campaign Lead", message.ToString());
                }

                catch (Exception)
                {

                    //   throw;
                }

            }

        }

        private static System.Text.StringBuilder CompileBodyText(string firstName, string surname, string telNum, string emailAddress, string phoneCheckbox, string emailCheckbox, string newHomeCheckbox, string optionsCheckbox)
        {
            System.Text.StringBuilder message = new System.Text.StringBuilder();

            message.Append("A new lead has been received from the Q1 internet campaign. Their details are: \r\n ");
            message.Append("Firstname : " + firstName + " \r\n ");
            message.Append("Surname : " + surname + " \r\n ");
            message.Append("Telephone Number : " + telNum + " \r\n ");
            message.Append("Email address : " + emailAddress + " \r\n ");
            if (phoneCheckbox == "on")
                message.Append("Can contact by phone : Yes \r\n ");
            else
                message.Append("Can contact by phone : No \r\n ");

            if (emailCheckbox == "on")
                message.Append("Can contact by email : Yes \r\n ");
            else
                message.Append("Can contact by email : No \r\n ");

            if (newHomeCheckbox == "on")
                message.Append("I am buying a new home : Yes \r\n ");
            else
                message.Append("I am buying a new home : No \r\n ");

            if (optionsCheckbox == "on")
            message.Append("I have a property. What options can you offer me  : Yes \r\n ");
            else
                message.Append("I have a property. What options can you offer me  : No \r\n ");

            
            return message;
        }
    }

}