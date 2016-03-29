using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Web.UI.Security;

namespace SAHL.Common.Web.UI.Controls
{

    /// <summary>
    /// Control providing autocomplete functionality.  This control works like a DropDownList in that 
    /// it uses key/value pairs, and should be used instead of the AjaxToolKit autocomplete control 
    /// which only works off an array of string values.
    /// </summary>
    [ToolboxBitmap(typeof(SAHLAutoComplete), "Resources.SAHLAutoComplete.bmp")]
    [ToolboxData("<{0}:SAHLAutoComplete runat=server></{0}:SAHLAutoComplete>")]
    [ParseChildren(true)]
    public class SAHLAutoComplete : CompositeControl, IPostBackEventHandler, ISAHLSecurityControl
    {
        #region Events

        /// <summary>
        /// Event raised when an item is selected on the control and the <see cref="AutoPostBack"/> property 
        /// is set to true.
        /// </summary>
        public event KeyChangedEventHandler ItemSelected;

        #endregion

        #region Private Attributes

        private string _targetControlID;
        private TextBox _textBox;
        private string _serviceMethod;
        private VerticalPositions _verticalPosition = VerticalPositions.Below;
        private HorizontalPositions _horizontalPosition = HorizontalPositions.Left;
        private string _cssClass = "SAHLAutoComplete_Default";
        private string _cssClassItem = "SAHLAutoComplete_DefaultItem";
        private string _cssClassItemSelected = "SAHLAutoComplete_DefaultItemSelected";
        private HiddenField _hiddenField = null;
        private bool _autoPostback = false;
        private SAHLAutoCompleteParentControls _parentControls = new SAHLAutoCompleteParentControls();
        private int _maxRowCount = -1;
        private string _clientClickFunction = "";
        private int _minCharacters = 0;
        private string _securityTag;
        private SAHLSecurityDisplayType _securityDisplayType = SAHLSecurityDisplayType.Hide;
        private SAHLSecurityHandler _securityHandler = SAHLSecurityHandler.Automatic;

        #endregion

        #region Enumerations

        /// <summary>
        /// Enumeration describing how an SAHLAutoComplete control can be vertically aligned with it's parent text box.
        /// </summary>
        public enum VerticalPositions
        {
            Above = 0,
            Below = 1
        }

        /// <summary>
        /// Enumeration describing how an SAHLAutoComplete control can be horizontally aligned with it's parent text box.
        /// </summary>
        public enum HorizontalPositions
        {
            Left = 0,
            Right = 1
        }

        #endregion

        #region Overridden Methods

        /// <summary>
        /// Overridden for implementation of parent controls collection.
        /// </summary>
        /// <param name="obj"></param>
        protected override void AddParsedSubObject(object obj)
        {
            if (obj is SAHLAutoCompleteParentControl)
            {
                _parentControls.Add((SAHLAutoCompleteParentControl)obj);
            }
            else
            {
                base.AddParsedSubObject(obj);
            }
        }

        /// <summary>
        /// Overridden.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            // create the hidden item
            _hiddenField = new HiddenField();
            _hiddenField.ID = this.ID + "_Hidden";
            _hiddenField.Value = SelectedValue;
            this.Controls.Add(_hiddenField);

            // use prerender event handler for security checking as it happens after OnPreRender
            this.PreRender += new EventHandler(SAHLAutoComplete_PreRender);
        }

        private void SAHLAutoComplete_PreRender(object sender, EventArgs e)
        {
            SAHLSecurityControlEventArgs eventArgs = new SAHLSecurityControlEventArgs();
            if (Authenticate != null)
                Authenticate(this, eventArgs);
            SecurityHelper.DoSecurityCheck(this, eventArgs);
        }

