using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Data.SqlClient;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class DocVerManagement : System.Web.UI.Page
{
    protected string sLoanNumber = "";
    protected string sClientName = "";
    protected string sPrintDate = "";
    protected string sReceiveDate = "";
    protected string sLAVersion = "";
    protected string sLADateReceived = "";
    protected string sLSVersion = "";
    protected string sLSDateReceived = "";
    protected string sUpdatedYN = "";
    protected string sUserName = HttpContext.Current.User.Identity.Name.ToString();

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!Page.IsPostBack)
        {
            // populate the form
            populateForm();
        }
        else
        {
            txtLoanNumber.Text = sLoanNumber;
            txtClientName.Text = sClientName;
            txtDatePrinted.Text = sPrintDate;
        }
    }

    protected override void OnInit(EventArgs e)
    {
        initParameters();
    }

    private void initParameters()
    {
        // want to get the default query string parameters.
        sLoanNumber = Request.QueryString["Param0"];
        sUserName = sUserName.Replace("SAHL\\", "");

        if (sLoanNumber != "")
        {
            getData(sLoanNumber);
        }
    }

    private void getData(string sRef)
    {
        //SqlConnection sqlCon = new SqlConnection(DBConnection.ConnectionString());
        //SqlCommand sqlCmd = new SqlCommand(string.Format("SELECT * FROM AccountDocumentVersionManagement WHERE LoanNumber = {0}", sRef));
        //string sSQL = "SELECT am.*, av.*, v.Description As [Version] FROM AccountDocumentVersionManagement am INNER JOIN AccountDocumentVersion av ON av.AccountKey = am.LoanNumber INNER JOIN Document d ON av.DocumentKey = d.DocumentKey INNER JOIN Version v ON d.VersionKey = v.VersionKey WHERE am.LoanNumber = {0} AND d.Description in ('Loan Agreement', 'Loan Schedule')";
        SqlDataAdapter sqlAd = new SqlDataAdapter(string.Format("SELECT * FROM AccountDocumentVersionManagement WHERE LoanNumber = {0}", sRef), DBConnection.ConnectionString());
        DataTable dt = new DataTable();

        try
        {
            // want to open the connection
            //sqlCon.Open();

            // fill the dataset
            sqlAd.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                // set the local variables
                sClientName = dt.Rows[0]["ClientFirstName"].ToString() + " " + dt.Rows[0]["ClientSurname"].ToString();
                sPrintDate = dt.Rows[0]["DatePrinted"].ToString();
                // get version and date information for Loan Agreement
                sLAVersion = dt.Rows[0]["LAVersion"].ToString();
                sLADateReceived = dt.Rows[0]["LADateReceived"].ToString();
                // get the version and date information for the Loan Schedule
                sLSVersion = dt.Rows[0]["LSVersion"].ToString();
                sLSDateReceived = dt.Rows[0]["LSDateReceived"].ToString();
                //sReceiveDate = dt.Rows[0]["DateReceived"].ToString();
                if (dt.Rows[0]["updatedYN"].ToString() == "Y") { sUpdatedYN = "Yes"; } else { sUpdatedYN = "No"; }

            }

        }
        catch (SqlException ex)
        {
            txtError.Text = ex.Message;
        }
    }

    private void populateForm()
    {
        // will set the text boxes ...
        txtLoanNumber.Text = sLoanNumber;
        txtClientName.Text = sClientName;
        txtDatePrinted.Text = sPrintDate;
        wcDateReceived.SelectedDate = sReceiveDate;

        // populate the document version section
        txtLAVersion.Text = sLAVersion;
        txtLSVersion.Text = sLSVersion;
        wcLADate.SelectedDate = sLADateReceived;
        wcLSDate.SelectedDate = sLSDateReceived;
        if (sLADateReceived != "") { chkLAReceived.Checked = true; } else { chkLAReceived.Checked = false; }
        if (sLSDateReceived != "") { chkLSReceived.Checked = true; } else { chkLSReceived.Checked = false; }

    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        SqlCommand sqlCmd = new SqlCommand();
        string sSQL = ""; string sVer = ""; string sStart = "UPDATE AccountDocumentVersionManagement SET ";
        SqlConnection sqlCon = new SqlConnection(DBConnection.ConnectionString());
        sqlCmd.Connection = sqlCon;

        // first check that input is ok
        if (!validateInput()) { return; };

        // else want to update the database
        try
        {
            sSQL = sStart;
            if (chkLSReceived.Checked)
            {
                // have received the loan schedule so set the date and version
                sVer = updateLoanSchedule();
                if (sVer != "") { sSQL += string.Format("LSReturned = 'Y', LSVersion = '{0}', LSDateReceived = '{1}'", sVer, wcLSDate.SelectedDate); }
            }
            if (chkLAReceived.Checked)
            {
                // have received the loan agreement back so set the date and version
                sVer = updateLoanAgreement();
                if (sVer != "") { if (sSQL != sStart) { sSQL += ", "; }; sSQL += string.Format("LAReturned = 'Y', LAVersion = '{0}', LADateReceived = '{1}'", sVer, wcLADate.SelectedDate); }
            }
            if (chkLAReceived.Checked && chkLSReceived.Checked)
            {
                sSQL += ", updatedYN = 'Y'";
            }
            sSQL += string.Format(" WHERE LoanNumber = {0}", sLoanNumber);
            sqlCon.Open();
            if (sSQL != "")
            {
                sqlCmd.CommandText = sSQL;
                sqlCmd.ExecuteNonQuery();
                // went through ok so can get the DB details again
                getData(sLoanNumber);
                populateForm();
            }
        }
        catch (SqlException ex)
        {
            txtError.Text = ex.Message;
        }
    }

    private string updateLoanAgreement()
    {
        SqlCommand sqlCmd = new SqlCommand();
        string sSQL = "";
        SqlConnection sqlCon = new SqlConnection(DBConnection.ConnectionString());
        SqlDataAdapter sqlAd;
        sqlCmd.Connection = sqlCon;
        DataTable dt = new DataTable();
        int iDocKey = 0; string sVersion = "";

        // get back the document key of the latest version of the document
        sSQL = "SELECT Document.DocumentKey, v.Description FROM Document inner join Version v on Document.VersionKey = v.VersionKey WHERE Document.Description = 'Loan Agreement' AND Document.ActiveIndicator = 'Y'";
        sqlAd = new SqlDataAdapter(sSQL, DBConnection.ConnectionString());
        sqlAd.Fill(dt);

        if (dt.Rows.Count > 0)
        {
            iDocKey = int.Parse(dt.Rows[0]["DocumentKey"].ToString());
            sVersion = dt.Rows[0]["Description"].ToString();
        }

        // check to see if there is already a older version
        sSQL = string.Format("SELECT d.DocumentKey FROM AccountDocumentVersion adv INNER JOIN Document d on adv.DocumentKey = d.DocumentKey WHERE adv.AccountKey = {0} AND d.Description = 'Loan Agreement'", sLoanNumber);
        sqlAd = new SqlDataAdapter(sSQL, DBConnection.ConnectionString());
        dt = new DataTable();
        sqlAd.Fill(dt);
        if (dt.Rows.Count > 0)
        {
            sSQL = string.Format("UPDATE AccountDocumentVersion SET DocumentKey = {0} WHERE AccountKey = {1} AND DocumentKey = {2}", iDocKey, sLoanNumber, dt.Rows[0]["DocumentKey"].ToString());
        }
        else
        {
            sSQL = string.Format("INSERT INTO AccountDocumentVersion (AccountKey, DocumentKey) VALUES ({0}, {1})", sLoanNumber, iDocKey);
        }
        sqlCmd.CommandText = sSQL;
        sqlCon.Open();
        sqlCmd.ExecuteNonQuery();

        return sVersion;
        
    }

    private string updateLoanSchedule()
    {
        SqlCommand sqlCmd = new SqlCommand();
        string sSQL = "";
        SqlConnection sqlCon = new SqlConnection(DBConnection.ConnectionString());
        SqlDataAdapter sqlAd;
        sqlCmd.Connection = sqlCon;
        DataTable dt = new DataTable();
        int iDocKey = 0; string sVersion = "";

        // get back the document key of the latest version of the document
        sSQL = "SELECT Document.DocumentKey, v.Description FROM Document inner join Version v on Document.VersionKey = v.VersionKey WHERE Document.Description = 'Loan Schedule' AND Document.ActiveIndicator = 'Y'";
        sqlAd = new SqlDataAdapter(sSQL, DBConnection.ConnectionString());
        sqlAd.Fill(dt);

        if (dt.Rows.Count > 0)
        {
            iDocKey = int.Parse(dt.Rows[0]["DocumentKey"].ToString());
            sVersion = dt.Rows[0]["Description"].ToString();
        }

        // check to see if there is already a older version
        sSQL = string.Format("SELECT d.DocumentKey FROM AccountDocumentVersion adv INNER JOIN Document d on adv.DocumentKey = d.DocumentKey WHERE adv.AccountKey = {0} AND d.Description = 'Loan Schedule'", sLoanNumber);
        sqlAd = new SqlDataAdapter(sSQL, DBConnection.ConnectionString());
        dt = new DataTable();
        sqlAd.Fill(dt);
        if (dt.Rows.Count > 0)
        {
            sSQL = string.Format("UPDATE AccountDocumentVersion SET DocumentKey = {0} WHERE AccountKey = {1} AND DocumentKey = {2}", iDocKey, sLoanNumber, dt.Rows[0]["DocumentKey"].ToString());
        }
        else
        {
            sSQL = string.Format("INSERT INTO AccountDocumentVersion (AccountKey, DocumentKey) VALUES ({0}, {1})", sLoanNumber, iDocKey);
        }
        sqlCmd.CommandText = sSQL;
        sqlCon.Open();
        sqlCmd.ExecuteNonQuery();
        
        return sVersion;
    }

    private bool validateInput()
    {
        if (wcDateReceived.SelectedDate == "")
        {
            txtError.Text = "Please select a date received.";
            return false;
        }
        if (chkLAReceived.Checked)
        {
            if (wcLADate.SelectedDate == "")
            {
                txtError.Text = "Please select a date received for the Loan Agreement.";
                return false;
            }
        }
        if (chkLSReceived.Checked)
        {
            if (wcLSDate.SelectedDate == "")
            {
                txtError.Text = "Please select a date received for the Loan Agreement.";
                return false;
            }
        }

        return true;
    }
}
