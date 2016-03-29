using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using System.Text;
using System.Security.Permissions;

[assembly: TagPrefix("SAHL.Common.Web.UI.Controls", "SAHL")]
namespace SAHL.Common.Web.UI.Controls
{
    public abstract class SAHLBaseValidator : WebControl, IValidator
    {
        // Note: these script-enabled controls have two sets
        // of client-side script. 
        // There is a fixed set in a script library called 
        // SAHLValidators.js. This is set as an include by using webresoucrce functionality.
        // The second is the dynamic block 
        // below that contains some inline script that 
        // should be executed at the end of the page load.
        // This is declared using RegisterStartupScript.
        protected const string ValidatorIncludeScriptKey = "SAHLValidatorsIncludeScript";
        protected const string TooltipIncludeScriptKey = "SAHLValidatorTooltipScript";
        protected const string ValidatorStartupScript = @"
<script type=""text/javascript"">
<!--
    var Page_ValidationActive = false;
    if (typeof(ValidatorOnLoad) == ""function"") 
    {
        ValidatorOnLoad();
    }

    function ValidatorOnSubmit() 
    {
        if (Page_ValidationActive) 
        {
            return ValidatorCommonOnSubmit();
        }
        else 
        {
            return true;
        }
    }
// -->
</script>
        ";

        protected bool preRenderCalled;
        protected bool isValid;
        protected bool propertiesChecked;
        protected bool propertiesValid;

        protected SAHLBaseValidator()
        {
            isValid = true;
            propertiesChecked = false;
            propertiesValid = true;
        }

        #region Properties

        [
         Category("Behavior"),
         DefaultValue(""),
         Description("The control to validate."),
         TypeConverter(typeof(ValidatedControlConverter))
         ]
        public string ControlToValidate
        {
            get
            {
                object o = this.ViewState["ControlToValidate"];
                if (o != null)
                {
                    return (string)o;
                }
                return string.Empty;
            }
            set
            {
                this.ViewState["ControlToValidate"] = value;
            }

        }

        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue(""),
        Description("Error Message")
        ]
        public string ErrorMessage
        {
            get
            {
                object o = ViewState["ErrorMessage"];
                return ((o == null) ? String.Empty : (string)o);
            }
            set
            {
                ViewState["ErrorMessage"] = value;
            }
        }

        [
        Category("Behavior"),
        DefaultValue(true),
        Description("Enable Client Script")
        ]
        public bool EnableClientScript
        {
            get
            {
                object o = ViewState["EnableClientScript"];
                return ((o == null) ? true : (bool)o);
            }
            set
            {
                ViewState["EnableClientScript"] = value;
            }
        }

        public override bool Enabled
        {
            get
            {
                return base.Enabled;
            }
            set
            {
                base.Enabled = value;
                // When a validator is disabled, 
                // generally, the intent is not to
                // make the page invalid for that round trip.
                if (!value)
                {
                    isValid = true;
                }
            }
        }

