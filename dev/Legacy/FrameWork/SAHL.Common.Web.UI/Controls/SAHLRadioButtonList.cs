using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using SAHL.Common.Web.UI.Events;
using System.ComponentModel;
using SAHL.Common.Web.UI.Security;

namespace SAHL.Common.Web.UI.Controls
{
    /// <summary>
    /// Summary description for SAHLRadioButtonList
    /// </summary>
    [ToolboxBitmap(typeof(SAHLRadioButtonList), "Resources.SAHLRadioButtonList.bmp")]
    [ToolboxData("<{0}:SAHLRadioButtonList runat=server></{0}:SAHLRadioButtonList>")]
    public class SAHLRadioButtonList : RadioButtonList, ISAHLSecurityControl
    {
        private string _securityTag;
        private SAHLSecurityDisplayType _securityDisplayType = SAHLSecurityDisplayType.Hide;
        private SAHLSecurityHandler _securityHandler = SAHLSecurityHandler.Automatic;

        public SAHLRadioButtonList()
        {

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            // use prerender event handler for security checking as it happens after OnPreRender
            this.PreRender += new EventHandler(SAHLRadioButtonList_PreRender);
        }

        private void SAHLRadioButtonList_PreRender(object sender, EventArgs e)
        {
            SAHLSecurityControlEventArgs eventArgs = new SAHLSecurityControlEventArgs();
            if (Authenticate != null)
                Authenticate(this, eventArgs);
            SecurityHelper.DoSecurityCheck(this, eventArgs);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            writer.AddStyleAttribute(HtmlTextWriterStyle.OverflowY, "auto");
            writer.AddStyleAttribute(HtmlTextWriterStyle.Width, this.Width.ToString());
            writer.AddStyleAttribute(HtmlTextWriterStyle.Height, this.Height.ToString());
            writer.AddStyleAttribute(HtmlTextWriterStyle.TextAlign, "left");


            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            base.Render(writer);

            writer.RenderEndTag(); //div
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
