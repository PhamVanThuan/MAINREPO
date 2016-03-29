using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

public partial class ManageTransaction_ascx : System.Web.UI.UserControl
{
    public const string SHOW_LOAN_TRANSACTIONS = "n";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            pnlTransactionGrid.Visible = true;
            initialiseTranTypeDropDown();
        }
    }

    private void Page_Init(object sender, EventArgs e)
    {
        setDataSourceConnectionStr();
    }

    /// <summary>
    /// Set all SQLDataSource objects connection string properties.
    /// </summary>
    private void setDataSourceConnectionStr()
    {
        //Need to create a function in SAHLLib to allow DB specification. For now replace SAHLDB with 2am.
        string sCon = DBConnection.ConnectionString().Replace("SAHLDB", "2am");

        oFinServiceTypes.ConnectionString = sCon;
        oTransactionGroups.ConnectionString = sCon;
        oTransactions.ConnectionString = sCon;
    }

    protected void gvTransactions_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string sHTMLColor = DataBinder.Eval(e.Row.DataItem, "TransactionTypeHTMLColour").ToString();

            e.Row.Cells[7].ForeColor = Color.White;
            e.Row.Cells[7].BackColor = ColorTranslator.FromHtml(sHTMLColor);

            e.Row.Cells[1].Text = ""; // no rollback allowed

        }
    }

    /// <summary>
    /// Sets the default value of the Transaction group
    /// </summary>
    private void initialiseTranTypeDropDown()
    {
        ddlTransGroupTypes.SelectedValue = Convert.ToInt32(enums.etTranGroupType.Financial).ToString(); ;
    }

    protected void gvTransactions_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvTransactions.SelectedIndex = e.NewEditIndex;
        e.Cancel = true;
    }
}