        [
        Browsable(false),
        Category("Behavior"),
        DefaultValue(true),
        Description("Is Valid"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
        ]
        public bool IsValid
        {
            get
            {
                return isValid;
            }
            set
            {
                isValid = value;
            }
        }

        [
        Themeable(false), 
        DefaultValue(""), 
        Category("Behavior")
        ]
        public string InitialValue
        {
            get
            {
                object o = this.ViewState["InitialValue"];
                if (o != null)
                {
                    return (string)o;
                }
                return string.Empty;
            }
            set
            {
                this.ViewState["InitialValue"] = value;
            }
        }

        [
        Category("Behavior"), 
        Themeable(false), 
        DefaultValue("")
        ]
        public virtual string ValidationGroup
        {
            get
            {
                object o = this.ViewState["ValidationGroup"];
                if (o != null)
                {
                    return (string)o;
                }
                return string.Empty;
            }
            set
            {
                this.ViewState["ValidationGroup"] = value;
            }
        }

        [
         Bindable(true),
         Category("Appearance"),
         DefaultValue(ValidatorDisplay.Dynamic),
         Description("Display"),
         ]
        public ValidatorDisplay Display
        {
            get
            {
                object o = ViewState["Display"];
                return ((o == null) ? ValidatorDisplay.Dynamic : (ValidatorDisplay)o);
            }
            set
            {
                if (value < ValidatorDisplay.None || value > ValidatorDisplay.Dynamic)
                {
                    throw new ArgumentException();
                }
                ViewState["Display"] = value;
            }
        }

        //[
        //Category("Behavior"),
        //DefaultValue(false),
        //Themeable(false)
        //]
        //public bool SetFocusOnError
        //{
        //    get
        //    {
        //        object obj1 = this.ViewState["SetFocusOnError"];
        //        if (obj1 != null)
        //        {
        //            return (bool)obj1;
        //        }
        //        return false;
        //    }
        //    set
        //    {
        //        this.ViewState["SetFocusOnError"] = value;
        //    }
        //}

        #endregion

        protected bool PropertiesValid
        {
            get
            {
                if (!propertiesChecked)
                {
                    propertiesValid = ControlPropertiesValid();
                    propertiesChecked = true;
                }
                return propertiesValid;
            }
        }

        internal static void AddExpandoAttribute(Page page, HtmlTextWriter writer, string controlId, string attributeName, string attributeValue, bool encode)
        {
            if (writer != null)
            {
                writer.AddAttribute(attributeName, attributeValue, encode);
            }
            else
            {
                page.ClientScript.RegisterExpandoAttribute(controlId, attributeName, attributeValue, encode);
            }
        }

        internal void AddExpandoAttribute(HtmlTextWriter writer, string controlId, string attributeName, string attributeValue, bool encode)
        {
            SAHLBaseValidator.AddExpandoAttribute(this.Page, writer, controlId, attributeName, attributeValue, encode);
        }

        internal void AddExpandoAttribute(HtmlTextWriter writer, string controlId, string attributeName, string attributeValue)
        {
            SAHLBaseValidator.AddExpandoAttribute(this.Page, writer, controlId, attributeName, attributeValue, true);
        }

        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            bool flag1 = !this.Enabled;
            if (flag1)
            {
                this.Enabled = true;
            }
            try
            {
                base.EnsureID();
                string TheClientID = this.ClientID;

                writer.AddAttribute("onmouseover", "Validator_MouseOver('" + ClientID + "');");
                writer.AddAttribute("onmouseout", "Validator_MouseOut('" + ClientID + "');");

                HtmlTextWriter writer1 = null;// base.EnableLegacyRendering ? writer : null;
                if (this.ControlToValidate.Length > 0)
                {
                    this.AddExpandoAttribute(writer1, TheClientID, "controltovalidate", this.GetControlRenderID(this.ControlToValidate));
                }
                //if (this.SetFocusOnError)
                //{
                //    this.AddExpandoAttribute(writer1, TheClientID, "focusOnError", "t", false);
                //}
                if (this.ErrorMessage.Length > 0)
                {
                    this.AddExpandoAttribute(writer1, TheClientID, "errormessage", this.ErrorMessage);
                }
                ValidatorDisplay display1 = this.Display;
                if (display1 != ValidatorDisplay.Static)
                {
                    this.AddExpandoAttribute(writer1, TheClientID, "display", PropertyConverter.EnumToString(typeof(ValidatorDisplay), display1), false);
                }
                if (!this.IsValid)
                {
                    this.AddExpandoAttribute(writer1, TheClientID, "isvalid", "False", false);
                }
                if (flag1)
                {
                    this.AddExpandoAttribute(writer1, TheClientID, "enabled", "False", false);
                }
                if (this.ValidationGroup.Length > 0)
                {
                    this.AddExpandoAttribute(writer1, TheClientID, "validationGroup", this.ValidationGroup);
                }

                base.AddAttributesToRender(writer);
            }
            finally
            {
                if (flag1)
                {
                    this.Enabled = false;
                }
            }
        }

        protected void CheckControlValidationProperty(string name, string propertyName)
        {
            // Get the control using the relative name.
            Control c = NamingContainer.FindControl(name);
            if (c == null)
            {
                throw new HttpException("Control not found.");
            }

            // Get  the control's validation property.
            PropertyDescriptor prop = GetValidationProperty(c);
            if (prop == null)
            {
                throw new HttpException("Control cannot be validated.");
            }
        }

        protected virtual bool ControlPropertiesValid()
        {
            // Determine whether the control to validate is blank.
            string controlToValidate = ControlToValidate;
            if (controlToValidate.Length == 0)
            {
                throw new HttpException("ControlToValidate cannot be blank.");
            }

            // Check that the property points to a valid control
            // (if not, an exception is thrown). 
            CheckControlValidationProperty(controlToValidate, "ControlToValidate");

            return true;
        }

        protected abstract bool EvaluateIsValid();

        protected string GetControlRenderID(string name)
        {
            // Get the control using the relative name.
            Control c = FindControl(name);
            if (c == null)
            {
                Debug.Fail("We should have already checked for the presence of this");
                return "";
            }
            return c.ClientID;
        }

        protected string GetControlToValidateRenderID()
        {
            string name = ControlToValidate;
            // Get the control using the relative name.
            Control c = FindControl(name);
            if (c == null)
            {
                Debug.Fail("We should have already checked for the presence of this");
                return "";
            }
            return c.ClientID;
        }

