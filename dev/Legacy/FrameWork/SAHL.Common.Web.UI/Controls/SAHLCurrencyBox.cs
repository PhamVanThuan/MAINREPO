using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using AjaxControlToolkit;
using System.Globalization;

namespace SAHL.Common.Web.UI.Controls
{
    /// <summary>
    /// A text control for entering currency amounts.  This is a bit of a fudge - it actually hides itself 
    /// and inserts two new text boxes which the user actually sees.  These then transfer their data to the hidden 
    /// control - this is so we can manage to still do a Request.Form on the control and still get the correct 
    /// value (as opposed to the standard technique of using a composite control for layout).
    /// </summary>
    [ToolboxData("<{0}:SAHLCurrencyBox runat=server></{0}:SAHLDateBox>")]
    public class SAHLCurrencyBox : SAHLTextBox, INamingContainer
    {

        #region Private Attributes

        private SAHLTextBox _txtCents;
        private SAHLTextBox _txtRands;

        #endregion

        public SAHLCurrencyBox()
        {
            // set default values
            this.MaxLength = 7;
            this.Columns = 7;
            this.AllowNegative = false;
        }

        #region Overridden Methods

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            _txtRands = new SAHLTextBox();
            _txtRands.ID = "txtRands";
            _txtRands.MaxLength = this.MaxLength;
            _txtRands.Columns = this.Columns;
            _txtRands.DisplayInputType = InputType.Number;
            _txtRands.AutoCompleteType = AutoCompleteType.None;
            this.Controls.Add(_txtRands);

            Label lblPt = new Label();
            lblPt.Text = ".";
            lblPt.Style.Add(HtmlTextWriterStyle.PaddingLeft, "1px");
            lblPt.Style.Add(HtmlTextWriterStyle.PaddingRight, "1px");
            this.Controls.Add(lblPt);

            _txtCents = new SAHLTextBox();
            _txtCents.ID = "txtCents";
            _txtCents.MaxLength = 2;
            _txtCents.Columns = 2;
            _txtCents.DisplayInputType = InputType.Number;
            _txtCents.AutoCompleteType = AutoCompleteType.None;
            this.Controls.Add(_txtCents);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            _txtRands.AllowNegative = this.AllowNegative;
            _txtRands.Columns = this.Columns;
            _txtRands.CssClass = this.CssClass;
            _txtRands.Mandatory = this.Mandatory;
            _txtRands.MaxLength = this.MaxLength;
            _txtRands.ReadOnly = this.ReadOnly;
            _txtRands.TabIndex = this.TabIndex;
            _txtRands.TextAlign = this.TextAlign;
            _txtRands.Width = this.Width;

            _txtCents.CssClass = this.CssClass;
            _txtCents.Mandatory = this.Mandatory;
            _txtCents.ReadOnly = this.ReadOnly;
            _txtCents.TabIndex = this.TabIndex;

            // if the base text box has a value and the visibles ones don't, try and set their values - this can 
            // happen during design time or if the Amount property is set in the markup
            if (base.Text.Length > 0 && _txtRands.Text.Length == 0 && _txtCents.Text.Length == 0)
            {
                Amount = Double.Parse(base.Text);
            }

            // make sure this text box doesn't receive focus
            this.TabIndex = -1;

            // add the blur attributes
            string onBlur = String.Format("SAHLCurrencyBox_blur(this, '{0}', '{1}', '{2}');", this.ClientID, _txtRands.ClientID, _txtCents.ClientID);
            _txtCents.Attributes["onblur"] = onBlur + _txtCents.Attributes["onblur"];
            _txtRands.Attributes["onblur"] = onBlur + _txtRands.Attributes["onblur"];
            _txtCents.Attributes["onfocus"] = "this.select()";

            string onKeyDown = String.Format("SAHLCurrencyBox_keyDown(this, '{0}', '{1}');", _txtRands.ClientID, _txtCents.ClientID);
            _txtRands.Attributes["onkeydown"] = onKeyDown + _txtRands.Attributes["onkeydown"];

            // copy over any event handler attributes here
            CopyEventAttributes("onblur");
            CopyEventAttributes("onfocus");
            CopyEventAttributes("onkeydown");
            CopyEventAttributes("onkeypress");
            CopyEventAttributes("onkeyup");

            if (!DesignMode)
            {
                RegisterCommonScript();
            }

            // make sure this control is never displayed - it's the child controls only that need to be shown
            this.Style.Add(HtmlTextWriterStyle.Display, "none");


        }

        /// <summary>
        /// Registers common script files for the control.
        /// </summary>
        protected new void RegisterCommonScript()
        {
            base.RegisterCommonScript();

            ClientScriptManager cs = Page.ClientScript;
            Type type = typeof(SAHLDateBox);
            string url = null;

            if (!Page.ClientScript.IsClientScriptIncludeRegistered(type, "SAHLCurrencyBoxScript"))
            {
                // javascript include
                url = cs.GetWebResourceUrl(type, "SAHL.Common.Web.UI.Controls.Resources.SAHLCurrencyBox.js");
                cs.RegisterClientScriptInclude(type, "SAHLCurrencyBoxScript", url);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);
            if (Visible)
                RenderChildren(writer);
        }

        #endregion

        #region Private Methods

        private void CopyEventAttributes(string attributeName)
        {
            string eventValue = this.Attributes[attributeName];
            if (!String.IsNullOrEmpty(eventValue))
            {
                _txtRands.Attributes[attributeName] = eventValue + ";" + _txtRands.Attributes[attributeName];
                _txtCents.Attributes[attributeName] = eventValue + ";" + _txtCents.Attributes[attributeName];
            }
        }

        #endregion
        #region Public Properties

        /// <summary>
        /// Gets/sets the amount entered into the control.  
        /// </summary>
        /// <remarks>When setting, if the value has more than 2 decimal places, the value will be rounded off.</remarks>
        public double? Amount
        {
            get
            {
                double? d = new double?();
                if (base.Text.Length > 0)
                    d = new double?(Double.Parse(base.Text));

                return d;
            }
            set
            {
                if (value.HasValue)
                {
                    double real = Math.Floor(value.Value);
                    double fraction = value.Value - real;

                    string rands = real.ToString();
                    string cents = fraction.ToString("0.00").Substring(2);
                    base.Text = rands + "." + cents;
                    if (_txtRands != null)
                    {
                        _txtRands.Text = rands;
                        _txtCents.Text = cents;
                    }
                }
                else
                {
                    base.Text = "";
                    if (_txtRands != null)
                    {
                        _txtRands.Text = "";
                        _txtCents.Text = "";
                    }
                }
            }
        }

        /// <summary>
        /// Overridden to ensure that the text applied is a valie double.
        /// </summary>
        public override string Text
        {
            get
            {
                if (DesignMode)
                    return base.Text;
                else
                {
                    if (String.IsNullOrEmpty(_txtRands.Text) && String.IsNullOrEmpty(_txtCents.Text))
                        return String.Empty;
                    else
                        return _txtRands.Text + "." + _txtCents.Text;
                }
            }
            set
            {
                double d;
                if (Double.TryParse(value, out d))
                    this.Amount = new double?(d);
                else
                    this.Amount = new double?();
            }
        }

        #endregion
    }
}
