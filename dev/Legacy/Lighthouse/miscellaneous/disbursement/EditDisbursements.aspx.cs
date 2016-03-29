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
using SAHLLib;

public partial class disbursement_EditDisbursements : System.Web.UI.Page
{
    private SqlConnection oConn = new SqlConnection();
    private DataSet oDS = new DataSet();
    private string sConnectionString = "";

    void Page_Load(object sender, EventArgs e)
    {

        sConnectionString = DBConnection.ConnectionString();

        if (!IsPostBack)
        {
            GetLookupData();
        }



        Page.DataBind();

    }


    private Boolean GetLookupData()
    {
        oConn.ConnectionString = DBConnection.ConnectionString();
        SqlDataAdapter oDA = new SqlDataAdapter("", oConn);

        oDA.SelectCommand.CommandText = "select * from acbtype";
        oDA.Fill(oDS, "ACBType");

        oDA.SelectCommand.CommandText = "select * from acbbank";
        oDA.Fill(oDS, "ACBBank");

        oDA.SelectCommand.CommandText = "select ACBBranchCode, ACBBranchCode + ' - ' + ACBBranchDescription ACBBranchDescription from ACBBranch where activeindicator = 0 ";
        oDA.Fill(oDS, "ACBBranch");

        ddlAccountType.DataSource = oDS;
        ddACBBank.DataSource = oDS;
        ddACBBranch.DataSource = oDS;

        ddACBBranch.DataMember = "ACBBranch";
        ddACBBranch.DataValueField = "ACBBranchCode";
        ddACBBranch.DataTextField = "ACBBranchDescription";

        lblLoanNumber.Text = oDS.Tables["Screen"].Rows[0]["LoanNumber"].ToString();
        tbAccountNumber.Text = oDS.Tables["Screen"].Rows[0]["LoanACBAccountNumber"].ToString();
        ddlAccountType.SelectedValue = Convert.ToString(oDS.Tables["Screen"].Rows[0]["ACBTypeNumber"]);
        ddACBBranch.SelectedValue = Convert.ToString(oDS.Tables["Screen"].Rows[0]["ACBBranchCode"]);
        ddACBBank.SelectedValue = Convert.ToString(oDS.Tables["Screen"].Rows[0]["ACBBankCode"]);

        return true;
    }

    protected void ddACBBank_SelectedIndexChanged(object sender, EventArgs e)
    {
        oConn.ConnectionString = sConnectionString;
        oConn.Open();

        string sSQL = string.Format("select * from acbbranch where acbbankcode = {0}", ddACBBank.SelectedValue);

        SqlDataAdapter oDA = new SqlDataAdapter(sSQL, oConn);
        oDA.Fill(oDS, "ACBBranchFiltered");

        ddACBBranch.DataSource = oDS;
        ddACBBranch.DataMember = "ACBBranchFiltered";
        ddACBBranch.DataValueField = "ACBBranchCode";
        ddACBBranch.DataTextField = "ACBBranchDescription";
        ddACBBranch.DataBind();

    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {

        SqlCommand oCMD = new SqlCommand();
        oCMD.Connection = new SqlConnection(sConnectionString);
        oCMD.CommandType = CommandType.Text;
        string sSQL = "";

        sSQL = string.Format("update Disbursement" +
                                     "set DisbursementAccountNumber = {0}, " +
                                     "ACBBranchCode = '{1}', " +
                                     "ACBTypeNumber = {2} " +
                                     "where loannumber = {3}",
                                     tbAccountNumber.Text,
                                     ddACBBranch.SelectedValue,
                                     ddlAccountType.SelectedValue,
                                     lblLoanNumber.Text);
     

        oCMD.CommandText = sSQL;
        oCMD.Connection.Open();
        oCMD.ExecuteNonQuery();

        //oCMD.CommandText = string.Format("Update CDInterimAudit " +
                          //  "set ErrorCode = '0000', " +
                          //  "SAHLError = '' " +
                          //  "where Loannumber = {0} " +
                          //  "and SequenceNumber = {1}", lblLoanNumber.Text, lblSeqNum.Text);


        oCMD.ExecuteNonQuery();

        Response.Write(Session["Closer"]);


    }
    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Select")
        {
            getData(GridView1.SelectedRow.Cells[1].Text);
        }
    }

    private void getData(string sLoanNumber)
    {
        oConn.ConnectionString = DBConnection.ConnectionString();
        oConn.Open();


        string sSQL = string.Format("SELECT Disbursement.LoanNumber, Client.ClientSalutation , " +
                      "Client.ClientInitials , Client.ClientSurname ClientName, " +
                      "Disbursement.ACBBranchCode, Disbursement.ACBTypeNumber, Disbursement.DisbursementAccountNumber LoanACBAccountNumber, " +
                      "ACBBank.ACBBankCode " +
                      "FROM Disbursement " +
                      "INNER JOIN vw_allloans Loan ON Disbursement.loannumber = Loan.loannumber" +
                      "INNER JOIN client ON Client.ClientNumber = Loan.ClientNumber " +
                      "INNER JOIN ACBBranch ON ACBBranch.ACBBranchCode = Loan.ACBBranchCode " +
                      "inner join ACBBank on ACBBank.ACBBankCode = ACBBranch.ACBBankCode " +
                      "WHERE DisbursementActionDate >= CONVERT(CHAR(8), GETDATE(), 112) " +
                      "AND Loan.LoanNumber = {0}", sLoanNumber);


        SqlDataAdapter oDA = new SqlDataAdapter(sSQL, oConn);
        oDA.Fill(oDS, "Screen");
        oConn.Close();
    }
}
