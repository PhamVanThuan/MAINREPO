using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using SAHLLib;

public partial class BankAccount_ascx : System.Web.UI.UserControl
{
    private DataSet oDS = new DataSet();

    void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ucToolbar.initializeToolbar(SAHLLib.Security.etFunction.LoanBankAccount);
            hideToolbarIfClosed();
        }

        setDataSourceConnectionStr();
        ucToolbar.btnSave.Click += new ImageClickEventHandler(save);
        bindData();

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
        string sCon = DBConnection.ConnectionString().Replace("SAHLDB","2am");
        
        oBankAccount.ConnectionString = sCon;
        oACBBank.ConnectionString = sCon;
        oACBBranch.ConnectionString = sCon;
        oACBType.ConnectionString = sCon;
    }


    /// <summary>
    /// Updates the database with the bank account changes specified by the user.
    /// </summary>
    private void save(object sender, ImageClickEventArgs e)
    {
        char[] caIllegalChars = {'.',','};

        Page.Validate();
        if (tbAccountNumber.Text.IndexOfAny(caIllegalChars) != -1)
        {
            rvAccountNumber.IsValid = false;
        }

        if (Page.IsValid)
        {
            oBankAccount.Update();
            oBankAccount.DataBind();
            ucToolbar.enableEdit(false);
        }
        
    }

    /// <summary>
    /// Sets the form controls with their database values.
    /// </summary>
    private void bindData()
    {
        if (pnlEdit.Visible == false)
        {
            DataSourceSelectArguments oDSSA = new DataSourceSelectArguments();
            DataView oDV = (DataView)oBankAccount.Select(oDSSA);

            

            ddAccountType.SelectedValue = oDV.Table.Rows[0]["ACBTypeNumber"].ToString();
            ddBank.SelectedValue = oDV.Table.Rows[0]["ACBBankCode"].ToString();
            oACBBranch.DataBind();
            ddBranch.DataBind();
            ddBranch.SelectedValue = oDV.Table.Rows[0]["ACBBranchCode"].ToString();
            tbAccountNumber.Text = oDV.Table.Rows[0]["AccountNumber"].ToString();
            tbBankAccKey.Value = oDV.Table.Rows[0]["BankAccountKey"].ToString();



        }
    }
}










  
