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

public partial class ManageHOC_ascx : System.Web.UI.UserControl
{
    void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ucToolbar.initializeToolbar(SAHLLib.Security.etFunction.LoanHOC);
            ibManageHOC.PostBackUrl = ConfigurationSettings.AppSettings["HOCManagementPath"] + "?Source=PLUGIN&Action=Update&Number=" + Request.QueryString["param0"];
            ibManageHOC.Visible = Security.editAllowed(SAHLLib.Security.etFunction.LoanHOC);
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
            ibManageHOC.Visible = false;
        }

    }

    /// <summary>
    /// Set all SQLDataSource objects connection string properties.
    /// </summary>
    private void setDataSourceConnectionStr()
    {
        //Need to create a function in SAHLLib to allow DB specification. For now replace SAHLDB with 2am.
        string sCon = DBConnection.ConnectionString().Replace("SAHLDB", "2am");

        oHOCInsurer.ConnectionString = sCon;
        oHOCSubsidence.ConnectionString = sCon;
        oHOCConstruction.ConnectionString = sCon;
        oHOCRoofType.ConnectionString = sCon;
        oHOC.ConnectionString = sCon;
    }


    /// <summary>
    /// Updates the database with the bank account changes specified by the user.
    /// </summary>
    private void save(object sender, ImageClickEventArgs e)
    {
        Page.Validate();
        if (Page.IsValid)
        {
            oHOC.Update();
            ucToolbar.enableEdit(false);
        }
        else
        {
            lbErrorMsg.Visible = true;
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

            DataView oDV = (DataView)oHOC.Select(oDSSA);

            ddInsurer.SelectedValue = oDV.Table.Rows[0]["HOCInsurerNumber"].ToString();
            ddSubsidence.SelectedValue = oDV.Table.Rows[0]["HOCSubsidenceNumber"].ToString();
            ddConstruction.SelectedValue = oDV.Table.Rows[0]["HOCConstructionNumber"].ToString();
            ddRoofType.SelectedValue = oDV.Table.Rows[0]["HOCRoofNumber"].ToString();

            tbPolicyNumber.Text = oDV.Table.Rows[0]["HOCPolicyNumber"].ToString();
            tbPremium.Text = oDV.Table.Rows[0]["HOCMonthlyPremium"].ToString();
            tbTotSumInsured.Text = oDV.Table.Rows[0]["HOCTotalSumInsured"].ToString();
            tbThatchAmount.Text = oDV.Table.Rows[0]["HOCThatchAmount"].ToString();
            tbConvAmount.Text = oDV.Table.Rows[0]["HOCConventionalAmount"].ToString();
            tbShingleAmount.Text = oDV.Table.Rows[0]["HOCShingleAmount"].ToString();


        }
    }

}
