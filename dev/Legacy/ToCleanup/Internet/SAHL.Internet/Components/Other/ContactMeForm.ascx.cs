using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Web.UI;

[assembly: WebResource("SAHL.Internet.Components.Other.Scripts.ContactMeForm.js", "application/javascript")]
[assembly: WebResource("SAHL.Internet.Components.Calculators.Scripts.Input.js", "application/javascript")]

namespace SAHL.Internet.Components.Other
{
    public partial class ContactMeForm : UserControl, IScriptControl
    {
        /// <summary>
        /// Gets or sets the control validation group.
        /// </summary>
        public string ValidationGroup
        {
            get { return ViewState["ValidationGroup"] as string; }
            set { ViewState["ValidationGroup"] = value; }
        }

        /// <summary>
        /// Resets the form.
        /// </summary>
        public void Reset()
        {
            txtFirstNames.Text = string.Empty;
            txtSurname.Text = string.Empty;
            txtEmailAddress.Text = string.Empty;
            phCode.Text = string.Empty;
            phNumber.Text = string.Empty;

            pnlContactMeForm.Visible = true;
            pnlContactMeMessage.Visible = false;
            pnlError.Visible = false;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load"/> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs"/> object that contains the event data. </param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!IsPostBack) Reset();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.PreRender"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data. </param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            RegisterClientScripts();
            DataBind();
        }

        protected void SendRequest(object sender, EventArgs e)
        {
            var builder = new StringBuilder();
            builder.AppendLine("A new CONTACT ME request has been received from the SA Home Loans website. Their details are:");
            builder.AppendLine(string.Format("Firstname: {0}", txtFirstNames.Text));
            builder.AppendLine(string.Format("Surname: {0}", txtSurname.Text));
            builder.AppendLine(string.Format("Telephone number: {0}-{1}", phCode.Text, phNumber.Text));
            builder.AppendLine(string.Format("E-mail address: {0}", txtEmailAddress.Text));
            builder.AppendLine(string.Format("Existing customer: {0}", chkExistingCustomer.Checked ? "Yes" : "No"));

            var recipients = System.Configuration.ConfigurationManager.AppSettings["CallMeRecipients"];
            if (string.IsNullOrWhiteSpace(recipients)) return;

            using (var service = new WebServiceBase())
            {
                try
                {
                    service.Mail.SendEmailInternal("WebmasterH@sahomeloans.com", recipients, null, null,
                                                   "Website CONTACT ME request", builder.ToString(), false);

                    pnlContactMeForm.Visible = false;
                    pnlContactMeMessage.Visible = true;
                    pnlError.Visible = false;

                    ScriptManager.RegisterClientScriptBlock(pnlContactMeForm, GetType(), "ContactMeForm.Tracking", "if (_gaq) _gaq.push([\"_trackPageview\", \"/track/contactMe-sent\"]);", true);
                }
                catch
                {
                    pnlError.Visible = true;  
                }
            }
        }

        protected void ResetClicked(object sender, EventArgs e)
        {
            Reset();
        }

        /// <summary>
        /// Create and Register the Views Javascript Model
        /// </summary>
        private void RegisterClientScripts()
        {
            var options = new
            {
                controls = new
                {
                    phCode = phCode.ClientID,
                    phNumber = phNumber.ClientID
                }
            };

            var scriptManager = ScriptManager.GetCurrent(Page);
            if (scriptManager != null) scriptManager.RegisterScriptControl(this);
            ScriptManager.RegisterStartupScript(pnlContactMeForm, GetType(), "ContactMeForm.Initialization", "$.contactMeForm.init(" + options.ToJson() + ");", true);
        }

        /// <summary>
        /// Gets a collection of script descriptors that represent ECMAScript (JavaScript) client components.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerable"/> collection of <see cref="T:System.Web.UI.ScriptDescriptor"/> objects.
        /// </returns>
        IEnumerable<ScriptDescriptor> IScriptControl.GetScriptDescriptors()
        {
            return new ScriptDescriptor[0];
        }

        /// <summary>
        /// Gets a collection of <see cref="T:System.Web.UI.ScriptReference"/> objects that define script resources that the control requires.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerable"/> collection of <see cref="T:System.Web.UI.ScriptReference"/> objects.
        /// </returns>
        IEnumerable<ScriptReference> IScriptControl.GetScriptReferences()
        {
            return new[]
                       {
                           new ScriptReference("SAHL.Internet.Components.Calculators.Scripts.Input.js", Assembly.GetExecutingAssembly().FullName),
                           new ScriptReference("SAHL.Internet.Components.Other.Scripts.ContactMeForm.js", Assembly.GetExecutingAssembly().FullName)
                       };
        }
    }
}

