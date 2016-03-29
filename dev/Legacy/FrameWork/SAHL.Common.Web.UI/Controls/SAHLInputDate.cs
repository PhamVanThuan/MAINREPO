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
    [ToolboxBitmap(typeof(Calendar))]
    [ToolboxData("<{0}:SAHLInputDate runat=server></{0}:SAHLInputDate>")]
    [DefaultProperty("Text")]
    [ValidationProperty("Text")]
    [Obsolete("Please use SAHLDateBox instead.", false)]
    public class SAHLInputDate : SAHLWebControl
    {
        private DateTime m_Date;
        private string m_Day;
        private string m_Month;
        private string m_Year;
        private bool m_IsDateValid = true;
        private bool m_LockTextInput = false;
        private bool m_DateFixed = false;

        private DateTime m_DefaultDate = new DateTime(1900, 01, 01);

        public string DateString
        {
            get
            {
                String s = (String)ViewState["DateString"];
                return ((s == null) ? String.Empty : s);
            }

            set
            {
                ViewState["DateString"] = value;
            }
        }

        [Bindable(true)]
        [Category("Data")]
        [DefaultValue(true)]
        [Localizable(true)]
        public bool IsDateValid
        {
            get { return m_IsDateValid; }
            set { m_IsDateValid = value; }
        }

        [Bindable(true)]
        [Category("Data")]
        [DefaultValue(false)]
        [Localizable(true)]
        public bool LockTextInput
        {
            get { return m_LockTextInput; }
            set { m_LockTextInput = value; }
        }

        [Bindable(true)]
        [Category("Data")]
        [DefaultValue("")]
        [Localizable(true)]
        public string Text
        {
            get
            {
                String s = (String)ViewState["DateString"];
                return ((s == null) ? String.Empty : s);
            }
        }

        [Localizable(true)]
        public DateTime DefaultDate
        {
            get
            {
                return m_DefaultDate;
            }
        }

        [Bindable(true)]
        [Category("Data")]
        [DefaultValue("")]
        [Localizable(true)]
        public DateTime Date
        {
            get
            {
                if (ViewState["Date"] == null)
                    m_Date = m_DefaultDate;
                else
                    m_Date = (DateTime)ViewState["Date"];
                return m_Date;
            }
            set
            {
                ViewState["Date"] = value;
                m_Date = value;
                DateString = m_Date.ToString("dd/MM/yyyy");
            }
        }

        [Bindable(true)]
        [Category("Data")]
        [DefaultValue(false)]
        [Localizable(true)]
        public bool DateFixed
        {
            get { return m_DateFixed; }
            set { m_DateFixed = value; }
        }

        public void ClearDate()
        {
            this.Date = m_DefaultDate;
            DateString = string.Empty;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!this.DesignMode)
            {
                // calendar include
                string location = this.Page.ClientScript.GetWebResourceUrl(this.GetType(), "SAHL.Common.Web.UI.Controls.Resources.SAHLInputDate.css");
                HtmlLink cssHtmlLink = new HtmlLink();
                cssHtmlLink.Href = location;
                cssHtmlLink.Attributes.Add("rel", "stylesheet");
                cssHtmlLink.Attributes.Add("type", "text/css");
                Page.Header.Controls.Add(cssHtmlLink);

                if (Page.Request.Form[this.UniqueID + "DD"] != null)
                {
                    // Check the postback values if they exist and update.
                    string d = Page.Request.Form[this.UniqueID + "DD"].ToString();
                    string m = Page.Request.Form[this.UniqueID + "MM"].ToString();
                    string y = Page.Request.Form[this.UniqueID + "YY"].ToString();

                    if (d.Equals("DD") && m.Equals("MM") && y.Equals("YYYY"))
                    {
                        d = m_DefaultDate.Day.ToString("00");
                        m = m_DefaultDate.Month.ToString("00");
                        y = m_DefaultDate.Year.ToString("00");
                    }
                    else
                    {
                        // If any of the following are missing, set it to current day, month or year.
                        if (d.IndexOf("D") != -1 || d.Length == 0)
                        {
                            d = DateTime.Now.Day.ToString("00");
                            m_IsDateValid = false;
                        }
                        if (m.IndexOf("M") != -1 || m.Length == 0)
                        {
                            m = DateTime.Now.Month.ToString("00");
                            m_IsDateValid = false;
                        }
                        if (y.IndexOf("Y") != -1 || y.Length == 0)
                        {
                            y = DateTime.Now.Year.ToString("0000");
                            m_IsDateValid = false;
                        }

                        // Check that the values entered ar acceptable.
                        if (int.Parse(y) < 1900)
                        {
                            y = "1900";
                            m_DateFixed = true;
                        }
                        if (int.Parse(y) > 3000)
                        {
                            y = "3000";
                            m_DateFixed = true;
                        }
                        if (int.Parse(m) > 12)
                        {
                            m = "12";
                            m_DateFixed = true;
                        }

                        switch (m)
                        {
                            case "01":
                            case "03":
                            case "05":
                            case "07":
                            case "08":
                            case "10":
                            case "12":
                                if (int.Parse(d) > 31)
                                {
                                    d = "31";
                                    m_DateFixed = true;
                                }
                                break;
                            case "02":
                                int fDays = daysInFebruary(int.Parse(y));
                                if (int.Parse(d) > fDays)
                                {
                                    d = fDays.ToString();
                                    m_DateFixed = true;
                                }
                                break;
                            case "04":
                            case "06":
                            case "09":
                            case "11":
                                if (int.Parse(d) > 30)
                                {
                                    d = "30";
                                    m_DateFixed = true;
                                }
                                break;
                        }
                    }

                    try
                    {
                        DateTime dt = new DateTime(int.Parse(y), int.Parse(m), int.Parse(d));
                        this.Date = dt;
                        DateString = dt.ToString("dd/MM/yyyy");
                        if (DateString.Equals(m_DefaultDate.Day.ToString("00") + "/" + m_DefaultDate.Month.ToString("00") + "/" + m_DefaultDate.Year.ToString("0000")))
                            DateString = string.Empty;
                    }
                    catch (ArgumentOutOfRangeException range)
                    {
                        string s = range.Message;
                    }
                    catch (ArgumentException ex)
                    {
                        string s = ex.Message;
                    }
                }
            }
        }

        private int daysInFebruary(int year)
        {
            return (((year % 4 == 0) && ( (!(year % 100 == 0)) || (year % 400 == 0))) ? 29 : 28 );
        }

        protected override void Render(HtmlTextWriter output)
        {
            string textClass;
            if (DateString == string.Empty || DateString.Equals(m_DefaultDate.Day.ToString("00") + "/" + m_DefaultDate.Month.ToString("00") + "/" + m_DefaultDate.Year.ToString("0000")))
            {
                m_Day = "DD";
                m_Month = "MM";
                m_Year = "YYYY";
                textClass = "DateGrey";
            }
            else
            {
                m_Day = m_Date.Day.ToString("00");
                m_Month = m_Date.Month.ToString("00");
                m_Year = m_Date.Year.ToString(); ;
                textClass = "DateNorm";
            }

            Table table = new Table();
            table.ID = this.UniqueID + "GROUP";
            table.CellSpacing = 0;
            table.CellPadding = 0;
            table.CssClass = "DateBox";

            TableRow row = new TableRow();
            TableCell cell = new TableCell();

            TextBox textbox = new TextBox();
            textbox.ID = this.UniqueID + "DD";
            textbox.Text = m_Day;
            textbox.CssClass = textClass;
            textbox.MaxLength = 2;
            textbox.Attributes.Add("size", "2");
            if (Enabled)
            {
                if (m_LockTextInput)
                    textbox.Attributes["readonly"] = "true";
                else
                {
                    textbox.Attributes.Add("onfocus", "dateFocus(this,'DD');");
                    textbox.Attributes.Add("onblur", "dateBlur(this,'DD');");
                    textbox.Attributes.Add("onkeypress", "NumOnly();");
                    textbox.Attributes.Add("onkeyup", "dateKeyUp(this,'DD');");
                }
            }
            else
                textbox.Enabled = false;
            cell.Controls.Add(textbox);

            Literal literal = new Literal();
            literal.Text = "/";
            cell.Controls.Add(literal);
            cell.VerticalAlign = VerticalAlign.Bottom;

            textbox = new TextBox();
            textbox.ID = this.UniqueID + "MM";
            textbox.Text = m_Month;
            textbox.CssClass = textClass;
            textbox.MaxLength = 2;
            textbox.Attributes.Add("size", "2");
            if (Enabled)
            {
                if (m_LockTextInput)
                    textbox.Attributes["readonly"] = "true";
                else
                {
                    textbox.Attributes.Add("onfocus", "dateFocus(this,'MM');");
                    textbox.Attributes.Add("onblur", "dateBlur(this,'MM');");
                    textbox.Attributes.Add("onkeypress", "NumOnly();");
                    textbox.Attributes.Add("onkeyup", "dateKeyUp(this,'MM');");
                }
            }
            else
                textbox.Enabled = false;
            cell.Controls.Add(textbox);

            literal = new Literal();
            literal.Text = "/";
            cell.Controls.Add(literal);
            cell.VerticalAlign = VerticalAlign.Bottom;

            textbox = new TextBox();
            textbox.ID = this.UniqueID + "YY";
            textbox.Text = m_Year;
            textbox.CssClass = textClass;
            textbox.MaxLength = 4;
            textbox.Attributes.Add("size", "4");
            if (Enabled)
            {
                if (m_LockTextInput)
                    textbox.Attributes["readonly"] = "true";
                else
                {
                    textbox.Attributes.Add("onfocus", "dateFocus(this,'YYYY');");
                    textbox.Attributes.Add("onblur", "dateBlur(this,'YYYY');");
                    textbox.Attributes.Add("onkeypress", "NumOnly();");
                    textbox.Attributes.Add("onkeyup", "dateKeyUp(this,'YYYY');");
                }
            }
            else
                textbox.Enabled = false;
            cell.Controls.Add(textbox);

            //cell.VerticalAlign = VerticalAlign.Bottom;
            row.Cells.Add(cell);


            cell = new TableCell();
            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
            img.ID = this.UniqueID + "IMG";
            string location = this.Page.ClientScript.GetWebResourceUrl(this.GetType(), "SAHL.Common.Web.UI.Controls.Resources.Calendar.jpg");
            img.ImageUrl = location;// "~/Images/Calendar.jpg"; // CHANGED BY E.D. TO BE AN IMBEDDED RESOURCE
            if (Enabled)
                img.Style["cursor"] = "hand";
            cell.Controls.Add(img);

// E.D., WHY IS THIS A TEXTBOX, SURELY A HIDDENFIELD WOULD BE MORE APPROPRIATE??????????
            TextBox txtHidden = new TextBox();
            //HiddenField hidden = new HiddenField();
            txtHidden.ID = this.UniqueID;
            if (m_Day.Equals("DD") && m_Month.Equals("MM") && m_Year.Equals("YYYY"))
                txtHidden.Text = DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;
            else
                txtHidden.Text = m_Day + "/" + m_Month + "/" + m_Year;
            txtHidden.CausesValidation = false;
            if (Enabled)
            {
// CHANGED BY E.D., THIS IS CALLED DIRCETLY FROM THE JS FILE AS THE DATE CHANGES, HOOKING ONCHANGE CAUSES THE VALIDATORS
// TO BLOW UP BADLY AS IT IS CALLED MANUALLY FROM THE CALENDAR WITH THE INCORRECT WINDOW.SRCELEMENT              
//                txtHidden.Attributes.Add("onchange", "updateDateBoxes(this);");
            }
            else
                txtHidden.Enabled = false;
            txtHidden.Style["display"] = "none";
            cell.Controls.Add(txtHidden);
            row.Cells.Add(cell);

            table.Rows.Add(row);

            StringBuilder script = new StringBuilder();
            script.AppendLine("Calendar.setup(");
            script.AppendLine("{");
            script.AppendLine("    inputField : \"" + this.ClientID + "\",");
            script.AppendLine("    ifFormat : \"%d/%m/%Y\",");
            script.AppendLine("    button : \"" + this.ClientID + "IMG\"");
            script.AppendLine("});");

            Page.ClientScript.RegisterStartupScript(this.GetType(), this.UniqueID, script.ToString(), true);

            table.RenderControl(output);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            RegisterCommonScript();
        }

        protected void RegisterCommonScript()
        {
            ClientScriptManager cs = Page.ClientScript;
            Type rsType = typeof(SAHLInputDate);
            string location = null;

            // calendar include
            if (!cs.IsClientScriptIncludeRegistered(rsType, "SAHLDateInput"))
            {
                location = cs.GetWebResourceUrl(rsType, "SAHL.Common.Web.UI.Controls.Resources.SAHLInputDate.js");
                cs.RegisterClientScriptInclude(rsType, "SAHLDateInput", location);
            }
        }
    }
}
