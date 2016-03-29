using System;
using System.Web.UI.WebControls;
using System.Web;
using System.Web.UI;
using System.ComponentModel;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Web.UI.Security;

namespace SAHL.Common.Web.UI.Controls
{
    /// <summary>
    /// Serves as a base class for SAHL Web controls.  This provides SAHL Security support.
    /// </summary>
    public class SAHLWebControl : WebControl, ISAHLSecurityControl
    {
        private string _securityTag;
        private SAHLSecurityDisplayType _securityDisplayType = SAHLSecurityDisplayType.Hide;
        private SAHLSecurityHandler _securityHandler = SAHLSecurityHandler.Automatic;

        #region Properties

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

        #endregion

        #region Page Life Cycle Methods

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            // use prerender event handler for security checking as it happens after OnPreRender
            this.PreRender += new EventHandler(SAHLWebControl_PreRender);
        }

        private void SAHLWebControl_PreRender(object sender, EventArgs e)
        {
            SAHLSecurityControlEventArgs eventArgs = new SAHLSecurityControlEventArgs();
            if (Authenticate != null)
                Authenticate(this, eventArgs);
            SecurityHelper.DoSecurityCheck(this, eventArgs);
        }

        #endregion

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
