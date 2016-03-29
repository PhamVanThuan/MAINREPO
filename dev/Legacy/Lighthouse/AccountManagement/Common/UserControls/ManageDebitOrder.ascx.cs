using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHLLib;


public partial class ManageDebitOrder_ascx : System.Web.UI.UserControl
{
    void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ucToolbar.initializeToolbar(SAHLLib.Security.etFunction.LoanDebitOrder);
            hideToolbarIfClosed();
        }

        setDataSourceConnectionStr();
        ucToolbar.btnSave.Click += new ImageClickEventHandler(save);
        bindData();

    }

    void save(object sender, ImageClickEventArgs e)
    {
        Page.Validate();

        if (Page.IsValid)
        {
            oDebitOrder.Update();
            oDebitOrder.DataBind();
            ucToolbar.enableEdit(false);
        }
    }

    void hideToolbarIfClosed()
    {
        Account oAcc = new Account();
        int iAccountKey = Convert.ToInt32(Request.QueryString["param0"]);
        if (!oAcc.Editable(iAccountKey))
        {
            ucToolbar.Visible = false;
        }

    }

    /// <summary>
    /// Set all SQLDataSource objects connection string properties.
    /// </summary>
    private void setDataSourceConnectionStr()
    {
        //Need to create a function in SAHLLib to allow DB specification. For now replace SAHLDB with 2am.
        string sCon = DBConnection.ConnectionString().Replace("SAHLDB", "2am");

        oDebitOrder.ConnectionString = sCon;
    }

    
    /// <summary>
    /// Sets the form controls with their database values.
    /// </summary>
    private void bindData()
    {
        if (pnlEdit.Visible == false)
        {
            DataSourceSelectArguments oDSSA = new DataSourceSelectArguments();

            DataView oDV = (DataView)oDebitOrder.Select(oDSSA);

           // tbAccountNumber.Text = oDV.Table.Rows[0]["AccountNumber"].ToString();
            tbDebitOrderAmount.Text = oDV.Table.Rows[0]["fixedpayment"].ToString();
            ddDebitOrderDay.SelectedValue = oDV.Table.Rows[0]["DebitOrderDay"].ToString();
            tbSuretorName.Text = oDV.Table.Rows[0]["LoanSuretorName"].ToString();
            tbSuretorID.Text = oDV.Table.Rows[0]["LoanSuretorIDNumber"].ToString();

        }
    }

}
