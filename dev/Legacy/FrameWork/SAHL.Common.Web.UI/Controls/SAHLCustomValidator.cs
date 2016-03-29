using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Security.Permissions;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

[assembly: TagPrefix("SAHL.Common.Web.UI.Controls", "SAHL")]

namespace SAHL.Common.Web.UI.Controls
{
    [ToolboxBitmap(typeof(SAHLCustomValidator), "Resources.SAHLCustomValidator.bmp")]
    [DefaultEvent("ServerValidate"), ToolboxData("<{0}:SAHLCustomValidator runat=server></{0}:SAHLCustomValidator>"), AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal), AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class SAHLCustomValidator : SAHLBaseValidator
    {
        // Fields
        private static readonly object EventServerValidate;

        #region Properties

        [
        Themeable(false),
        Category("Behavior"),
        DefaultValue("")
        ]
        public string ClientValidationFunction
        {
            get
            {
                object obj1 = this.ViewState["ClientValidationFunction"];
                if (obj1 != null)
                {
                    return (string)obj1;
                }
                return string.Empty;
            }
            set
            {
                this.ViewState["ClientValidationFunction"] = value;
            }
        }

        [
        DefaultValue(false),
        Themeable(false),
        Category("Behavior")
        ]
        public bool ValidateEmptyText
        {
            get
            {
                object obj1 = this.ViewState["ValidateEmptyText"];
                if (obj1 != null)
                {
                    return (bool)obj1;
                }
                return false;
            }
            set
            {
                this.ViewState["ValidateEmptyText"] = value;
            }
        }

        public event ServerValidateEventHandler ServerValidate
        {
            add
            {
                base.Events.AddHandler(SAHLCustomValidator.EventServerValidate, value);
            }
            remove
            {
                base.Events.RemoveHandler(SAHLCustomValidator.EventServerValidate, value);
            }
        }

        #endregion Properties

        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            base.AddAttributesToRender(writer);

            string text1 = this.ClientID;
            HtmlTextWriter writer1 = null;
            base.AddExpandoAttribute(writer1, text1, "evaluationfunction", "CustomValidatorEvaluateIsValid", false);
            if (this.ClientValidationFunction.Length > 0)
            {
                base.AddExpandoAttribute(writer1, text1, "clientvalidationfunction", this.ClientValidationFunction);
                if (this.ValidateEmptyText)
                {
                    base.AddExpandoAttribute(writer1, text1, "validateemptytext", "true", false);
                }
            }
        }

        protected override bool ControlPropertiesValid()
        {
            string text1 = base.ControlToValidate;
            if (text1.Length > 0)
            {
                base.CheckControlValidationProperty(text1, "ControlToValidate");
            }
            return true;
        }

        protected override bool EvaluateIsValid()
        {
            string text1 = string.Empty;
            string text2 = base.ControlToValidate;
            if (text2.Length > 0)
            {
                text1 = base.GetControlValidationValue(text2);
                if (((text1 == null) || (text1.Trim().Length == 0)) && !this.ValidateEmptyText)
                {
                    return true;
                }
            }
            return this.OnServerValidate(text1);
        }

        protected virtual bool OnServerValidate(string value)
        {
            ServerValidateEventHandler handler1 = (ServerValidateEventHandler)base.Events[SAHLCustomValidator.EventServerValidate];
            ServerValidateEventArgs args1 = new ServerValidateEventArgs(value, true);
            if (handler1 != null)
            {
                handler1(this, args1);
                return args1.IsValid;
            }
            return true;
        }
    }
}