using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

public partial class ArrearGrid : System.Web.UI.Page
{
    string sLoanNumber = "";
    bool bVariFix = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            System.Text.StringBuilder sbSQL = new System.Text.StringBuilder("");

            if (!Page.IsPostBack)
            {
                // want to set up the initial parameters
                optHistory.SelectedIndex = 0;
            }

            // get loan number from querystring
            sLoanNumber = Request.QueryString["loanNumber"];
            if ((sLoanNumber == "") || (sLoanNumber == null)) { sLoanNumber = "1475044"; }

            string query = @"SELECT
	                            isnull(convert(numeric(10), at.ArrearTransactionKey),0) as [Number],
	                            rtrim(ltrim(at.Reference)) as [Reference],
	                            rtrim(ltrim(tt.Description)) as [Type],
	                            at.UserID as [Changed By],
	                            convert(varchar(10), at.InsertDate ,103) as [Insert Date],
	                            convert(varchar(10), at.EffectiveDate ,103) as [Effective Date],
								(
									SELECT TOP 1 CONVERT(VARCHAR(16), CONVERT(NUMERIC(10, 2), ROUND(InterestRate * 100,2)))
									FROM [2am].fin.FinancialTransaction ft (NOLOCK) 
									WHERE ft.FinancialServiceKey = pfs.FinancialServiceKey
									AND at.EffectiveDate >= ft.EffectiveDate
									ORDER BY ft.EffectiveDate desc
								) as [Rate],
	                            CONVERT(varchar(16), convert(numeric(10,2), ROUND(at.Amount, 2))) as [Amount],
	                            CONVERT(varchar(16), convert(numeric(10,2), ROUND(atab.AccountBalance, 2))) as [Balance],
	                            ISNULL(fst.Description, 'Variable Loan') as [ServiceType],
	                            ttui.HTMLColour as [HTMLColour]
                            FROM [2am].fin.ArrearTransaction at (nolock)
                            INNER JOIN [2am].fin.TransactionType tt (nolock) ON tt.TransactionTypeKey = at.TransactionTypeKey
                            INNER JOIN [2am].fin.ArrearTransactionAccountBalance atab (nolock) ON at.ArrearTransactionKey = atab.ArrearTransactionKey
                            INNER JOIN [2am].dbo.FinancialService fs (nolock) ON fs.FinancialServiceKey = at.FinancialServiceKey
                            INNER JOIN [2am].dbo.FinancialService pfs (nolock) ON pfs.FinancialServiceKey = fs.ParentFinancialServiceKey
                            INNER JOIN [2am].fin.TransactionTypeGroup ttg (NOLOCK) ON ttg.TransactionTypeKey = tt.TransactionTypeKey
                            JOIN [2am].dbo.FinancialServiceType fst (nolock) ON pfs.FinancialServiceTypeKey = fst.FinancialServiceTypeKey
                            JOIN [2am].dbo.[TransactionTypeUI] ttui (NOLOCK) ON ttui.TransactionTypeKey = tt.TransactionTypeKey
                            WHERE ttg.TransactionGroupKey = 1 and pfs.AccountKey = {0}";

            sbSQL.Append(string.Format(query, sLoanNumber));

            // want to set up and bind the grid
            SqlDataSource sqlDS = new SqlDataSource();
            sqlDS.ConnectionString = DBConnection.ConnectionString("AppPluginUser", "W0rdpass");

            if (optHistory.SelectedItem != null)
            {
                switch (optHistory.SelectedValue)
                {
                    case "4":
                    case "6":
                        sbSQL.Append(" AND at.InsertDate BETWEEN DateAdd(m, -" + optHistory.SelectedValue + ", GetDate()) AND GetDate() ");
                        break;
                    case "0":
                        break;
                }
            }

            // Append order by clause
            sbSQL.Append(" ORDER BY at.ArrearTransactionKey");

            sqlDS.SelectCommand = sbSQL.ToString();

            gvArrearGrid.DataSource = sqlDS;
            gvArrearGrid.DataBind();

            formatGrid();
        }
        catch (Exception ex)
        {
            lblErr.Visible = true;
            lblErr.Text = ex.Message;
        }
    }

    private void getParameters()
    {
        // now get whether loan is fixed or not
        bVariFix = getFixed(sLoanNumber);
    }

    private bool getFixed(string sRef)
    {
        System.Data.SqlClient.SqlDataAdapter sqlAd = new System.Data.SqlClient.SqlDataAdapter(string.Format("SELECT VariFix FROM vw_AllOpenLoans WHERE LoanNumber = {0}", sRef), DBConnection.ConnectionString());
        DataTable dt = new DataTable();

        try
        {
            sqlAd.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0][0].ToString() == "0")
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    /// <summary>
    /// This function will go through the format grid and set all necessary fonts & colours.
    /// </summary>
    private void formatGrid()
    {
        // set the background colours on the type cells
        for (int i = 0; i < gvArrearGrid.Rows.Count; i++)
        {
            gvArrearGrid.HeaderRow.Cells[gvArrearGrid.Rows[i].Cells.Count - 1].Style.Add("display", "none");
            gvArrearGrid.Rows[i].Cells[1].ForeColor = System.Drawing.Color.White;
            gvArrearGrid.Rows[i].Cells[1].Style["background-color"] = gvArrearGrid.Rows[i].Cells[gvArrearGrid.Rows[i].Cells.Count - 1].Text;
            gvArrearGrid.Rows[i].Cells[gvArrearGrid.Rows[i].Cells.Count - 1].Style.Add("display", "none");
        }
    }
}