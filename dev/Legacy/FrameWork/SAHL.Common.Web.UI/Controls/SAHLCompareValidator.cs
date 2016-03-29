using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Diagnostics;
using System.Globalization;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;

[assembly: TagPrefix("SAHL.Common.Web.UI.Controls", "SAHL")]
namespace SAHL.Common.Web.UI.Controls
{
    [ToolboxBitmap(typeof(SAHLCompareValidator), "Resources.SAHLCompareValidator.bmp")]
    [ToolboxData("<{0}:SAHLCompareValidator runat=server></{0}:SAHLCompareValidator>")]
    public class SAHLCompareValidator : SAHLBaseCompareValidator
    {

        #region Properties

        [
        TypeConverter(typeof(ValidatedControlConverter)), 
        Category("Behavior"), Themeable(false), 
        DefaultValue(""), 
        Description("CompareValidator_ControlToCompare")
        ]
        public string ControlToCompare
        {
            get
            {
                object obj1 = this.ViewState["ControlToCompare"];
                if (obj1 != null)
                {
                    return (string)obj1;
                }
                return string.Empty;
            }
            set
            {
                this.ViewState["ControlToCompare"] = value;
            }
        }

        [
        DefaultValue(0), 
        Themeable(false), 
        Category("Behavior")
        ]
        public ValidationCompareOperator Operator
        {
            get
            {
                object obj1 = this.ViewState["Operator"];
                if (obj1 != null)
                {
                    return (ValidationCompareOperator)obj1;
                }
                return ValidationCompareOperator.Equal;
            }
            set
            {
                if ((value < ValidationCompareOperator.Equal) || (value > ValidationCompareOperator.DataTypeCheck))
                {
                    throw new ArgumentOutOfRangeException("value");
                }
                this.ViewState["Operator"] = value;
            }
        }

        [
        Category("Behavior"), 
        Themeable(false), 
        DefaultValue("")
        ]
        public string ValueToCompare
        {
            get
            {
                object obj1 = this.ViewState["ValueToCompare"];
                if (obj1 != null)
                {
                    return (string)obj1;
                }
                return string.Empty;
            }
            set
            {
                this.ViewState["ValueToCompare"] = value;
            }
        }

        #endregion


        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            base.AddAttributesToRender(writer);

            string text1 = this.ClientID;
            HtmlTextWriter writer1 = null;
            base.AddExpandoAttribute(writer1, text1, "evaluationfunction", "CompareValidatorEvaluateIsValid", false);
            if (this.ControlToCompare.Length > 0)
            {
                string text2 = base.GetControlRenderID(this.ControlToCompare);
                base.AddExpandoAttribute(writer1, text1, "controltocompare", text2);
                base.AddExpandoAttribute(writer1, text1, "controlhookup", text2);
            }
            if (this.ValueToCompare.Length > 0)
            {
                string text3 = this.ValueToCompare;
                if (base.CultureInvariantValues)
                {
                    text3 = base.ConvertCultureInvariantToCurrentCultureFormat(text3, base.Type);
                }
                base.AddExpandoAttribute(writer1, text1, "valuetocompare", text3);
            }
            if (this.Operator != ValidationCompareOperator.Equal)
            {
                base.AddExpandoAttribute(writer1, text1, "operator", PropertyConverter.EnumToString(typeof(ValidationCompareOperator), this.Operator), false);
            }
        }

        protected override bool ControlPropertiesValid()
        {
            if (this.ControlToCompare.Length > 0)
            {
                base.CheckControlValidationProperty(this.ControlToCompare, "ControlToCompare");
                if (EqualsIgnoreCase(base.ControlToValidate, this.ControlToCompare))
                {
                    throw new HttpException("Validator_bad_compare_control");
                }
            }
            else if ((this.Operator != ValidationCompareOperator.DataTypeCheck) && !BaseCompareValidator.CanConvert(this.ValueToCompare, base.Type, base.CultureInvariantValues))
            {
                throw new HttpException("Validator_value_bad_type");
            }
            return base.ControlPropertiesValid();
        }

        internal static bool EqualsIgnoreCase(string s1, string s2)
        {
            if (string.IsNullOrEmpty(s1) && string.IsNullOrEmpty(s2))
            {
                return true;
            }
            if (string.IsNullOrEmpty(s1) || string.IsNullOrEmpty(s2))
            {
                return false;
            }
            if (s2.Length != s1.Length)
            {
                return false;
            }
            return (0 == string.Compare(s1, 0, s2, 0, s2.Length, StringComparison.OrdinalIgnoreCase));
        }

 

        protected override bool EvaluateIsValid()
        {
            string text1 = base.GetControlValidationValue(base.ControlToValidate);
            if (text1.Trim().Length == 0)
            {
                return true;
            }
            bool flag1 = (base.Type == ValidationDataType.Date);
            if (flag1 && !base.IsInStandardDateFormat(text1))
            {
                text1 = base.ConvertToShortDateString(text1);
            }
            bool flag2 = false;
            string text2 = string.Empty;
            if (this.ControlToCompare.Length > 0)
            {
                text2 = base.GetControlValidationValue(this.ControlToCompare);
                if (flag1 && !base.IsInStandardDateFormat(text2))
                {
                    text2 = base.ConvertToShortDateString(text2);
                }
            }
            else
            {
                text2 = this.ValueToCompare;
                flag2 = base.CultureInvariantValues;
            }
            return SAHLBaseCompareValidator.Compare(text1, false, text2, flag2, this.Operator, base.Type);
        }
    }
}
