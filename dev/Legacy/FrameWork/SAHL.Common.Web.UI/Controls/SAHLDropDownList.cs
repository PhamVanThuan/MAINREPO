using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Web.UI.Security;

[assembly: TagPrefix("SAHL.Common.Web.UI.Controls", "SAHL")]
namespace SAHL.Common.Web.UI.Controls
{
    /// <summary>
    /// SAHL implementation of a standard ASP.NET DropDownList.  A "-Please select-" item is added 
    /// to the control by default, as the first item in the list.
    /// </summary>
    [ToolboxBitmap(typeof(DropDownList))]
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:SAHLDropDownList runat=server></{0}:SAHLDropDownList>")]
    public class SAHLDropDownList : DropDownList, IPostBackEventHandler, ISAHLSecurityControl
    {
        [Obsolete("Please use PleaseSelectValue constant instead.", false)]
        public const string PLEASESELECTVALUE = "-select-";

        /// <summary>
        /// Default text for the "Please select" item.
        /// </summary>
        public const string PleaseSelectText = "- Please select -";

        /// <summary>
        /// Default value for the "Please select" item.
        /// </summary>
        public const string PleaseSelectValue = "-select-";

        /// <summary>
        /// Default text for when dynamic values are being loaded from an AJAX call.
        /// </summary>
        public const string LoadingText = "[Loading...";

        private bool m_PleaseSelectItem = true;
        private string pleaseSelectTextOverride;
        private bool selectedIndexChangeRaised = false;
        private bool _mandatory;
        private string _securityTag;
        private SAHLSecurityDisplayType _securityDisplayType = SAHLSecurityDisplayType.Hide;
        private SAHLSecurityHandler _securityHandler = SAHLSecurityHandler.Automatic;

        #region Properties

        /// <summary>
        /// Gets/sets whether the control is mandatory.  This will add the "mandatory" 
        /// CSS class to the control if set to true.
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(false)]
        public bool Mandatory
        {
            get
            {
                return _mandatory;
            }
            set
            {
                _mandatory = value;
            }
        }

        /// <summary>
        /// Gets/sets whether the standard "-select-" item is displayed.
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue(true)]
        [Localizable(true)]
        public bool PleaseSelectItem
        {
            get { return m_PleaseSelectItem; }
            set { m_PleaseSelectItem = value; }
        }

        [Category("Appearance")]
        [DefaultValue("- Please select -")]
        public string PleaseSelectTextOverride
        {
            get { return pleaseSelectTextOverride; }
            set { pleaseSelectTextOverride = value; }
        }

        #endregion

        #region Overridden methods

        /// <summary>
        /// Binds a datasource to the drop down list.  Overridden so that <see cref="IDictionary"/> collections can be bound 
        /// to the control easily, and to ensure that the default "Please select" item is added if necessary.
        /// </summary>
        public override void DataBind()
        {
            // if the data source is a dictionary, set the appropriate data source values
            if (DataSource is IDictionary)
            {
                DataValueField = "Key";
                DataTextField = "Value";
            }

            base.DataBind();

            // if the default item needs to be added, do it now, and select it if necessary
            if (m_PleaseSelectItem)
            {
                VerifyPleaseSelect();
                if (Page.Request[UniqueID] == null || Page.Request[UniqueID] == PleaseSelectValue)
                    this.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Registers common script files for the control.
        /// </summary>
        protected void RegisterCommonScript()
        {
            ClientScriptManager cs = Page.ClientScript;
            Type type = typeof(SAHLDropDownList);
            string url = null;

            if (!Page.ClientScript.IsClientScriptIncludeRegistered(type, "SAHLDropDownListScript"))
            {
                // javascript include
                url = cs.GetWebResourceUrl(type, "SAHL.Common.Web.UI.Controls.Resources.SAHLDropDownList.js");
                cs.RegisterClientScriptInclude(type, "SAHLDropDownListScript", url);

            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            // use prerender event handler for security checking as it happens after OnPreRender
            this.PreRender += new EventHandler(SAHLDropDownList_PreRender);

        }

        void SAHLDropDownList_PreRender(object sender, EventArgs e)
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

            Attributes["onkeypress"] = "SAHLDropDownList_searchKey(this.id);" + Attributes["onkeypress"];

            // build up the css classes to apply
            CssClass = (CssClass.Length == 0) ? "SAHLDropDownList" : CssClass + " SAHLDropDownList";
            if (Mandatory && (CssClass.IndexOf("mandatory") == -1))
                CssClass = (CssClass + " mandatory");

        }

        /// <summary>
        /// Overridden as some extra functionality is added to ensure the "Please select" item 
        /// still raises an event.  This prevents the event being raised twice for the same 
        /// action.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            if (selectedIndexChangeRaised) return;

            base.OnSelectedIndexChanged(e);
            selectedIndexChangeRaised = true;
        }

        #endregion

        #region Methods

        public void VerifyPleaseSelect()
        {
            ListItem listItem = this.Items.FindByValue(PleaseSelectValue);
            if (listItem == null)
            {
                if (!String.IsNullOrWhiteSpace(pleaseSelectTextOverride))
                    listItem = new ListItem(pleaseSelectTextOverride, PleaseSelectValue);
                else
                    listItem = new ListItem(PleaseSelectText, PleaseSelectValue);

                this.Items.Insert(0, listItem);
            }
        }

        #endregion

        #region IPostBackEventHandler Members

        /// <summary>
        /// This is implemented to ensure that even if the default "Please select" item is added right 
        /// at the end of the page cycle, the SelectedIndexChanged event is still raised.
        /// </summary>
        /// <param name="eventArgument"></param>
        public void RaisePostBackEvent(string eventArgument)
        {
            if (!selectedIndexChangeRaised)
                OnSelectedIndexChanged(new EventArgs());
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
