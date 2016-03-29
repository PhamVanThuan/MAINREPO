using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for Disbursement_BO
/// </summary>
public class Disbursement_BO
{
    public DataTable GetDisbursementData()
    {
        DataTable dt = new DataTable();
        SqlConnection con = new SqlConnection( DBConnection.ConnectionString());
        SqlDataAdapter db = new SqlDataAdapter("SELECT CDInterimAudit.LoanNumber, CDInterimAudit.SequenceNumber,Branchcode, accountnumber, " +
                    " Client.ClientSalutation + ' ' + Client.ClientInitials + ' ' + Client.ClientSurname as ClientName,  " +
                    "isNull(CDStatusErrorCodes.CDStatusErrorDescription,CDInterimAudit.SAHLError) as ErrorDescription, " +
                    "CDInterimAudit.ErrorCode " +
                    "FROM CDInterimAudit  " +
                    "inner join cdpaymentfileschedule on FileSequenceNumber = SequenceNumber " +
                    "left outer JOIN vw_allLoansbasic Loan ON CDInterimAudit.LoanNumber = Loan.LoanNumber  " +
                    "left outer JOIN Client ON Client.ClientNumber = Loan.ClientNumber  " +
                    "left outer JOIN CDStatusErrorCodes ON CDInterimAudit.ErrorCode = CDStatusErrorCodes.CDStatusErrorCode  " +
                    "WHERE (CDInterimAudit.ErrorCode not in ( 'W002','0000','')) " +
                    "and CDInterimAudit.status is null " +
                    "and CDInterimAudit.CDInterimAuditType = 'L' " +
                    "and (SBICActionDate >=  convert(varchar, GETDATE()-10, 101) " +
                    "or actiondate >=  convert(varchar, GETDATE()-10, 101))  " +
                    "order by CDInterimAudit.SequenceNumber , CDInterimAudit.LoanNumber ", con);
        db.Fill(dt);

        return dt;
    }

    public DataTable GetAccountTypes()
    {
        DataTable dt = new DataTable();
        SqlConnection con = new SqlConnection(DBConnection.ConnectionString());
        SqlDataAdapter db = new SqlDataAdapter("select * from acbtype", con);
        db.Fill(dt);
        return dt;
    }

    public DataTable GetBankTypes()
    {
        DataTable dt = new DataTable();
        SqlConnection con = new SqlConnection(DBConnection.ConnectionString());
        SqlDataAdapter db = new SqlDataAdapter("select * from acbbank", con);
        db.Fill(dt);
        return dt;
    }

    public DataTable GetBranchCodes()
    {
        DataTable dt = new DataTable();
        SqlConnection con = new SqlConnection(DBConnection.ConnectionString());
        SqlDataAdapter db = new SqlDataAdapter("select ACBBranchCode, ACBBranchCode + ' - ' + ACBBranchDescription ACBBranchDescription from ACBBranch where activeindicator = 0", con);
        db.Fill(dt);
        return dt;
    }
}
