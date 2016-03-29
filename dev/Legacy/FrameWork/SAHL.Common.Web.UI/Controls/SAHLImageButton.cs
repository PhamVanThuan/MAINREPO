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
    [ToolboxBitmap(typeof(ImageButton))]
    [ToolboxData("<{0}:SAHLImageButton runat=server></{0}:SAHLImageButton>")]
    public class SAHLImageButton : ImageButton, ISAHLSecurityControl
    {
        private string _securityTag;
        private SAHLSecurityDisplayType _securityDisplayType = SAHLSecurityDisplayType.Hide;
        private SAHLSecurityHandler _securityHandler = SAHLSecurityHandler.Automatic;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            // use prerender event handler for security checking as it happens after OnPreRender
            this.PreRender += new EventHandler(SAHLImageButton_PreRender);
        }

        private void SAHLImageButton_PreRender(object sender, EventArgs e)
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
           
            // add the client event handler to the button if it hasn't already been added - this will set the 
            // __EVENTTARGET value to the id of this button so we know what control caused the postback (by 
            // default buttons do not populate the __EVENTTARGET value
            if (OnClientClick.IndexOf("SAHLImageButton_updateEventTarget") == -1)
            {
                OnClientClick = "SAHLImageButton_updateEventTarget(this);" + OnClientClick;
            }

        }

        /// <summary>
        /// Registers common script files for the control.
        /// </summary>
        protected void RegisterCommonScript()
        {
            ClientScriptManager cs = Page.ClientScript;
            Type type = typeof(SAHLImageButton);
            string url = null;

            if (!Page.ClientScript.IsClientScriptIncludeRegistered(type, "SAHLImageButtonScript"))
            {

                // javascript include
                url = cs.GetWebResourceUrl(type, "SAHL.Common.Web.UI.Controls.Resources.SAHLImageButton.js");
                cs.RegisterClientScriptInclude(type, "SAHLImageButtonScript", url);

                // css include
                url = cs.GetWebResourceUrl(type, "SAHL.Common.Web.UI.Controls.Resources.SAHLImageButton.css");
                cs.RegisterClientScriptBlock(type, "SAHLImageButtonCss", "<link href=\"" + url + "\" type=\"text/css\" rel=\"stylesheet\">", false);

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
}
