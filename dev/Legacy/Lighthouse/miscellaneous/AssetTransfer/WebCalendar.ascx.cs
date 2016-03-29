using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class WebCalendar : System.Web.UI.UserControl
{
    protected string _SelectedDate = "";

    public Calendar calendarDate
    {
        get
        {
            return calDate;
        }
    }

    public string SelectedDate
    {

        get { return _SelectedDate; }
        set
        {
            _SelectedDate = value;
            ViewState[this.ID + "_SelectedDate"] = value;
            txtDate.Text = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
        {

            _SelectedDate = ViewState[this.ID + "_SelectedDate"].ToString();
            txtDate.Text = _SelectedDate;
        }
        else
        {
            // First time
            InitCal();
            ViewState.Add(this.ID + "_SelectedDate", _SelectedDate);

        }
    }

    private void InitCal()
    {        
        calDate.Visible = false;
        if (_SelectedDate == "")
        {
            calDate.SelectedDate = DateTime.Now;
            _SelectedDate = DateTime.Now.ToString("dd/MM/yyyy"); // string.Format(DateTime.Now.ToShortDateString(), "dd/MM/yyyy");
            txtDate.Text = _SelectedDate;
        }
        else
        {
            try
            {
                calDate.SelectedDate = DateTime.Parse(_SelectedDate);
            }
            catch (Exception ex)
            {
                calDate.SelectedDate = DateTime.Now;
            }
        }

    }


    /// <summary>
    /// Shows or hides the calendar control.  Sets the date textbox with the selected date.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmdPick_Click(object sender, EventArgs e)
    {
     
        calDate.Visible = !calDate.Visible;
        txtDate.Text = _SelectedDate;

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void calDate_SelectionChanged(object sender, EventArgs e)
    {
        txtDate.Text = calDate.SelectedDate.ToString("dd/MM/yyyy");
        //txtDate.Text = calDate.SelectedDate.ToShortDateString();        
        //ViewState["SelectedDate"] = calDate.SelectedDate.ToShortDateString();
        ViewState[this.ID + "_SelectedDate"] = txtDate.Text;
        _SelectedDate = txtDate.Text;
        calDate.Visible = !calDate.Visible; 
    }

}
