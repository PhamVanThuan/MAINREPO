using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using System.Text;

[assembly: TagPrefix("SAHL.Common.Web.UI.Controls", "SAHL")]
namespace SAHL.Common.Web.UI.Controls
{
    [ToolboxBitmap(typeof(SAHLRequiredFieldValidator), "Resources.SAHLRequiredFieldValidator.bmp")]
    [ToolboxData("<{0}:SAHLRequiredFieldValidator runat=server></{0}:SAHLRequiredFieldValidator>")]
    public class SAHLRequiredFieldValidator : SAHLBaseValidator
    {
        public SAHLRequiredFieldValidator() : base()
        {
            ErrorMessage = "SAHLRequiredFieldValidator";
        }

        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            base.AddAttributesToRender(writer);
            base.AddExpandoAttribute(null, ClientID, "evaluationfunction", "RequiredFieldValidatorEvaluateIsValid", false);
            base.AddExpandoAttribute(null, ClientID, "initialvalue", this.InitialValue);
        }

        protected override bool EvaluateIsValid()
        {
            string text1 = base.GetControlValidationValue(base.ControlToValidate);
            if (text1 == null)
            {
                return true;
            }
            return !text1.Trim().Equals(this.InitialValue.Trim());

        }
   
    }
}
