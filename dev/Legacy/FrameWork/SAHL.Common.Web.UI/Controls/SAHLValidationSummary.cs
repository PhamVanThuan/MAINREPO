using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using AjaxControlToolkit;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using SAHL.Common.Security;
using SAHL.Common.CacheData;

namespace SAHL.Common.Web.UI.Controls
{
    /// <summary>
    /// Provides a validation summary for display on web pages.  This is built up from a list of <see cref="IDomainMessage"/> 
    /// objects.
    /// <para>
    /// Note that this control relies on the following SAHL JavaScript files that must be included as part of the master:
    ///     <list type="number">
    ///         <item>
    ///             <term>Event.js</term>
    ///         </item>
    ///         <item>
    ///             <term>Display.js</term>
    ///         </item>
    ///     </list>
    /// </para>
    /// </summary>
    [ToolboxData("<{0}:SAHLValidationSummary runat=server></{0}:SAHLValidationSummary>")]
    [ParseChildren(true)]
    public class SAHLValidationSummary : SAHLWebControl, IValidationSummary, INamingContainer
    {
        #region Private Attributes

        private DragPanelExtender _dragPanelExtender;
        private Panel _pnlHeader;
        private Panel _pnlBody;
        private HtmlGenericControl _header;
        private HtmlGenericControl _lstErrors;
        private HtmlGenericControl _lblMessage;
        private string _errorMessage = "The following errors occurred:";
        private string _warningMessage = "If you would like to continue processing anyway, please click the \"{0}\" button, Otherwise click the \"Cancel\" button.";
        private Panel _pnlButtons;
        private SAHLButton _btnSubmit;
        private SAHLButton _btnCancel;
        private HiddenField _hidIgnoreWarnings;
        private HtmlImage _imgMinimise;
        private HtmlImage _imgTitle;
        private SAHLPrincipalCache _spc;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public SAHLValidationSummary()
        {
            // set the attributes of the control itself
            this.ID = "SAHLValidationSummary";
            this.CssClass = "SAHLValidationSummary";

            // the header
            _pnlHeader = new Panel();
            _pnlHeader.ID = "Header";
            _pnlHeader.CssClass = "SAHLValidationSummaryHeader backgroundDark";
            _imgTitle = new HtmlImage();
            _imgTitle.Alt = "";
            _imgTitle.Style.Add("float", "left");
            _pnlHeader.Controls.Add(_imgTitle);
            _header = new HtmlGenericControl("span");
            _pnlHeader.Controls.Add(_header);
            this.Controls.Add(_pnlHeader);

            // minimise button 
            _imgMinimise = new HtmlImage();
            _imgMinimise.Border = 0;
            _imgMinimise.Attributes.Add("class", "minimiseImage");
            _pnlHeader.Controls.Add(_imgMinimise);

            // the body panel
            _pnlBody = new Panel();
            _pnlBody.ID = "Body";
            _pnlBody.CssClass = "SAHLValidationSummaryBody";
            this.Controls.Add(_pnlBody);

            // the information label
            _lblMessage = new HtmlGenericControl("div");
            _pnlBody.Controls.Add(_lblMessage);

            // the control holding list of errors
            HtmlGenericControl divList = new HtmlGenericControl("div");
            _lstErrors = new HtmlGenericControl("ol");
            _pnlBody.Controls.Add(divList);
            divList.Controls.Add(_lstErrors);

            // buttons and a parent panel
            _pnlButtons = new Panel();
            _pnlButtons.Visible = false;
            _pnlButtons.CssClass = "SAHLValidationButtons";
            _pnlBody.Controls.Add(_pnlButtons);
            _btnSubmit = new SAHLButton();
            _btnSubmit.Text = "Continue";
            _btnCancel = new SAHLButton();
            _btnCancel.Text = "Cancel";
            _pnlButtons.Controls.Add(_btnSubmit);
            _pnlButtons.Controls.Add(_btnCancel);

            // the drag panel extender allows the control to be moved around the screen
            _dragPanelExtender = new DragPanelExtender();
            _dragPanelExtender.ID = "SAHLValidationSummaryDragPanel";
            this.Controls.Add(_dragPanelExtender);

            // hidden control to determine if warnings should be ignored
            _hidIgnoreWarnings = new HiddenField();
            _hidIgnoreWarnings.Value = "0";
            _hidIgnoreWarnings.ID = "_hidIgnoreWarnings";
            this.Controls.Add(_hidIgnoreWarnings);

        }
        #endregion

        #region Properties

