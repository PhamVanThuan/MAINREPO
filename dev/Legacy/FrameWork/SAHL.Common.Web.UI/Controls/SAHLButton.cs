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
    /// <summary>
    /// SAHL implementation of a standard ASP.NET button.
    /// </summary>
    [ToolboxBitmap(typeof(Button))]
    [ToolboxData("<{0}:SAHLButton runat=server></{0}:SAHLButton>")]
    public class SAHLButton : Button, ISAHLSecurityControl
    {
        private int m_ButtonSize = (int)ButtonSizeTypes.Size3;
        private string _securityTag;
        private SAHLSecurityDisplayType _securityDisplayType = SAHLSecurityDisplayType.Hide;
        private SAHLSecurityHandler _securityHandler = SAHLSecurityHandler.Automatic;

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue(ButtonSizeTypes.Size3)]
        [Localizable(true)]
        public ButtonSizeTypes ButtonSize
        {
            get { return (ButtonSizeTypes)m_ButtonSize; }
            set 
            { 
                m_ButtonSize = (int)value; 
                // setButtonStyle(); 
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            //this.Attributes["onclick"] += "disableButton(this);";

            // use prerender event handler for security checking as it happens after OnPreRender
            this.PreRender += new EventHandler(SAHLButton_PreRender);
        }

        private void SAHLButton_PreRender(object sender, EventArgs e)
        {
            SAHLSecurityControlEventArgs eventArgs = new SAHLSecurityControlEventArgs();
            if (Authenticate != null)
                Authenticate(this, eventArgs);
            SecurityHelper.DoSecurityCheck(this, eventArgs);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!DesignMode)
                RegisterCommonScript();

            //setButtonStyle();
            
            // add the client event handler to the button if it hasn't already been added - this will set the 
            // __EVENTTARGET value to the id of this button so we know what control caused the postback (by 
            // default buttons do not populate the __EVENTTARGET value
            if (OnClientClick.IndexOf("SAHLButton_updateEventTarget") == -1)
            {
                OnClientClick = "SAHLButton_updateEventTarget(this);" + OnClientClick;
            }

        }

        /// <summary>
        /// Registers common script files for the control.
        /// </summary>
        protected void RegisterCommonScript()
        {
            ClientScriptManager cs = Page.ClientScript;
            Type type = typeof(SAHLButton);
            string url = null;

            if (!Page.ClientScript.IsClientScriptIncludeRegistered(type, "SAHLButtonScript"))
            {

                // javascript include
                url = cs.GetWebResourceUrl(type, "SAHL.Common.Web.UI.Controls.Resources.SAHLButton.js");
                cs.RegisterClientScriptInclude(type, "SAHLButtonScript", url);

                // css include
                url = cs.GetWebResourceUrl(type, "SAHL.Common.Web.UI.Controls.Resources.SAHLButton.css");
                cs.RegisterClientScriptBlock(type, "SAHLButtonCss", "<link href=\"" + url + "\" type=\"text/css\" rel=\"stylesheet\">", false);

            }
        }


        //private void setButtonStyle()
        //{
        //    string cssClass = this.CssClass;
        //    if (DesignMode)
        //        cssClass = "";

        //    if (Enabled)
        //    {
        //        cssClass = cssClass + " SAHLButton ";
        //        switch ((ButtonSizeTypes)m_ButtonSize)
        //        {
        //            case ButtonSizeTypes.Size1: { cssClass += "SAHLButton1 "; break; }
        //            case ButtonSizeTypes.Size2: { cssClass += "SAHLButton2 "; break; }
        //            case ButtonSizeTypes.Size3: { cssClass += "SAHLButton3 "; break; }
        //            case ButtonSizeTypes.Size4: { cssClass += "SAHLButton4 "; break; }
        //            case ButtonSizeTypes.Size5: { cssClass += "SAHLButton5 "; break; }
        //            case ButtonSizeTypes.Size6: { cssClass += "SAHLButton6 "; break; }
        //        }
        //    }
        //    else
        //    {
        //        cssClass = cssClass + " SAHLButtonDis ";
        //        switch ((ButtonSizeTypes)m_ButtonSize)
        //        {
        //            case ButtonSizeTypes.Size1: { cssClass += "SAHLButton1Dis"; break; }
        //            case ButtonSizeTypes.Size2: { cssClass += "SAHLButton2Dis"; break; }
        //            case ButtonSizeTypes.Size3: { cssClass += "SAHLButton3Dis"; break; }
        //            case ButtonSizeTypes.Size4: { cssClass += "SAHLButton4Dis"; break; }
        //            case ButtonSizeTypes.Size5: { cssClass += "SAHLButton5Dis"; break; }
        //            case ButtonSizeTypes.Size6: { cssClass += "SAHLButton6Dis"; break; }
        //        }
        //    }
        //    this.CssClass = cssClass.Trim();
        //}

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

    public enum ButtonSizeTypes
    {
        Size1 = 1,
        Size2 = 2,
        Size3 = 3,
        Size4 = 4,
        Size5 = 5,
        Size6 = 6,
    }
}
