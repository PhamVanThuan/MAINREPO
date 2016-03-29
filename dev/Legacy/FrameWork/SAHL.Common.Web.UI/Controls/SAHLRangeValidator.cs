using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

[assembly: TagPrefix("SAHL.Common.Web.UI.Controls", "SAHL")]
namespace SAHL.Common.Web.UI.Controls
{
    [ToolboxBitmap(typeof(SAHLRangeValidator), "Resources.SAHLRangeValidator.bmp")]
    [ToolboxData("<{0}:SAHLRangeValidator runat=server></{0}:SAHLRangeValidator>")]
    public class SAHLRangeValidator : SAHLBaseCompareValidator
    {
        #region Properties

        [
        Themeable(false), 
        Category("Behavior"), 
        DefaultValue("")
        ]
        public string MaximumValue
        {
            get
            {
                object obj1 = this.ViewState["MaximumValue"];
                if (obj1 != null)
                {
                    return (string)obj1;
                }
                return string.Empty;
            }
            set
            {
                this.ViewState["MaximumValue"] = value;
            }
        }

        [
        DefaultValue(""), 
        Themeable(false), 
        Category("Behavior")
        ]
        public string MinimumValue
        {
            get
            {
                object obj1 = this.ViewState["MinimumValue"];
                if (obj1 != null)
                {
                    return (string)obj1;
                }
                return string.Empty;
            }
            set
            {
                this.ViewState["MinimumValue"] = value;
            }
        }

        #endregion

        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            base.AddAttributesToRender(writer);

            string text1 = this.ClientID;
            HtmlTextWriter writer1 = null;
            base.AddExpandoAttribute(writer1, text1, "evaluationfunction", "RangeValidatorEvaluateIsValid", false);
            string text2 = this.MaximumValue;
            string text3 = this.MinimumValue;
            if (base.CultureInvariantValues)
            {
                text2 = base.ConvertCultureInvariantToCurrentCultureFormat(text2, base.Type);
                text3 = base.ConvertCultureInvariantToCurrentCultureFormat(text3, base.Type);
            }
            base.AddExpandoAttribute(writer1, text1, "maximumvalue", text2);
            base.AddExpandoAttribute(writer1, text1, "minimumvalue", text3);

        }

        protected override bool ControlPropertiesValid()
        {
            this.ValidateValues();
            return base.ControlPropertiesValid();
        }

        protected override bool EvaluateIsValid()
        {
            string text1 = base.GetControlValidationValue(base.ControlToValidate);
            if (text1.Trim().Length == 0)
            {
                return true;
            }
            if (((base.Type == ValidationDataType.Date) && !base.IsInStandardDateFormat(text1)))
            {
                text1 = base.ConvertToShortDateString(text1);
            }
            if (SAHLBaseCompareValidator.Compare(text1, false, this.MinimumValue, base.CultureInvariantValues, ValidationCompareOperator.GreaterThanEqual, base.Type))
            {
                return SAHLBaseCompareValidator.Compare(text1, false, this.MaximumValue, base.CultureInvariantValues, ValidationCompareOperator.LessThanEqual, base.Type);
            }
            return false;
        }

        private void ValidateValues()
        {
            string text1 = this.MaximumValue;
            if (!SAHLBaseCompareValidator.CanConvert(text1, base.Type, base.CultureInvariantValues))
            {
                throw new HttpException("Validator_value_bad_type");
            }
            string text2 = this.MinimumValue;
            if (!SAHLBaseCompareValidator.CanConvert(text2, base.Type, base.CultureInvariantValues))
            {
                throw new HttpException("Validator_value_bad_type");
            }
            if (SAHLBaseCompareValidator.Compare(text2, base.CultureInvariantValues, text1, base.CultureInvariantValues, ValidationCompareOperator.GreaterThan, base.Type))
            {
                throw new HttpException("Validator_range_overalap");
            }
        }

 
 
 


    }
}
