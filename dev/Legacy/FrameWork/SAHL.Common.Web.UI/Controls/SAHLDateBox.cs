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
    /// A composite control for entering dates.  This is a combination of a TextBox, a MaskedEditExtender, a CalendarExtender 
    /// and an ImageButton.
    /// </summary>
    [DefaultProperty("Date")]
    [ToolboxData("<{0}:SAHLDateBox runat=server></{0}:SAHLDateBox>")]
    public class SAHLDateBox : SAHLTextBox
    {

        #region Private Attributes

        //private TextBox _txtInput = null;
        private MaskedEditExtender _maskExtender = null;
        private CalendarExtender _calendarExtender = null;
        private ImageButton _imageButton = null;

        #endregion

        #region Constructore

        public SAHLDateBox()
        {
            _maskExtender = new MaskedEditExtender();
            _maskExtender.CultureName = Constants.CultureGb;
            _imageButton = new ImageButton();
            _calendarExtender = new CalendarExtender();
        }

        #endregion


        #region Overridden Methods

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            Columns = 12;
            AutoCompleteType = AutoCompleteType.Disabled;

            // a masked edit is applied to the text box
            _maskExtender.ID = this.ClientID + "Mask";
            _maskExtender.TargetControlID = this.UniqueID;
            _maskExtender.ClearTextOnInvalid = false;
            _maskExtender.AutoComplete = false;
            _maskExtender.MaskType = MaskedEditType.Date;
            this.Controls.Add(_maskExtender);


            // add the calendar button - an image button is used so we get tab focus, but AutoPostback is turned off 
            // by returning false with the onclick attribute
            _imageButton.ID = this.ClientID + "Image";
            _imageButton.ImageUrl = Page.ClientScript.GetWebResourceUrl(this.GetType(), "SAHL.Common.Web.UI.Controls.Resources.calendar.png");
            _imageButton.Attributes.Add("onclick", "return false;");
            _imageButton.Style.Add(HtmlTextWriterStyle.MarginLeft, "5px");
            this.Controls.Add(_imageButton);

            // a calendar extender is applied to the same text box
            _calendarExtender.ID = this.ClientID + "Calendar";
            _calendarExtender.TargetControlID = this.UniqueID;
            _calendarExtender.Format = DateFormat;
            _calendarExtender.PopupButtonID = _imageButton.UniqueID;
            _calendarExtender.OnClientDateSelectionChanged = "SAHLDateBox_hideCalendar";
            _calendarExtender.OnClientShowing = "SAHLDateBox_showCalendar";
            _calendarExtender.EnabledOnClient = true;
            _calendarExtender.Animated = false;
            this.Controls.Add(_calendarExtender);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            _calendarExtender.Format = DateFormat;
            if (!DesignMode)
            {
                RegisterCommonScript();
                _maskExtender.Mask = DateFormat.ToLower().Replace("d", "9").Replace("m", "9").Replace("y", "9");
            }

        }

        /// <summary>
        /// Registers common script files for the control.
        /// </summary>
        protected new void RegisterCommonScript()
        {
            ClientScriptManager cs = Page.ClientScript;
            Type type = typeof(SAHLDateBox);
            string url = null;

            if (!Page.ClientScript.IsClientScriptIncludeRegistered(type, "SAHLDateBoxScript"))
            {
                // javascript include
                url = cs.GetWebResourceUrl(type, "SAHL.Common.Web.UI.Controls.Resources.SAHLDateBox.js");
                cs.RegisterClientScriptInclude(type, "SAHLDateBoxScript", url);

                // css include
                url = cs.GetWebResourceUrl(type, "SAHL.Common.Web.UI.Controls.Resources.SAHLDateBox.css");
                cs.RegisterClientScriptBlock(type, "SAHLDateBoxCss", "<link href=\"" + url + "\" type=\"text/css\" rel=\"stylesheet\">", false);

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

        #region Public Properties

        /// <summary>
        /// Gets/sets the culture name associated with the control, which is used to determine which 
        /// currency symbol is used.  This defaults to <see cref="Constants.CultureGb"/> (we use dd/mm/yyyy).
        /// </summary>
        public string CultureName
        {
            get
            {
                return _maskExtender.CultureName;
            }
            set
            {
                _maskExtender.CultureName = value;
            }
        }

        /// <summary>
        /// Gets/sets the date displayed by the control.  If a null date is supplied, the text box will be set to a zero-length string.
        /// </summary>
        [Category("Data")]
        public DateTime? Date
        {
            get
            {
                DateTime dtResult;
                // "dd/MM/yyyy"
                if (Text.Length > 0 && DateTime.TryParseExact(Text, DateFormat, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out dtResult))
                    return new Nullable<DateTime>(dtResult);
                else
                    return new Nullable<DateTime>();
            }
            set
            {
                if (value.HasValue)
                    Text = value.Value.ToString(DateFormat);
                else
                    Text = "";

                _calendarExtender.SelectedDate = value;
            }
        }

        /// <summary>
        /// Can be used to determine if a valid date has been entered.  This can also be determined by checking 
        /// if the <see cref="Date"/> value is null.
        /// </summary>
        [Category("Data")]
        [Browsable(false)]
        public bool DateIsValid
        {
            get
            {
                DateTime dt;
                return DateTime.TryParseExact(Text, DateFormat, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out dt);
            }
        }

        /// <summary>
        /// Gets the date format used on the control - this is determined by the current culture.
        /// </summary>
        public string DateFormat
        {
            get
            {
                return CultureInfo.GetCultureInfo(CultureName).DateTimeFormat.ShortDatePattern;
            }
        }

        #endregion
    }
}
