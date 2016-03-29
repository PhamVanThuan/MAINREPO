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

public partial class PrepaymentThresholds_ascx : System.Web.UI.UserControl
{
    public string PPThresholdYr1 = "";
    public string PPThresholdYr2 = "";
    public string PPThresholdYr3 = "";
    public string PPThresholdYr4 = "";
    public string PPThresholdYr5 = "";

    public string Cumulative2 = "";
    public string Cumulative3 = "";
    public string Cumulative4 = "";
    public string Cumulative5 = "";
    
    void Page_Load(object sender, EventArgs e)
    {
        GetPrepaymentThresholds();
    }

    /// <summary>
    /// Use DBHelper to get Prepayment Threshold info from DB
    /// </summary>
    private void GetPrepaymentThresholds()
    {
        string AccountKey = Request.QueryString["param0"];

        string Procedure = uiStatement.Get("LightHouse", "SuperLoGetPrepaymentThresholds");
        
        DBHelper db = new DBHelper(Procedure, CommandType.Text);

        db.AddIntParameter("AccountKey", AccountKey);
        db.Fill();
        DataRow dr = db.FirstRow();

        if (dr["PPThresholdYr1"] != DBNull.Value) { PPThresholdYr1 = Convert.ToDouble(dr["PPThresholdYr1"]).ToString(Constants.CURRENCY_FORMAT); }
        if (dr["PPThresholdYr2"] != DBNull.Value) { PPThresholdYr2 = Convert.ToDouble(dr["PPThresholdYr2"]).ToString(Constants.CURRENCY_FORMAT);}
        if (dr["PPThresholdYr3"] != DBNull.Value) { PPThresholdYr3 = Convert.ToDouble(dr["PPThresholdYr3"]).ToString(Constants.CURRENCY_FORMAT);}
        if (dr["PPThresholdYr4"] != DBNull.Value) { PPThresholdYr4 = Convert.ToDouble(dr["PPThresholdYr4"]).ToString(Constants.CURRENCY_FORMAT);}
        if (dr["PPThresholdYr5"] != DBNull.Value) { PPThresholdYr5 = Convert.ToDouble(dr["PPThresholdYr5"]).ToString(Constants.CURRENCY_FORMAT);}

        if (dr["Cumulative2"] != DBNull.Value) { Cumulative2 = Convert.ToDouble(dr["Cumulative2"]).ToString(Constants.CURRENCY_FORMAT); }
        if (dr["Cumulative3"] != DBNull.Value) { Cumulative3 = Convert.ToDouble(dr["Cumulative3"]).ToString(Constants.CURRENCY_FORMAT); }
        if (dr["Cumulative4"] != DBNull.Value) { Cumulative4 = Convert.ToDouble(dr["Cumulative4"]).ToString(Constants.CURRENCY_FORMAT); }
        if (dr["Cumulative5"] != DBNull.Value) { Cumulative5 = Convert.ToDouble(dr["Cumulative5"]).ToString(Constants.CURRENCY_FORMAT); }

        db = null;
    }
}