        protected string GetControlValidationValue(string name)
        {
            // Get the control using the relative name.
            Control c = NamingContainer.FindControl(name);
            if (c == null)
            {
                return null;
            }

            // Get the control's validation property.
            PropertyDescriptor prop = GetValidationProperty(c);
            if (prop == null)
            {
                return null;
            }

            // Get its value as a string.
            object value = prop.GetValue(c);
            if (value is ListItem)
            {
                return ((ListItem)value).Value;
            }
            else if (value != null)
            {
                return value.ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        public static PropertyDescriptor GetValidationProperty(object component)
        {
            ValidationPropertyAttribute valProp = (ValidationPropertyAttribute)TypeDescriptor.GetAttributes(component)[typeof(ValidationPropertyAttribute)];
            if (valProp != null && valProp.Name != null)
            {
                return TypeDescriptor.GetProperties(component, null)[valProp.Name];
            }
            return null;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Page.Validators.Add(this);
        }

        protected override void OnUnload(EventArgs e)
        {
            if (Page != null)
            {
                Page.Validators.Remove(this);
            }
            base.OnUnload(e);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            preRenderCalled = true;

            // Force a re-query of properties for render.
            propertiesChecked = false;

            RegisterValidatorCommonScript();
        }

        protected void RegisterValidatorCommonScript()
        {
            ClientScriptManager cs = Page.ClientScript;
            Type rsType = this.GetType();
            string location = null;

            // tooltips javascript include
            if (!cs.IsClientScriptIncludeRegistered(rsType, TooltipIncludeScriptKey))
            {
                location = cs.GetWebResourceUrl(rsType, "SAHL.Common.Web.UI.Controls.Resources.BalloonHints.js");
                cs.RegisterClientScriptInclude(rsType, TooltipIncludeScriptKey, location);
            }

            // validator javascript include
            if (!cs.IsClientScriptIncludeRegistered(rsType, ValidatorIncludeScriptKey))
            {
                location = cs.GetWebResourceUrl(rsType, "SAHL.Common.Web.UI.Controls.Resources.SAHLValidators.js");
                cs.RegisterClientScriptInclude(rsType, ValidatorIncludeScriptKey, location);
            }

            // Create the validator startup script block.
            if (!cs.IsStartupScriptRegistered(ValidatorIncludeScriptKey))
                cs.RegisterStartupScript(typeof(BaseValidator), "ValidatorIncludeScript", ValidatorStartupScript);
            
            // Create the onsubmit statement script block
            if(!cs.IsOnSubmitStatementRegistered(rsType, "ValidatorOnSubmit"))
                cs.RegisterOnSubmitStatement(rsType, "ValidatorOnSubmit", "if (typeof(ValidatorOnSubmit) == \"function\" && ValidatorOnSubmit() == false) return false;");
        }

        protected virtual void RegisterValidatorDeclaration()
        {
            ClientScriptManager cs = Page.ClientScript;
            Type rsType = this.GetType();
            string element = "document.getElementById(\"" + ClientID + "\")";
            cs.RegisterArrayDeclaration("Page_Validators", element);
        }

        public void Validate()
        {
            this.IsValid = true;
            if (this.Visible && this.Enabled)
            {
                this.propertiesChecked = false;
                if (this.PropertiesValid)
                {
                    this.IsValid = this.EvaluateIsValid();

                    // disallow setting focus for now
                    //if ((!this.IsValid && (this.Page != null)) && this.SetFocusOnError)
                    //{
                    //    this.SetValidatorInvalidControlFocus(this.ControlToValidate);
                    //}
                }
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            bool flag1;
            if (base.DesignMode || (!this.preRenderCalled && (this.Page == null)))
            {
                this.propertiesChecked = true;
                this.propertiesValid = true;
                flag1 = true;

                ClientScriptManager cs = Page.ClientScript;
                Type rsType = this.GetType();
                writer.AddAttribute(HtmlTextWriterAttribute.Src, cs.GetWebResourceUrl(rsType, "SAHL.Common.Web.UI.Controls.Resources.ValidatorInvalid.gif"));
                writer.AddAttribute(HtmlTextWriterAttribute.Id, ClientID);
                writer.RenderBeginTag(HtmlTextWriterTag.Img);
                return;
            }
            else
            {
                flag1 = this.Enabled && !this.IsValid;
            }
            if (this.PropertiesValid)
            {
                bool flag2;
                if (this.Page != null)
                {
                    this.Page.VerifyRenderingInServerForm(this);
                }
                ValidatorDisplay display1 = this.Display;

                flag2 = display1 != ValidatorDisplay.None;

                this.RegisterValidatorDeclaration();

                if ((display1 == ValidatorDisplay.None) || (!flag1 && (display1 == ValidatorDisplay.Dynamic)))
                {
                    base.Style["display"] = "none";
                }
                else if (!flag1)
                {
                    base.Style["visibility"] = "hidden";
                }

                AddAttributesToRender(writer);
                writer.RenderBeginTag(HtmlTextWriterTag.Span);

                if (flag2)
                {
                    ClientScriptManager cs = Page.ClientScript;
                    Type rsType = this.GetType();
                    writer.AddAttribute(HtmlTextWriterAttribute.Src, cs.GetWebResourceUrl(rsType, "SAHL.Common.Web.UI.Controls.Resources.ValidatorInvalid.gif"));
                    writer.RenderBeginTag(HtmlTextWriterTag.Img);
                    writer.RenderEndTag();
                }

                writer.RenderEndTag();
            }
        }
    }
}
