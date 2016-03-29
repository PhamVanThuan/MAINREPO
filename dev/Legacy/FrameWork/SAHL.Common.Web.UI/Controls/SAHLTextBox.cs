using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Web.UI.Security;

[assembly: TagPrefix("SAHL.Common.Web.UI.Controls", "SAHL")]
namespace SAHL.Common.Web.UI.Controls
{
    [ToolboxBitmap(typeof(TextBox))]
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:SAHLTextBox runat=server></{0}:SAHLTextBox>")]
    public class SAHLTextBox : TextBox, ISAHLSecurityControl
    {
        private InputType m_InputType = InputType.Normal;
        private bool m_Mandatory = false;
        private bool m_AllowNegative = false;
        private TextAlign m_Alignment;
        private string _securityTag;
        private SAHLSecurityDisplayType _securityDisplayType = SAHLSecurityDisplayType.Hide;
        private SAHLSecurityHandler _securityHandler = SAHLSecurityHandler.Automatic;
        private int i_decimalPlaces = 2;

        [Bindable(true)]
        [Category("Display")]
        [DefaultValue(false)]
        [Localizable(true)]
        public bool Mandatory
        {
            get { return m_Mandatory; }
            set { m_Mandatory = value; }
        }

        [Bindable(true)]
        [Category("Data")]
        [DefaultValue(false)]
        [Description("Specifies whether to allow negative values to be input")]
        public bool AllowNegative
        {
            get { return m_AllowNegative; }
            set { m_AllowNegative = value; }
        }

        /// <summary>
        /// Determines if we are in design mode.
        /// </summary>
        protected new bool DesignMode
        {
            get
            {
                return (HttpContext.Current == null);
            }
        }

        [Bindable(true)]
        [Category("Data")]
        [DefaultValue(InputType.Normal)]
        [Localizable(true)]
        public InputType DisplayInputType
        {
            get { return m_InputType; }
            set { m_InputType = value; }
        }

        [Bindable(true)]
        [Category("Data")]
        [DefaultValue(2)]
        [Localizable(true)]
        public int FormatDecimalPlace
        {
            get { return i_decimalPlaces; }
            set { i_decimalPlaces = value; }
        }

        [Bindable(false)]
        [Category("Appearance")]
        [DefaultValue(typeof(TextAlign), "Left")]
        [Description("The alignment of text in the control")]
        public TextAlign TextAlign
        {
            get { return m_Alignment;}
            set { m_Alignment = value;}
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            // use prerender event handler for security checking as it happens after OnPreRender
            this.PreRender += new EventHandler(SAHLTextBox_PreRender);
        }

        private void SAHLTextBox_PreRender(object sender, EventArgs e)
        {
            SAHLSecurityControlEventArgs eventArgs = new SAHLSecurityControlEventArgs();
            if (Authenticate != null)
                Authenticate(this, eventArgs);
            SecurityHelper.DoSecurityCheck(this, eventArgs);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            RegisterCommonScript();

            if (this.ReadOnly == false)
            {

                switch (m_InputType)
                {
                    case InputType.Number:
                        if (m_AllowNegative)
                        {
                            Attributes["onkeypress"] = "SAHLTextBox_NumOnly(true);";
                            Attributes["onblur"] = "SAHLTextBox_negativeBlur(this)";
                        }
                        else
                            Attributes["onkeypress"] = "SAHLTextBox_NumOnly();";

                        break;
                    case InputType.Currency:
                        if (m_AllowNegative)
                        {
                            Attributes["onkeypress"] = string.Format("SAHLTextBox_NumCurrency(true,{0});",i_decimalPlaces);
                            Attributes["onblur"] = "SAHLTextBox_negativeBlur(this)";
                        }
                        else
                            Attributes["onkeypress"] = string.Format("SAHLTextBox_NumCurrency(false,{0});",i_decimalPlaces);

                        Attributes["onblur"] = "SAHLTextBox_checkTextChanged(this);" + Attributes["onblur"];

                        break;
                    case InputType.CurrencyUnLimitedDecimals:
                        if (m_AllowNegative)
                        {
                            Attributes["onkeypress"] = "SAHLTextBox_NumCurrency(true,-1);";
                            Attributes["onblur"] = "SAHLTextBox_negativeBlur(this)";
                        }
                        else
                            Attributes["onkeypress"] = "SAHLTextBox_NumCurrency(false,-1);";

                        Attributes["onblur"] = "SAHLTextBox_checkTextChanged(this);" + Attributes["onblur"];

                        break;

                    case InputType.AlphaNumNoSpace:
                        Attributes["onkeypress"] = "SAHLTextBox_AlphaNumNoSpace();";
                        break;
                }
            }

            // build up the css classes to apply
            if (m_Mandatory && (CssClass.IndexOf("mandatory") == -1))
                CssClass = (CssClass + " mandatory");
            CssClass = (CssClass + " SAHLTextBox").Trim();

        }

        // Added by CraigF 23/03/07
        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            base.AddAttributesToRender(writer);

            if (m_Alignment == TextAlign.Right)
                writer.AddStyleAttribute(HtmlTextWriterStyle.TextAlign, "Right");
        }

        // Added by CraigF 12/04/07
        protected void RegisterCommonScript()
        {
            ClientScriptManager cs = Page.ClientScript;
            Type rsType = typeof(SAHLTextBox);
            string location = null;

            // calendar include
            if (!cs.IsClientScriptIncludeRegistered(rsType, "SAHLTextBox"))
            {
                location = cs.GetWebResourceUrl(rsType, "SAHL.Common.Web.UI.Controls.Resources.SAHLTextBox.js");
                cs.RegisterClientScriptInclude(rsType, "SAHLTextBox", location);
            }
        }

        #region ISAHLSecurityControl Members

        /// <summary>
        /// Tag that identifies the security block in the control.  This should be unique 
        /// per object (view/presenter).
        /// </summary>
        [Category("Authentication")]
        [Description("The configuration security tag to apply to the control.")]
        public string SecurityTag
        {
            get
            {
                return _securityTag;
            }
            set
            {
                _securityTag = value;
            }
        }

        /// <summary>
        /// Gets/sets what happens to the control when authentication fails.  This 
        /// defaults to <see cref="SAHLSecurityDisplayType.Hide"/>
        /// </summary>
        [Category("Authentication")]
        [DefaultValue(SAHLSecurityDisplayType.Hide)]
        [Description("Specifies what happens to the control when authentication fails.")]
        public SAHLSecurityDisplayType SecurityDisplayType
        {
            get
            {
                return _securityDisplayType;
            }
            set
            {
                _securityDisplayType = value;
            }
        }

        /// <summary>
        /// Gets/sets what happens to the control when authentication fails.  This 
        /// defaults to <see cref="SAHLSecurityHandler.Automatic"/>
        /// </summary>
        [Category("Authentication")]
        [DefaultValue(SAHLSecurityHandler.Automatic)]
        [Description("Specifies whether a custom implementation of security exists or if security is automatic.")]
        public SAHLSecurityHandler SecurityHandler
        {
            get
            {
                return _securityHandler;
            }
            set
            {
                _securityHandler = value;
            }
        }

        /// <summary>
        /// Occurs when the control tries to authenticate i.e. ensure that all security 
        /// restrictions have been passed.
        /// </summary>
        public event SAHLSecurityControlEventHandler Authenticate;

        #endregion

    }

    public enum InputType
    {
        Normal,
        Number,
        Currency,
        CurrencyUnLimitedDecimals,
        AlphaNumNoSpace
    }
}
