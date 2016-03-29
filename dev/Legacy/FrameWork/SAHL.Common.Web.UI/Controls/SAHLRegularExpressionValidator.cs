using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
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
    [ToolboxBitmap(typeof(SAHLRegularExpressionValidator), "Resources.SAHLRegularExpressionValidator.bmp")]
    [ToolboxData("<{0}:SAHLRegularExpressionValidator runat=server></{0}:SAHLRegularExpressionValidator>")]
    public class SAHLRegularExpressionValidator : SAHLBaseValidator
    {
        #region Properties

        [
        Editor("System.Common.Web.UI.Design.WebControls.RegexTypeEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor)), DefaultValue(""), 
        Category("Behavior"),
        Themeable(false)
        ]
        public string ValidationExpression
        {
            get
            {
                object obj1 = this.ViewState["ValidationExpression"];
                if (obj1 != null)
                {
                    return (string)obj1;
                }
                return string.Empty;
            }
            set
            {
                try
                {
                    Regex.IsMatch(string.Empty, value);
                }
                catch (Exception exception1)
                {
                    throw new HttpException("Validator_bad_regex", exception1);
                }
                this.ViewState["ValidationExpression"] = value;
            }
        }

        #endregion

        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            base.AddAttributesToRender(writer);

            string text1 = this.ClientID;
            HtmlTextWriter writer1 = null;
            base.AddExpandoAttribute(writer1, text1, "evaluationfunction", "RegularExpressionValidatorEvaluateIsValid", false);
            if (this.ValidationExpression.Length > 0)
            {
                base.AddExpandoAttribute(writer1, text1, "validationexpression", this.ValidationExpression);
            }
        }

        protected override bool EvaluateIsValid()
        {
            string text1 = base.GetControlValidationValue(base.ControlToValidate);
            if ((text1 == null) || (text1.Trim().Length == 0))
            {
                return true;
            }
            try
            {
                Match match1 = Regex.Match(text1, this.ValidationExpression);
                return ((match1.Success && (match1.Index == 0)) && (match1.Length == text1.Length));
            }
            catch
            {
                return true;
            }
        }

 


    }
}
