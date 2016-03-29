using System;
using SAHL.Internet.Logging;


namespace SAHL.Internet.Components.Other
{
    public partial class SpectacularAdCampaign : System.Web.UI.UserControl
    {

        //private SAHLWebSession DNNWebSession = new SAHLWebSession();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {


            if (!IsPostBack)
            {
                RegisterClientScripts();

                txtEmail.Attributes.Add("onclick", "ClearInput('youremail@domain.co.za', this.id);");
                txtEmail.Attributes.Add("onblur", "insertInput('youremail@domain.co.za', this.id);");

                chkFeedback.Attributes.Add("onclick", "showHide('email_item')");
                chkMoreInfo.Attributes.Add("onclick", "showHide('email_item')");
            }
            else
            {
                // User has submitted the form hide form objects and display thank you message
                pnlWebApplicationFormSummary.Visible = false;
                resultsPanel.Visible = true;
            }

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ApplicationFormCommand(object sender, EventArgs e)
        {

            // Send the Email
            System.Text.StringBuilder message = new System.Text.StringBuilder();
            message.AppendLine("Submission From: " + txtEmail.Text + ", ");

            switch (criteria.SelectedIndex)
            {
                case 0:
                    message.AppendLine("Gran loses her knitting as the dog jumps on her lap" + ", ");
                    break;
                case 1:
                    message.AppendLine("The kids, the dog and gran take over the couch." + ", ");
                    break;
                case 2:
                    message.AppendLine("Dad moves out of the lounge and mows the lawn." + ", ");
                    break;
                case 3:
                    message.AppendLine("Dad lies down with his head on mom's lap - and gran stands along side - looking disgruntled." + ", ");
                    break;
                case 4:
                    message.AppendLine("Dad sits on the couch next to mom - and leaves gran sitting on the side table  holding the fishbowl" + ", ");
                    break;
                case 5:
                    message.AppendLine("Dad starts knitting and the dog jumps on the couch and everyone leaves" + ", ");
                    break;
                case 6:
                    message.AppendLine("Gran and the dog take over the couch. Mom is on the floor. Dad and the kids leave" + ", ");
                    break;
                case 7:
                    message.AppendLine("Kids take over the couch and destroy gran's knitting." + ", ");
                    break;

            }

            if (chkFeedback.Checked)
                message.AppendLine("Please send me feedback about what happened." + ", ");
            else
                message.AppendLine("I dont want to know what happened." + ", ");


            if (chkMoreInfo.Checked)
                message.AppendLine("Please send me more info about SA Home Loans." + ", ");
            else
                message.AppendLine("I dont want any info on SA Home Loans thanks." + ", ");



            // Store Data 
            string filename = System.Configuration.ConfigurationManager.AppSettings["LogPath"];
            CreateLogFiles Log = new CreateLogFiles();
            Log.ErrorLog(Server.MapPath("filename"), "Test Log entry.");
            Log.ErrorLog(filename, message.ToString());

            // Send the mail to 
            if (chkMoreInfo.Checked && (txtEmail.Text != "youremail@domain.co.za"))
            {
                string mail1 = System.Configuration.ConfigurationManager.AppSettings["Spectacular"];
                string mail2 = System.Configuration.ConfigurationManager.AppSettings["SpectacularCC"];
                SendMail.Send("WebmasterH@sahomeloans.com", mail1 , "SA Homeloans - Spectacular Feedback", message.ToString());
                SendMail.Send("WebmasterH@sahomeloans.com", mail2, "SA Homeloans - Spectacular Feedback", message.ToString());
            }

        }


        private void RegisterClientScripts()
        {
            System.Text.StringBuilder mBuilder = new System.Text.StringBuilder();


            mBuilder.AppendLine("  function showHide(id){");
            mBuilder.AppendLine("  el =	document.getElementById(id);	");
            mBuilder.AppendLine("  el.style.display	= ((document.getElementById('" + chkFeedback.ClientID + "').checked == false) && (document.getElementById('" + chkMoreInfo.ClientID + "').checked == false)) ? 'none' : 'block';");
            mBuilder.AppendLine(" }");

            if (!Page.ClientScript.IsClientScriptBlockRegistered(GetType(), "lstScripts"))
                Page.ClientScript.RegisterClientScriptBlock(GetType(), "lstScripts", mBuilder.ToString(), true);
        }

    }
}