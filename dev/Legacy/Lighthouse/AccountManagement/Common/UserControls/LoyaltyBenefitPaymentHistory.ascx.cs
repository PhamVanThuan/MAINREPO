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

public partial class LoyaltyBenefitPaymentHistory_ascx : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        GetDataAndBind();
        if (!Page.IsPostBack)
        {
            lExit.NavigateUrl = HttpContext.Current.Request.ServerVariables["HTTP_REFERER"].ToString();
        }
    }

    private void GetDataAndBind()
    {

        bool bHasPayments = false;
        DataView dvPayments = new DataView();
        DataSet dsPayment = new DataSet();

        string AccountKey = Request.QueryString["param0"];

        DBHelper db = new DBHelper(uiStatement.Get("LightHouse", "LoanGetTransactions"), CommandType.StoredProcedure);

        db.AddIntParameter("accountKey", AccountKey);
        db.AddIntParameter("transactionTypeGroup", 6);
        db.AddIntParameter("financialServiceType", 1);
        db.AddVarcharParameter("returnArrearsTransactions", "N");
        db.Fill();
        dvPayments = db.Table(0).DefaultView;
        dvPayments.RowFilter = "TransactionTypeNumber=520"; //520 = LoyaltyBenefitPayment
        foreach (DataRowView drv in dvPayments)
        {
            bHasPayments = true;
        }
        if (bHasPayments)
        {
            gvTransactions.DataSource = dvPayments;
        }
        else
        {
            dsPayment.Tables.Add();
            foreach (DataColumn dc in dvPayments.Table.Columns)
	        {
                dsPayment.Tables[0].Columns.Add(dc.ColumnName);
	        }
            dsPayment.Tables[0].Rows.Add("");
            gvTransactions.DataSource = dsPayment;
        }

        gvTransactions.DataBind();
        db = null;

    }
}