        /// <summary>
        /// Implements <see cref="IValidationSummary.DomainMessages"/>.
        /// </summary>
        public IDomainMessageCollection DomainMessages
        {
            get
            {
                SAHLPrincipal principal = Context.User as SAHLPrincipal;
                SAHLPrincipalCache SPC = SAHLPrincipalCache.GetPrincipalCache(principal);
                return SPC.DomainMessages;
            }
        }

        /// <summary>
        /// Implements <see cref="IValidationSummary.ErrorMessage"/>.
        /// </summary>
        /// <remarks>Defaults to <strong>The following errors occurred:</strong>.</remarks>
        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }
            set
            {
                _errorMessage = value;
            }
        }

        /// <summary>
        /// Implements <see cref="IValidationSummary.HeaderText"/>.
        /// </summary>
        public string HeaderText
        {
            get
            {
                return _header.InnerText;
            }
            set
            {
                _header.InnerText = value;
            }
        }

        /// <summary>
        /// Implements <see cref="IValidationSummary.IgnoreWarnings"/>. Some views may elect to ignore 
        /// warnings or an end-user has confirmed that the warnings can be ignored - set this to true to 
        /// prevent the control from rendering.
        /// </summary>
        public bool IgnoreWarnings
        {
            get
            {
                if (Page != null)
                {
                    string formValue = Page.Request.Form[_hidIgnoreWarnings.UniqueID];
                    if (!String.IsNullOrEmpty(formValue))
                        return (formValue == "1");

                }
                return (_hidIgnoreWarnings.Value == "1");
            }
            set
            {
                _hidIgnoreWarnings.Value = (value ? "1" : "0");
            }
        }

        /// <summary>
        /// Implements <see cref="IValidationSummary.IsValid"/>.
        /// </summary>
        public bool IsValid
        {
            get
            {
                return (!RenderErrorMessages && !RenderWarningMessages);
            }
        }

        /// <summary>
        /// Used internally to determine if we need to render the error validation messages.
        /// </summary>
        private bool RenderErrorMessages
        {
            get
            {
                return (!DesignMode && DomainMessages.ErrorMessages.Count > 0);
            }
        }

        /// <summary>
        /// Used internally to determine if we need to render the warning validation messages.  They must only be rendered 
        /// if 1) we are not in design mode, 2) warning messages are not ignored, 3) there are not error messages, and 
        /// 4) there are warning messages in the domain message collection.
        /// </summary>
        private bool RenderWarningMessages
        {
            get
            {
                return (!DesignMode && !IgnoreWarnings && DomainMessages.ErrorMessages.Count == 0 && DomainMessages.WarningMessages.Count > 0);
            }
        }

        /// <summary>
        /// Implements <see cref="IValidationSummary.WarningMessage"/>.
        /// </summary>
        /// <remarks>Defaults to <strong>"You may wish to consider the following warnings.  If you would like to continue anyway, please click the \"Ignore\" button."</strong></remarks>
        public string WarningMessage
        {
            get
            {
                return _warningMessage;
            }
            set
            {
                _warningMessage = value;
            }
        }

        #endregion

        #region Methods

        private void AddMessage(IDomainMessage message)
        {
            HtmlGenericControl lc = new HtmlGenericControl("li");
            lc.InnerHtml = message.Message;
            _lstErrors.Controls.Add(lc);
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            // initialise the SPC and set the value against the principal
            _spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            _spc.IgnoreWarnings = IgnoreWarnings;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // the drag panel set up needs to happen here, otherwise the target control IDs aren't always found
            _dragPanelExtender.TargetControlID = this.UniqueID;
            _dragPanelExtender.DragHandleID = _pnlHeader.UniqueID;
        }

        /// <summary>
        /// Overridden.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (IsValid)
            {
                Visible = false;
                return;
            }

            RegisterCommonScript();

            // add event handlers to controls.
            _imgMinimise.Src = Page.ClientScript.GetWebResourceUrl(typeof(SAHLValidationSummary), "SAHL.Common.Web.UI.Controls.Resources.minimise_arrow.gif");
            _imgMinimise.Attributes.Add("onclick", "SAHLValidationSummary_toggle('" + this.ClientID + "', '" + _pnlHeader.ClientID + "', '" + _pnlBody.ClientID + "')");
            _imgMinimise.Attributes.Add("onmousedown", "stopEventBubble()");   // Event.js
            _imgMinimise.Attributes.Add("title", "Minimise");

            _btnCancel.Attributes.Add("onclick", "return SAHLValidationSummary_clickCancel('" + this.ClientID + "')");

            // add error messages to the display
            if (RenderErrorMessages)
            {
                List<string> addedErrorMessages = new List<string>();

                foreach (IDomainMessage message in DomainMessages.ErrorMessages)
                {
                    if (!addedErrorMessages.Contains(message.Message))
                    {
                        AddMessage(message);
                        addedErrorMessages.Add(message.Message);
                    }

                }
                _lblMessage.InnerText = ErrorMessage;
                _imgTitle.Src = Page.ClientScript.GetWebResourceUrl(typeof(SAHLValidationSummary), "SAHL.Common.Web.UI.Controls.Resources.ValidatorInvalid.gif");
                if (String.IsNullOrEmpty(_header.InnerText)) _header.InnerText = "Validation failed with errors";

                return;
            }

            // no errors - display warnings if they aren't ignored
            if (RenderWarningMessages)
            {
                // warnings were generated and need to be displayed, so try and get hold of the button that was clicked
                // if we're able to find the button, then we can display the buttons on the control and set the text of 
                // the continue button on the control to the same as the text of the button on the screen - when the 
                // use clicks that button a client-side even fires that actually just updates a hidden variable to tell 
                // the control to ignore warnings on the next postback, and click the screen's continue button again
                string eventSource = this.Page.Request.Form["__EVENTTARGET"];
                Button btnSource = null;

                if (eventSource != null && eventSource.Length > 0)
                {
                    btnSource = Page.FindControl(eventSource) as Button;
                    if (btnSource != null)
                    {
                        _btnSubmit.Attributes.Add("onclick", "return SAHLValidationSummary_clickIgnore('" + _hidIgnoreWarnings.ClientID + "', '" + btnSource.ClientID + "')");

                        // set the text of the submit button to be the same as the designated "submit" button on the page
                        _btnSubmit.Text = btnSource.Text;
                        if (_btnSubmit.Text.Length < 5)
                            _btnSubmit.Text = String.Format(" {0} ", _btnSubmit.Text);
                        // set the warning message to have the same text as the submit button 
                        _warningMessage = String.Format(_warningMessage, _btnSubmit.Text);
                    }
                }


                List<string> addedWarningMessages = new List<string>();
                foreach (IDomainMessage message in DomainMessages.WarningMessages)
                {
                    if (!addedWarningMessages.Contains(message.Message))
                    {
                        AddMessage(message);
                        addedWarningMessages.Add(message.Message);
                    }
                }
                if (btnSource != null)
                {
                    _pnlButtons.Visible = true;
                    _lblMessage.InnerText = WarningMessage;
                }
                else
                {
                    // if no submit button has been set then hide the buttons
                    _pnlButtons.Visible = false;
                }
                _imgTitle.Src = Page.ClientScript.GetWebResourceUrl(typeof(SAHLValidationSummary), "SAHL.Common.Web.UI.Controls.Resources.ValidatorExclaim.gif");
                if (String.IsNullOrEmpty(_header.InnerText)) _header.InnerText = "Validation failed with warnings";
            }

        }

        /// <summary>
        /// Registers common script files for the control.
        /// </summary>
        protected void RegisterCommonScript()
        {
            ClientScriptManager cs = Page.ClientScript;
            Type type = typeof(SAHLValidationSummary);
            string url = null;

            if (!Page.ClientScript.IsClientScriptIncludeRegistered(type, "SAHLValidationSummaryScript"))
            {
                // javascript include
                url = cs.GetWebResourceUrl(type, "SAHL.Common.Web.UI.Controls.Resources.SAHLValidationSummary.js");
                cs.RegisterClientScriptInclude(type, "SAHLValidationSummaryScript", url);

                // css include
                url = cs.GetWebResourceUrl(type, "SAHL.Common.Web.UI.Controls.Resources.SAHLValidationSummary.css");
                cs.RegisterClientScriptBlock(type, "SAHLValidationSummaryCss", "<link href=\"" + url + "\" type=\"text/css\" rel=\"stylesheet\">", false);

                // register start up scrip to initialise the control on the screen
                cs.RegisterStartupScript(type, "SAHLValidationSummaryStartup", "SAHLValidationSummary_init('" + this.ClientID + "');" + Environment.NewLine, true);

            }
        }

        /// <summary>
        /// Overridden.
        /// </summary>
        /// <param name="writer"></param>
        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            // if we're in design mode, just render a blank tag
            if (DesignMode)
            {
                writer.Write("[ SAHLValidationSummary ]");
                return;
            }

            // at runtime, only render if we 1) have errors, or 2) have warnings and mustn't ignore them
            if (!IsValid)
            {
                base.Render(writer);
            }
        }

        #endregion


    }
}
