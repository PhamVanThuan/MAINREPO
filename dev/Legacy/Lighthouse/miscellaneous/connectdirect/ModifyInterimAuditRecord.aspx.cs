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

public partial class ModifyInterimAuditRecord_aspx:System.Web.UI.Page
{

    private SqlConnection oConn = new SqlConnection();
    private DataSet oDS = new DataSet();
    private string sConnectionString = "";

    void Page_Load(object sender, EventArgs e)
    {

        sConnectionString = DBConnection.ConnectionString();
        
        if (!IsPostBack)
        {
            if (getData()) {

                ddlAccountType.DataSource = oDS;
                ddACBBank.DataSource = oDS;
                ddACBBranch.DataSource = oDS;

                ddACBBranch.DataMember = "ACBBranch";
                ddACBBranch.DataValueField = "ACBBranchCode";
                ddACBBranch.DataTextField = "ACBBranchDescription";

                lblLoanNumber.Text = oDS.Tables["Screen"].Rows[0]["LoanNumber"].ToString();
                lblSeqNum.Text = oDS.Tables["Screen"].Rows[0]["SequenceNum"].ToString();
                lblClientNames.Text = oDS.Tables["Screen"].Rows[0]["ClientName"].ToString();
                tbAccountNumber.Text = oDS.Tables["Screen"].Rows[0]["LoanACBAccountNumber"].ToString();
                ddlAccountType.SelectedValue = Convert.ToString(oDS.Tables["Screen"].Rows[0]["ACBTypeNumber"]);
                ddACBBranch.SelectedValue = Convert.ToString(oDS.Tables["Screen"].Rows[0]["ACBBranchCode"]);
                ddACBBank.SelectedValue = Convert.ToString(oDS.Tables["Screen"].Rows[0]["ACBBankCode"]);

            }

        }

        

        Page.DataBind();

    }


    private Boolean getData()
    {
        oConn.ConnectionString = sConnectionString;
        oConn.Open();

        string sLoanNumber = (Request.QueryString["param0"] == null) ? "null" : Request.QueryString["param0"].ToString();
        sLoanNumber = sLoanNumber.Trim() != "" ?  sLoanNumber : "null";

        string sSequenceNumber = (Request.QueryString["param1"]) == null ? "" : Request.QueryString["param1"].ToString();


        string sSQL = string.Format("SELECT Loan.LoanNumber, Client.ClientSalutation , " +
                      "Client.ClientInitials , Client.ClientSurname ClientName, " +
                      "Loan.ACBBranchCode, Loan.ACBTypeNumber, Loan.LoanACBAccountNumber, " +
                      "ACBBank.ACBBankCode, " +
                      "'SequenceNum' = '" + sSequenceNumber + "' " +
                      "FROM Client " +
                      "INNER JOIN Loan ON Client.ClientNumber = Loan.ClientNumber " +
                      "INNER JOIN ACBBranch ON ACBBranch.ACBBranchCode = Loan.ACBBranchCode " +
                      "inner join ACBBank on ACBBank.ACBBankCode = ACBBranch.ACBBankCode " + 
                      "WHERE Loan.LoanNumber = {0}",sLoanNumber);
        
        
        SqlDataAdapter oDA = new SqlDataAdapter(sSQL, oConn);
        if (oDA.Fill(oDS, "Screen") == 0)
            return false;


        oDA.SelectCommand.CommandText = "select * from acbtype";
        oDA.Fill(oDS, "ACBType");

        oDA.SelectCommand.CommandText = "select * from acbbank";
        oDA.Fill(oDS, "ACBBank");

        oDA.SelectCommand.CommandText = "select ACBBranchCode, ACBBranchCode + ' - ' + ACBBranchDescription ACBBranchDescription from ACBBranch where activeindicator = 0 ";
        oDA.Fill(oDS, "ACBBranch");

        oConn.Close();

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

        switch (rblAction.SelectedValue)
        {
            case "U":
                sSQL = string.Format("update loan " +
                                     "set LoanACBAccountNumber = {0}, " +
                                     "ACBBranchCode = '{1}', " +
                                     "ACBTypeNumber = {2} " +
                                     "where loannumber = {3}",
                                     tbAccountNumber.Text,
                                     ddACBBranch.SelectedValue,
                                     ddlAccountType.SelectedValue,
                                     lblLoanNumber.Text);
                 break;

            case "S":
                sSQL = string.Format("insert into detail " +
                                     "(DetailTypeNumber,LoanNumber, DetailDate) " +
                                     "Values " +
                                     "(150, {0}, getdate())",lblLoanNumber.Text);
                break;
            case "I":
                sSQL = string.Format("insert into detail " +
                                     "(DetailTypeNumber,LoanNumber, DetailDate) " +
                                     "Values " +
                                     "(217, {0}, getdate())", lblLoanNumber.Text);
                break;

        }

        oCMD.CommandText = sSQL;
        oCMD.Connection.Open();
        oCMD.ExecuteNonQuery();

        oCMD.CommandText = string.Format("Update CDInterimAudit " +
                            "set ErrorCode = '0000', " +
                            "SAHLError = '' " +
                            "where Loannumber = {0} " +
                            "and SequenceNumber = {1}",lblLoanNumber.Text,lblSeqNum.Text);


        oCMD.ExecuteNonQuery();

        Response.Write(Session["Closer"]);


    }


}
