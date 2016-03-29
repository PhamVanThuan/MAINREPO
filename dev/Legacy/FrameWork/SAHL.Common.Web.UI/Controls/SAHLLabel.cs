using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAHL.Common.DataSets;
//using SAHL.Common.ServiceFacade;
using System.Data;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Web.UI.Security;

[assembly: TagPrefix("SAHL.Common.Web.UI.Controls", "SAHL")]
namespace SAHL.Common.Web.UI.Controls
{

    [ToolboxBitmap(typeof(Label))]
    [ToolboxData("<{0}:SAHLLabel runat=server></{0}:SAHLLabel>")]
    public class SAHLLabel : Label, ISAHLSecurityControl
    {
        private TextAlign m_Alignment;
        private string _securityTag;
        private SAHLSecurityDisplayType _securityDisplayType = SAHLSecurityDisplayType.Hide;
        private SAHLSecurityHandler _securityHandler = SAHLSecurityHandler.Automatic;

        [Bindable(false)]
        [Category("Appearance")]
        [DefaultValue(typeof(TextAlign), "Left")]
        [Description("The alignment of text in the control")]
        public TextAlign TextAlign
        {
            get { return m_Alignment; }
            set { m_Alignment = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            CssClass = "LabelText";

            // use prerender event handler for security checking as it happens after OnPreRender
            this.PreRender += new EventHandler(SAHLLabel_PreRender);
        }

        private void SAHLLabel_PreRender(object sender, EventArgs e)
        {
            SAHLSecurityControlEventArgs eventArgs = new SAHLSecurityControlEventArgs();
            if (Authenticate != null)
                Authenticate(this, eventArgs);
            SecurityHelper.DoSecurityCheck(this, eventArgs);
        }

        // Added by CraigF 23/03/07
        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            base.AddAttributesToRender(writer);

            if (m_Alignment == TextAlign.Right)
                writer.AddStyleAttribute(HtmlTextWriterStyle.TextAlign, "Right");
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