        /// <summary>
        /// Overridden to add attributes to the associated textbox.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!DesignMode)
            {
                // make sure the textbox exists, and if so, catch the PreRender event so we can add the 
                // attibutes we need
                _textBox = ControlHelpers.FindControlRecursive(this.Page, TargetControlID) as TextBox;
                if (_textBox == null)
                {
                    throw new ApplicationException("Unable to locate text box with ID " + TargetControlID);
                }
                _textBox.PreRender += new EventHandler(textBox_PreRender);

                // register the script
                RegisterCommonScript();

            }
        }

        /// <summary>
        /// Overridden.
        /// </summary>
        /// <param name="writer"></param>
        protected override void Render(HtmlTextWriter writer)
        {
            // base.Render(writer);
            if (DesignMode)
            {
                writer.Write("SAHLAutoComplete: [" +
                    ((TargetControlID.Length == 0) ? "TextBox not set" : TargetControlID) +
                    "]"
                    );
            }
            else
            {
                _hiddenField.RenderControl(writer);
            }
        }


        #endregion

        #region Properties

        /// <summary>
        /// Gets/sets whether the control causes a postback when an item is selected.  This defaults to false.
        /// </summary>
        [BrowsableAttribute(true)]
        public bool AutoPostBack
        {
            get
            {
                return _autoPostback;
            }
            set
            {
                _autoPostback = value;
            }
        }


        /// <summary>
        /// Gets/sets the name of a JavaScript function that will be called when one of the items in the AutoComplete 
        /// list is selected.  This method must take one parameter - the value of which will be populated with an 
        /// object with four properties: 
        /// <list type="bullet">
        ///     <listheader>
        ///         <term>textBox</term>
        ///         <description>The text box displaying the autocomplete list where the event occurred.</description>
        ///     </listheader>
        ///     <listheader>
        ///         <term>autoComplete</term>
        ///         <description>The AutoComplete control where the event occurred.</description>
        ///     </listheader>
        ///     <listheader>
        ///         <term>key</term>
        ///         <description>The key of the item that was clicked.</description>
        ///     </listheader>
        ///     <listheader>
        ///         <term>value</term>
        ///         <description>The value (text) of the item that was clicked.</description>
        ///     </listheader>
        ///     <listheader>
        ///         <term>displayText</term>
        ///         <description>The text displayed in the textbox when the item is selected.</description>
        ///     </listheader>
        /// </list>
        /// </summary>
        [BrowsableAttribute(true)]
        public string ClientClickFunction
        {
            get
            {
                return _clientClickFunction;
            }
            set
            {
                _clientClickFunction = value;
            }
        }

        /// <summary>
        /// Gets/sets the CSS class applied to the autocomplete box (not the items).  A default style is applied.
        /// </summary>        
        [BrowsableAttribute(true)]
        [Category("Appearance")]
        public override string CssClass
        {
            get
            {
                return _cssClass;
            }
            set
            {
                _cssClass = value;
            }
        }

        /// <summary>
        /// Gets/sets the CSS class applied to the items in the autocomplete box.  A default style is applied.
        /// </summary>
        [BrowsableAttribute(true)]
        [Category("Appearance")]
        public string CssClassItem
        {
            get
            {
                return _cssClassItem;
            }
            set
            {
                _cssClassItem = value;
            }
        }

        /// <summary>
        /// Gets/sets the CSS class applied to the items in the autocomplete box when the mouse hovers over them or they
        /// are selected using the up or down arrow keys. A default style is applied.
        /// </summary>
        [BrowsableAttribute(true)]
        [Category("Appearance")]
        public string CssClassItemSelected
        {
            get
            {
                return _cssClassItemSelected;
            }
            set
            {
                _cssClassItemSelected = value;
            }
        }


        /// <summary>
        /// Gets/sets the horizontal positioning of the autocomplete box.  This defaults to <see cref="HorizontalPosition"/>.
        /// </summary>
        [BrowsableAttribute(true)]
        [Category("Appearance")]
        public HorizontalPositions HorizontalPosition
        {
            get
            {
                return _horizontalPosition;
            }
            set
            {
                _horizontalPosition = value;
            }
        }

        /// <summary>
        /// Gets/sets the maximum number of rows that can be displayed.  The default is -1, which means no limit is set.
        /// </summary>
        [BrowsableAttribute(true)]
        [Category("Appearance")]
        public int MaxRowCount
        {
            get
            {
                return _maxRowCount;
            }
            set
            {
                _maxRowCount = value;
            }
        }

        /// <summary>
        /// Gets/sets the minimum number of characters that must be entered before the AJAX call is made.
        /// </summary>
        [BrowsableAttribute(true)]
        [Category("Appearance")]
        public int MinCharacters
        {
            get
            {
                return _minCharacters;
            }
            set
            {
                _minCharacters = value;
            }
        }

        /// <summary>
        /// Gets a reference to the ParentControls list.  These are elements whose values are sent as 
        /// part of the autocomplete web service call.  Currently only TextBox, DropDownList and 
        /// Hidden input types are support.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Category("Data Retrieval")]
        public SAHLAutoCompleteParentControls ParentControls
        {
            get
            {
                return _parentControls;
            }
        }

        /// <summary>
        /// Gets/sets the selected value of the AutoComplete control.
        /// </summary>
        public string SelectedValue
        {
            get
            {
                string selectedValue = "";

                if (!DesignMode)
                {
                    selectedValue = _hiddenField.Value;
                    if (String.IsNullOrEmpty(selectedValue))
                    {
                        selectedValue = Page.Request.Form[_hiddenField.UniqueID];
                    }
                    if (selectedValue == null) selectedValue = "";
                }
                return selectedValue;
            }
            set
            {
                _hiddenField.Value = value;
            }
        }

        /// <summary>
        /// Gets/sets the AJAX method that is called to populate the autocomplete list.
        /// </summary>
        [BrowsableAttribute(true)]
        [Category("Data Retrieval")]
        public string ServiceMethod
        {
            get
            {
                return _serviceMethod;
            }
            set
            {
                _serviceMethod = value;
            }
        }

        /// <summary>
        /// The ID of the <see cref="System.Web.UI.WebControls.TextBox">TextBox</see> to which the 
        /// SAHLAutoComplete control is associated.
        /// </summary>
        [BrowsableAttribute(true)]
        [TypeConverter(typeof(TextBoxStringConverter))]
        public string TargetControlID
        {
            get
            {
                return _targetControlID;
            }
            set
            {
                _targetControlID = value;
            }
        }

        /// <summary>
        /// Gets/sets the vertical positioning of the autocomplete box.  This defaults to <see cref="VerticalPosition"/>.
        /// </summary>
        [BrowsableAttribute(true)]
        [Category("Appearance")]
        public VerticalPositions VerticalPosition
        {
            get
            {
                return _verticalPosition;
            }
            set
            {
                _verticalPosition = value;
            }
        }
        #endregion

        #region Methods

        /// <summary>
        /// Raises the <see cref="ItemSelected"/> event.
        /// </summary>
        /// <param name="e"></param>
        public void OnItemSelected(KeyChangedEventArgs e)
        {
            if (ItemSelected != null)
                ItemSelected(this, e);
        }

        /// <summary>
        /// Registers common script files for the control.
        /// </summary>
        protected void RegisterCommonScript()
        {
            ClientScriptManager cs = Page.ClientScript;
            Type type = typeof(SAHLAutoComplete);
            string url = null;

            if (!cs.IsClientScriptIncludeRegistered(type, "SAHLAutoCompleteScript"))
            {
                // javascript include
                url = cs.GetWebResourceUrl(type, "SAHL.Common.Web.UI.Controls.Resources.SAHLAutoComplete.js");
                cs.RegisterClientScriptInclude(type, "SAHLAutoCompleteScript", url);

                // css include
                url = cs.GetWebResourceUrl(type, "SAHL.Common.Web.UI.Controls.Resources.SAHLAutoComplete.css");
                cs.RegisterClientScriptBlock(type, "SAHLAutoCompleteCss", "<link href=\"" + url + "\" type=\"text/css\" rel=\"stylesheet\">", false);

            }
        }

        #endregion

        #region Event Handlers

        void textBox_PreRender(object sender, EventArgs e)
        {
            if (!DesignMode && Visible)
            {
                TextBox textBox = (TextBox)sender;

                // build up the list of parent control IDs
                StringBuilder sbParentControls = new StringBuilder();
                foreach (SAHLAutoCompleteParentControl parentControl in ParentControls)
                {
                    Control c = ControlHelpers.FindControlRecursive(Page, parentControl.ControlID);
                    if (c == null) throw new Exception("Unable to locate parent control with ID " + parentControl.ControlID + ".");
                    if (sbParentControls.Length > 0) sbParentControls.Append("|");
                    sbParentControls.Append(c.ClientID);
                }

                // add the client-side event handlers to the textbox
                textBox.Attributes.Add("onfocus", 
                    String.Format("SAHLAutoComplete_focus(this, '{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}')",
                        _hiddenField.ClientID,
                        ServiceMethod,
                        (this.VerticalPosition == VerticalPositions.Below ? "0" : "1"),
                        (this.HorizontalPosition == HorizontalPositions.Left ? "0" : "1"),
                        this.MaxRowCount.ToString(),
                        MinCharacters.ToString(),
                        CssClass,
                        CssClassItem,
                        CssClassItemSelected,
                        sbParentControls.ToString(),
                        (AutoPostBack ? Page.ClientScript.GetPostBackEventReference(this, this.ID).Replace("'", "\\'") : ""),
                        ClientClickFunction
                    )
                );
                textBox.Attributes.Add("onblur", "SAHLAutoComplete_blur(this)");
                textBox.Attributes.Add("onkeydown", "SAHLAutoComplete_keyDown(event)");
                textBox.Attributes.Add("onkeyup", "SAHLAutoComplete_keyUp(event)");
                textBox.Attributes.Add("autocomplete", "off");

                if (textBox.CssClass.IndexOf("SAHLAutoComplete_TextBox") == -1)
                    textBox.CssClass += " SAHLAutoComplete_TextBox";

            }
        }

        #endregion

        #region Internal Classes

        /// <summary>
        /// This class is used to display the textbox options in the UI Designer when the SAHLAutoComplete 
        /// component is selected.
        /// </summary>
        internal class TextBoxStringConverter : System.ComponentModel.StringConverter
        {

            public override bool GetStandardValuesSupported(
                           ITypeDescriptorContext context)
            {
                return true;
            }

            public override bool GetStandardValuesExclusive(
                           ITypeDescriptorContext context)
            {
                return false;
            }

            public override StandardValuesCollection GetStandardValues(
                          ITypeDescriptorContext context)
            {

                List<string> _lstBoxes = new List<string>();

                if (context != null)    // design-time check
                {
                    foreach (IComponent component in context.Container.Components)
                    {
                        if (component.Site != null)
                        {
                            // this is a bit of a cheat to cover all textboxes - should this be 
                            // a "is TextBox" check?
                            if (component.GetType().ToString().IndexOf("TextBox") > -1)
                            {
                                _lstBoxes.Add(component.Site.Name);
                            }
                        }
                    }
                }

                return new StandardValuesCollection(_lstBoxes);
            }
        }

        #endregion

        #region IPostBackEventHandler Members

        /// <summary>
        /// Implements <see cref="IPostBackEventHandler.RaisePostBackEvent"/>.  This is used to raise the 
        /// <see cref="ItemSelected"/> event.
        /// </summary>
        /// <param name="eventArgument"></param>
        public void RaisePostBackEvent(string eventArgument)
        {
            OnItemSelected(new KeyChangedEventArgs(this.SelectedValue));
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

    #region SAHLAutoCompleteItem Class

    /// <summary>
    /// An item that can be added to a <see cref="SAHLAutoComplete"/> control.
    /// </summary>
    [Serializable]
    public class SAHLAutoCompleteItem
    {

        private string _value;
        private string _text;
        private string _displayText;

        /// <summary>
        /// Constructor
        /// </summary>
        public SAHLAutoCompleteItem()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="value">The value of the item.</param>
        /// <param name="text">The text displayed by the item.</param>
        public SAHLAutoCompleteItem(string value, string text) : this(value, text, text)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="value">The value of the item.</param>
        /// <param name="text">The text displayed by the item.</param>
        /// <param name="displayText"></param>
        public SAHLAutoCompleteItem(string value, string text, string displayText)
        {
            _value = value;
            _text = text;
            _displayText = displayText;
        }

        /// <summary>
        /// The text displayed when the item is selected.  By default this is the same as <see cref="Text"/>, but can be 
        /// set to something different.
        /// </summary>
        public string DisplayText
        {
            get
            {
                return _displayText;
            }
            set
            {
                _displayText = value;
            }
        }

        /// <summary>
        /// The text displayed by the item.
        /// </summary>
        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
            }
        }

        /// <summary>
        /// The value of the item.
        /// </summary>
        public string Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }

        public override string ToString()
        {
            return Text;
        }
    }

    #endregion

    #region SAHLAutoCompleteParentControls Class (Collection)

    /// <summary>
    /// Collection containing parent controls for a <see cref="SAHLAutoComplete"/> control.
    /// </summary>
    public class SAHLAutoCompleteParentControls : List<SAHLAutoCompleteParentControl>
    {

    }

    #endregion

    #region SAHLAutoCompleteParentControl class

    /// <summary>
    /// Used to declare control dependencies on a <see cref="SAHLAutoComplete"/> control.
    /// </summary>
    // [Serializable()]
    public class SAHLAutoCompleteParentControl
    {
        private string _controlID = String.Empty;

        public SAHLAutoCompleteParentControl()
        {
        }

        public SAHLAutoCompleteParentControl(string controlID)
        {
            _controlID = controlID;
        }

        /// <summary>
        /// The ID of the control that depends on the item selected.
        /// </summary>
        public string ControlID
        {
            get { return _controlID; }
            set { _controlID = value; }
        }

    }

    #endregion

}
