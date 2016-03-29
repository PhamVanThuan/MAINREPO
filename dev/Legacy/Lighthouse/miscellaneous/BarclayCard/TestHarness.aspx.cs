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
using System.Data.SqlClient;

public partial class BarclayCard_TestHarness : System.Web.UI.Page
{
    private SqlConnection oConn = new SqlConnection();

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            //this.txtLoanNumber.Text = "1362572"; // Loan excample
            //this.txtLoanNumber.Text = "273969"; // Prospect example
            //chkProspect.Checked = true;
            this.txtLoanNumber.Text = "1277899"; // Unregistered Loan excample
            txtLoanNumber.Text = "1479657"; // joint
            //txtLoanNumber.Text = "1469350"; // single
            LoadData(sender, e);
        }
    }

    protected void btnGoldCardApp_Click(object sender, EventArgs e)
    {
        string s = GridView1.SelectedRow.Cells[2].Text;
        string sProspectNumber = "", sClientNumber = "";
        string sFirstNames = "",sSurname = "";
        int iPurpose = 10;

        string sASPX_Page = "";
        if (chkProspect.Checked)
        {
            sProspectNumber = GridView1.SelectedRow.Cells[1].Text;
            sClientNumber = "";
        }
        else
        {
            sClientNumber = GridView1.SelectedRow.Cells[2].Text;
            sProspectNumber = "";
        }

        sFirstNames = GridView1.SelectedRow.Cells[3].Text;
        sSurname = GridView1.SelectedRow.Cells[4].Text;
        iPurpose = int.Parse(GridView1.SelectedRow.Cells[5].Text);

        //Check for existance of '&' in firstname - this weill tell us if it is a joint application
        enums.enApplicationType enAppType = enums.enApplicationType.Single;
        if (sFirstNames.IndexOf('&') > 0)
            enAppType = enums.enApplicationType.Joint;

        // Call Stored Procedure to create/update Barclay Card Records
        switch (enAppType)
        {
            case enums.enApplicationType.Single:
                sASPX_Page = "BarclayCardApp.aspx";
                AddUpdateBarclayCardRecord(sProspectNumber, sClientNumber, iPurpose, enums.enBondHolderType.Principal);
                break;
            case enums.enApplicationType.Joint:

                 sASPX_Page = "BarclayCardJointApp.aspx";

                 AddUpdateBarclayCardRecord(sProspectNumber, sClientNumber, iPurpose, enums.enBondHolderType.Principal);
                 AddUpdateBarclayCardRecord(sProspectNumber, sClientNumber, iPurpose, enums.enBondHolderType.Secondary);
                break;
        }

        string sUrl=sASPX_Page + "?Mid=101";
        string sNames=sFirstNames.Replace("&"," and ") + " " + sSurname.Replace("&"," and");
        if (chkProspect.Checked)
        {
            sUrl += "&param0=" + this.txtLoanNumber.Text.Trim() + "&param1=0" + "&param2=" + sNames;
        }
        else
        {
            sUrl += "&param0=0&param1=" + this.txtLoanNumber.Text.Trim() + "&param2=" + sNames;
        }

        Response.Redirect(sUrl);
    }

    #region FUNCTION : AddUpdateBarclayCardRecord
    private bool AddUpdateBarclayCardRecord(string sProspectNumber, string sLoanNumber, int iPurpose, enums.enBondHolderType enBondHolder)
    {
        string sStoredProc = "";

        try
        {
            if (enBondHolder == enums.enBondHolderType.Principal)
                sStoredProc = "bc_AddUpdateBarclayCardRecord";
            else
                sStoredProc = "bc_AddUpdateBarclayCardRecordSecondary";

            oConn.ConnectionString = DBConnection.ConnectionString(); 
            oConn.Open();

            SqlCommand com = new SqlCommand(string.Format("exec " + sStoredProc + " {0}, '{1}'," + iPurpose,
                        sProspectNumber == "" ? sLoanNumber : sProspectNumber,
                        sProspectNumber == "" ? "Client" : "Prospect"), oConn);

            com.ExecuteNonQuery();

            oConn.Close();

            return true;

        }
        catch
        {
            oConn.Close();

            return false;
        }
    } 
    #endregion

    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.txtLoanNumber.Text = GridView1.SelectedRow.Cells[1].Text;
    }

    protected void LoadData(object sender, EventArgs e)
    {
        string sSQL = "";
        string sStartFrom = txtLoanNumber.Text == "" ? "0" : txtLoanNumber.Text;

        if (chkProspect.Checked)
        {
            sSQL = "select top 50 prospectnumber as loannumber,clientnumber,prospectfirstnames as clientname,prospectsurname as clientsurname,purposenumber from Prospect";
            sSQL += " where prospectnumber >= " + sStartFrom;
        }
        else
        {
            sSQL = "select top 50 loannumber,clientnumber,clientfirstnames as clientname,clientsurname,'10' as purposenumber from vw_allloansbasic";
            sSQL += " Where loannumber >= " + sStartFrom;
        }
        this.SqlDataSource1.ConnectionString = DBConnection.ConnectionString();
        this.SqlDataSource1.SelectCommand = sSQL;

        this.GridView1.DataBind();
    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (chkProspect.Checked)
            {
                e.Row.Cells[1].Text = "Prospect Number";
                e.Row.Cells[2].Text = "Client Number";
                e.Row.Cells[3].Text = "Prospect First Names";
                e.Row.Cells[4].Text = "Prospect Surname";
                e.Row.Cells[5].Text = "Purpose";
            }
            else
            {
                e.Row.Cells[1].Text = "Loan Number";
                e.Row.Cells[2].Text = "Client Number";
                e.Row.Cells[3].Text = "Client Name";
                e.Row.Cells[4].Text = "Client Surname";
                e.Row.Cells[5].Text = "Purpose";
            }
        }
        
    }
    
}
