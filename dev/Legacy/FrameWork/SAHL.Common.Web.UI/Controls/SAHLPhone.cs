using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

[assembly: TagPrefix("SAHL.Common.Web.UI.Controls", "SAHL")]
namespace SAHL.Common.Web.UI.Controls
{
    [ToolboxBitmap(typeof(TextBox))]
    [ToolboxData("<{0}:SAHLPhone runat=server></{0}:SAHLPhone>")]
    [ValidationProperty("PhoneString")]
    public class SAHLPhone : SAHLWebControl, INamingContainer
    {
        private SAHLTextBox _txtCode;
        private SAHLTextBox _txtNumber;
        private string _onClientKeyUp = "";

        public SAHLPhone()
        {
            _txtCode = new SAHLTextBox();
            _txtCode.ID = this.ID + "_CODE";
            _txtCode.Columns = 3;
            _txtCode.MaxLength = _txtCode.Columns;
            _txtCode.Style["padding-left"] = "1px";
            _txtCode.DisplayInputType = InputType.Number;
            this.Controls.Add(_txtCode);

            HtmlGenericControl separator = new HtmlGenericControl("span");
            separator.InnerText = "-";
            separator.Style.Add(HtmlTextWriterStyle.Padding, "0px 3px 0px 3px");
            this.Controls.Add(separator);

            _txtNumber = new SAHLTextBox();
            _txtNumber.ID = this.ID + "_NUMB";
            _txtNumber.Columns = 7;
            _txtNumber.MaxLength = _txtNumber.Columns;
            _txtNumber.DisplayInputType = InputType.Number;
            this.Controls.Add(_txtNumber);

        }

        [Bindable(true)]
        [Category("Data")]
        [DefaultValue("false")]
        [Localizable(true)]
        public bool ReadOnly
        {
            get
            {
                return _txtCode.ReadOnly;
            }
            set
            {
                _txtCode.ReadOnly = value;
                _txtNumber.ReadOnly = value;
            }
        }


        [Bindable(true)]
        [Category("Data")]
        [DefaultValue("")]
        [Localizable(true)]
        public string PhoneString
        {
            get
            {
                if (Code.Length == 0 && Number.Length == 0)
                    return "";
                else
                    return Code + "-" + Number;
            }
        }

        [Bindable(true)]
        [Category("Data")]
        [DefaultValue("")]
        [Localizable(true)]
        public string Code
        {
            get
            {
                return _txtCode.Text;
            }

            set
            {
                _txtCode.Text = value;
            }
        }

        public override string CssClass
        {
            get
            {
                return _txtCode.CssClass;
            }
            set
            {
                _txtCode.CssClass = value;
                _txtNumber.CssClass = value;
            }
        }

        [Bindable(true)]
        [Category("Data")]
        [DefaultValue("")]
        [Localizable(true)]
        public string Number
        {
            get
            {
                return _txtNumber.Text;
            }

            set
            {
                _txtNumber.Text = value;
            }
        }

        [Bindable(true)]
        [Category("Data")]
        [DefaultValue(3)]
        [Localizable(true)]
        public int CodeLength
        {
            get
            {
                return _txtCode.Columns;
            }

            set
            {
                _txtCode.Columns = value;
            }
        }

        [Bindable(true)]
        [Category("Data")]
        [DefaultValue(7)]
        [Localizable(true)]
        public int NumberLength
        {
            get
            {
                return _txtNumber.Columns;
            }

            set
            {
                _txtNumber.Columns = value;
            }
        }

        /// <summary>
        /// Gets/sets a javascript value that will be executed on a key up event - this will only fire if a valid 
        /// is pressed.
        /// </summary>
        public string OnClientKeyUp
        {
            get
            {
                return _onClientKeyUp;
            }
            set
            {
                _onClientKeyUp = value;
            }
        }

        public void ClearNumber()
        {
            Code = "";
            Number = "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
           base.OnInit(e);
       }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!DesignMode)
            {
                RegisterCommonScript();

                if (this.ReadOnly == false)
                {
                    string onKeyUp = "SAHLPhone_keyUp(this,'{0}',{1},'" + ControlHelpers.CleanJavaScript(OnClientKeyUp) + "');";
                    _txtCode.Attributes.Add("onkeyup", String.Format(onKeyUp, "CODE", CodeLength));
                    _txtNumber.Attributes.Add("onkeyup", String.Format(onKeyUp, "NUMB", NumberLength));
                }
            }

            // set the tab index on the child controls - this implementation may seem a little odd but use base.TabIndex 
            // doesn't work because of the way .NET applies this property to the child controls of a container - this 
            // was the only way of consistently ensuring the parent doesn't get a tabindex and the children do
            string tabIndex = this.TabIndex.ToString();
            this.TabIndex = -1;
            _txtCode.Attributes.Add("tabindex", tabIndex);
            _txtNumber.Attributes.Add("tabindex", tabIndex);
        }

        /// <summary>
        /// Registers common script files for the control.
        /// </summary>
        protected void RegisterCommonScript()
        {
            ClientScriptManager cs = Page.ClientScript;
            Type type = typeof(SAHLPhone);
            string url = null;

            if (!Page.ClientScript.IsClientScriptIncludeRegistered(type, "SAHLPhoneScript"))
            {
                // javascript include
                url = cs.GetWebResourceUrl(type, "SAHL.Common.Web.UI.Controls.Resources.SAHLPhone.js");
                cs.RegisterClientScriptInclude(type, "SAHLPhoneScript", url);
            }
        }

    }
}
