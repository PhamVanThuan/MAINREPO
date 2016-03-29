using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

public partial class AssetTransfer : System.Web.UI.Page
{
    string spvList = "";
    string sUserName = "";
    bool bTransferSPVAllowed = false;

    SqlDataSource SQLDS = new SqlDataSource();

    /// <summary>
    /// First time: populates the combo, creates viewstate variable
    /// Postback: retrieve loans from viewstate, populates grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        SAHLLib.Security ss = new SAHLLib.Security();
        string sCon = DBConnection.ConnectionString(sUserName);
        bTransferSPVAllowed = ss.functionAccessAllowed(SAHLLib.Security.etFunction.TransferToTargetSPV, sUserName, sCon);
        cmdTransferSPV.Enabled = bTransferSPVAllowed;

        if (!Page.IsPostBack)
        {
            TruncateTable();
            PopGrid();
            PopSPVCombo();

            cmdClearAll.Attributes.Add("onclick", "return confirm('Are you sure you want to delete all the rows?');");
            cmdRemoveLoan.Attributes.Add("onclick", "return confirm('Are you sure you want to remove the selected loan?');");
            cmdTransferSPV.Attributes.Add("onclick", "return confirm('Are you sure you want to execute the transfer?');");

            txtAccrualDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        }
    }

    //______________________________________________________________________________________________________________________________
    protected override void OnInit(EventArgs e)
    {
        initParameters();
    }

    //______________________________________________________________________________________________________________________________
    /// <summary>
    /// Gets the initial QueryString parameters.
    /// </summary>
    private void initParameters()
    {
        // get the standard variables from the QueryString
        sUserName = Request.QueryString["Param2"];
        if (sUserName == "" || sUserName == null)
        {
            sUserName = HttpContext.Current.User.Identity.Name.ToString();
            sUserName = sUserName.Replace("SAHL\\", "").ToString();
        }
    }

    //______________________________________________________________________________________________________________________________
    /// <summary>
    /// Populates the spv dropdown list with unlocked spvs
    /// </summary>
    private void PopSPVCombo()
    {
        try
        {
            SqlDataSource sqlDSSPV = new SqlDataSource();
            sqlDSSPV.ConnectionString = DBConnection.ConnectionString(sUserName);

            spvList = "SELECT SPVKey as SPVNumber, [Description] as SPVDescription FROM [2am].spv.SPV (nolock) WHERE GeneralStatusKey = 1 and SPVKey not in ( 24, 25, 26 )";

            sqlDSSPV.SelectCommand = spvList;

            ddlSPV.DataSource = sqlDSSPV;
            ddlSPV.DataBind();
        }
        catch (SqlException ex)
        {
            txtError.Text = ex.Message;
        }
    }

    /// <summary>
    ///
    /// </summary>
    private void SetupInterface()
    {
        if (grdLoans.Rows.Count == 0)
        {
            cmdRemoveLoan.Enabled = false;
            cmdClearAll.Enabled = false;
            cmdTransferSPV.Enabled = false;
        }
        else
        {
            cmdRemoveLoan.Enabled = true;
            cmdClearAll.Enabled = true;
            cmdTransferSPV.Enabled = bTransferSPVAllowed;
        }
    }

    /// <summary>
    /// Truncates the AssetTransfer table when the page is loaded for the first time.
    /// </summary>
    ///
    private void TruncateTable()
    {
        SqlConnection oSQLCon = new SqlConnection();

        try
        {
            // Create and open connection
            oSQLCon.ConnectionString = DBConnection.ConnectionString(sUserName);
            oSQLCon.Open();

            string sqlText = "Delete From [2am]..AssetTransfer Where UserName = '" + sUserName + "'";

            SqlCommand oSQLCom = new SqlCommand(sqlText, oSQLCon);
            oSQLCom.ExecuteNonQuery();
        }
        catch (SqlException ex)
        {
            txtError.Text = ex.Message;
        }

        oSQLCon.Close();
    }

    /// <summary>
    /// Populates the grid with the loan details of the loans in the LoanNumber ArrayList
    /// </summary>
    ///
    private void PopGrid()
    {
        string sGridSQL = "Select AccountKey, "
                        + "ClientSurname, "
                        + "LoanTotalBondAmount, "
                        + "LoanCurrentBalance, "
                        + "SPVDescription, "
                        + "sum(AccruedInterest) as AccruedInterest, "
                        + "LoanCurrentBalance + sum(AccruedInterest) as DebtorBalance, "
                        + "TransferedYN "
                + "From ( "
                    + "Select "
                    + "AccountKey = atr.AccountKey, "
                    + "ClientSurname, "
                    + "LoanTotalBondAmount=CONVERT(numeric(10,0),ROUND( LoanTotalBondAmount,0)), "
                    + "LoanCurrentBalance=CONVERT(numeric(10,2),ROUND( LoanCurrentBalance,2)), "
                    + "s.[Description] as SPVDescription, "
                    + "AccruedInterest = Round(IsNull(lb.MTDInterest,0),2),"
                    + "TransferedYN "
                    + "From [2am]..AssetTransfer atr (nolock) "
                        + "Join [2am].spv.SPV s (nolock) on s.SPVKey = atr.SPVKey "
                        + "Left Join [2am]..FinancialService fs (nolock) On fs.AccountKey = atr.Accountkey "
                        + "Left join [2am].fin.LoanBalance lb (nolock) on lb.FinancialServiceKey = fs.FinancialServiceKey "
                    + "Where UserName = '" + sUserName + "') a "

                + "Group By AccountKey, ClientSurname, LoanTotalBondAmount, LoanCurrentBalance, SPVDescription, TransferedYN ";

        try
        {
            SQLDS.ConnectionString = DBConnection.ConnectionString(sUserName);

            SQLDS.SelectCommand = sGridSQL;

            grdLoans.DataSource = SQLDS;
            grdLoans.DataBind();
        }
        catch (SqlException ex)
        {
            txtError.Text = ex.Message;
        }

        FormatGrid();

        CalcTotals();

        SetupInterface();
    }

    /// <summary>
    /// Formats the visual properties of the grid
    /// </summary>
    ///
    private void FormatGrid()
    {
        int i;

        // Format rows with alternate colours
        for (i = 0; i < grdLoans.Rows.Count; i++)
        {
            if (i % 2 > 0)
            {
                grdLoans.Rows[i].BackColor = System.Drawing.Color.LightGoldenrodYellow;
            }
            else
            {
                grdLoans.Rows[i].BackColor = System.Drawing.Color.Silver;
            }
        }

        grdLoans.HeaderStyle.BackColor = System.Drawing.Color.CornflowerBlue;
    }

    /// <summary>
    /// Exports the contents of the grid to csv file.
    /// Redirects to another aspx page.
    /// </summary>
    private void ExportGridToCSV()
    {
        // Variables for file name and url paths for redirect
        string sFileName = Guid.NewGuid().ToString() + "_AssetTransfer.csv";
        string sFilePathName = Request.PhysicalApplicationPath.ToString() + "AssetTransfer\\" + sFileName;
        string sURL = Request.Url.ToString();
        string sURLRedirect = sURL.Substring(0, sURL.LastIndexOf("/"));

        string sFileLine = "";

        // Create the file
        TextWriter tw = new StreamWriter(sFilePathName);

        for (int i = 0; i < grdLoans.Rows.Count; i++)
        {
            for (int j = 0; j < grdLoans.Columns.Count; j++)
            {
                if (sFileLine != "") sFileLine += ", ";
                sFileLine += grdLoans.Rows[i].Cells[j].Text;
            }

            tw.WriteLine(sFileLine);
            sFileLine = "";
        }

        tw.Close();

        Response.Redirect("DownloadCSV.aspx?FileNamePath=" + sFilePathName + "&FileName=" + sFileName);
    }

    /// <summary>
    /// Calculates the totals for the grid.
    /// </summary>
    private void CalcTotals()
    {
        // Loan Count
        txtLoanCount.Text = grdLoans.Rows.Count.ToString();

        // Current Balance
        txtCurrentBalance.Text = GetColumnTotal(5).ToString("R #,###,###,##0.00");

        // Accrued Interest
        txtAccruedInterest.Text = GetColumnTotal(6).ToString("R #,###,###,##0.00");

        // Debtor Balance
        txtDebtorBalance.Text = GetColumnTotal(7).ToString("R #,###,###,##0.00");

        // Bond
        txtBond.Text = GetColumnTotal(4).ToString("R #,###,###,##0.00");
    }

    /// <summary>
    /// Returns the total for the column
    /// </summary>
    /// <param name="ColNo">Column index</param>
    /// <returns></returns>
    private double GetColumnTotal(Int16 ColNo)
    {
        double colTotal = 0;

        if (grdLoans.Rows.Count > 0)
        {
            for (int i = 0; i < grdLoans.Rows.Count; i++)
            {
                colTotal += Convert.ToDouble(grdLoans.Rows[i].Cells[ColNo].Text);
            }

            return colTotal;
        }
        else return 0;
    }

    /// <summary>
    /// Adds a loan to the AssetTransfer table.
    /// </summary>
    /// <param name="LoanNo">The loan to be added</param>
    ///
    private void AddLoan(Int32 LoanNo)
    {
        // Create and open connection
        SqlConnection oSQLCon = new SqlConnection();
        oSQLCon.ConnectionString = DBConnection.ConnectionString(sUserName);
        oSQLCon.Open();

        string sInclCancel;

        if (chkCancellation.Checked == true)
        {
            sInclCancel = "Y";
        }
        else
        {
            sInclCancel = "N";
        }

        string sqlText = "Exec [2am]..pLoanAddAssetTransferItem " + txtLoanNumber.Text + ", '" + sInclCancel + "', '" + sUserName + "'";

        SqlCommand oSQLCom = new SqlCommand(sqlText, oSQLCon);

        try
        {
            DataSet dsResults = new DataSet();
            SqlDataAdapter sdaSDA = new SqlDataAdapter(oSQLCom);
            sdaSDA.Fill(dsResults);
            if (dsResults.Tables.Count > 1)
            {
                if (dsResults.Tables[1].Rows.Count > 0)
                {
                    txtConfirm.Text = " This loan has a DIRECT MORTGAGE BOND. " +
                        " lnBreak It should NOT be moved unless the Readvance Amount plus costs " +
                        " lnBreak exceeds " + (Convert.ToDecimal(dsResults.Tables[1].Rows[0]["MaxReAdvance"])).ToString("R #,###,###,##0.00") + ". If this amount is exceeded please report " +
                        " lnBreak this to your supervisor as a registered cession is required and the " +
                        " lnBreak loan will need to be moved to Blue Banner Agency (16). " +
                        " lnBreak lnBreak CONTINUE WITH TRANSFER?";
                    txtLoanToRemove.Text = txtLoanNumber.Text;
                }
            }
        }
        catch (SqlException ex)
        {
            txtError.Text = ex.Message;
        }

        oSQLCon.Close();
    }

    /// <summary>
    /// Removes a loan from the LoanNumbers ArrayList
    /// </summary>
    /// <param name="LoanNo">The loan to be removed</param>
    ///
    private void RemoveLoan(Int32 LoanNo)
    {
        SqlConnection oSQLCon = new SqlConnection();

        try
        {
            // Create and open connection
            oSQLCon.ConnectionString = DBConnection.ConnectionString(sUserName);
            oSQLCon.Open();

            string sqlText = "Delete From [2am]..AssetTransfer Where AccountKey = " + txtLoanNumber.Text.ToString();

            SqlCommand oSQLCom = new SqlCommand(sqlText, oSQLCon);
            oSQLCom.ExecuteNonQuery();
        }
        catch (SqlException ex)
        {
            txtError.Text = ex.Message;
        }

        oSQLCon.Close();
    }

    /// <summary>
    /// Performs the actual SPV transfer
    /// </summary>
    /// <param name="SPVNo">SPVNumber that the loans are to be transfered to</param>
    ///
    private void TransferLoans(int SPVNo)
    {
        SqlConnection oSQLCon = new SqlConnection();

        // Create and open connection
        oSQLCon.ConnectionString = DBConnection.ConnectionString(sUserName);
        oSQLCon.Open();

        string sqlText = "[process].[lighthouse].pTransferSPV " + SPVNo + ",'" + sUserName + "'";

        SqlCommand oSQLCom = new SqlCommand(sqlText, oSQLCon);

        try
        {
            oSQLCom.ExecuteNonQuery();
        }
        catch (SqlException ex)
        {
            txtError.Text = ex.Message;
        }

        oSQLCon.Close();
    }

    #region "Events"

    /// <summary>
    /// User event Add Loan
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    ///
    protected void cmdAddLoan_Click(object sender, EventArgs e)
    {
        if (txtLoanNumber.Text == "") return;

        try
        {
            AddLoan(Convert.ToInt32(txtLoanNumber.Text));
        }
        catch
        {
        }

        txtLoanNumber.Text = "";

        PopGrid();
        PopSPVCombo();
    }

    /// <summary>
    /// Populates the Loan number text box
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    ///
    protected void grdLoans_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtLoanNumber.Text = grdLoans.SelectedRow.Cells[1].Text.ToString();
        CalcTotals();
    }

    /// <summary>
    /// Removes a loan from the list
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    ///
    protected void cmdRemoveLoan_Click(object sender, EventArgs e)
    {
        if (txtLoanNumber.Text != "")
        {
            RemoveLoan(Convert.ToInt32(txtLoanNumber.Text));
        }
        txtLoanNumber.Text = "";

        PopGrid();
        PopSPVCombo();
    }

    protected void cmdTransferSPV_Click(object sender, EventArgs e)
    {
        if (grdLoans.Rows.Count > 0)
        {
            if (ddlSPV.SelectedItem != null)
            {
                TransferLoans(Convert.ToInt16(ddlSPV.SelectedItem.Value));
                PopGrid();
            }
            else
            { txtError.Text = "Please select an SPV."; }
        }
        else
        {
            txtError.Text = "No loans to transfer!";
        }
    }

    protected void cmdExport_Click(object sender, EventArgs e)
    {
        ExportGridToCSV();
    }

    #endregion "Events"

    protected void cmdClearAll_Click(object sender, EventArgs e)
    {
        TruncateTable();
        PopGrid();
        PopSPVCombo();
    }
}