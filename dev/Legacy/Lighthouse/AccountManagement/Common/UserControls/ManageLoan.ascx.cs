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

public partial class Loan_ascx : System.Web.UI.UserControl
{
    public string Product;
    public string ResetDate;
    public string sInstallment;
    public bool bIsInterestOnly;
    
    void Page_Load(object sender, EventArgs e)
    {
        setDataSourceConnectionStr();
        SetInterestOnlyFlag();
        GetProductAndResetDate();

    }

    /// <summary>
    /// Set all SQLDataSource objects connection string properties.
    /// </summary>
    private void setDataSourceConnectionStr()
    {
        //Need to create a function in SAHLLib to allow DB specification. For now replace SAHLDB with 2am.
        string sCon = DBConnection.ConnectionString().Replace("SAHLDB", "2am");

        oLoanSummary.ConnectionString = sCon;

    }

    private void SetInterestOnlyFlag()
    {
        DataSourceSelectArguments oDSSA = new DataSourceSelectArguments();
        DataView oDV = (DataView)oLoanSummary.Select(oDSSA);

        bIsInterestOnly = Convert.ToBoolean(oDV.Table.Rows[0]["IsInterestOnly"]);
    }


    /// <summary>
    /// Get Product and Reset Date for the loan using the QueryString input
    /// </summary>
    private void GetProductAndResetDate()
    {
        SqlConnection Con = new SqlConnection(DBConnection.ConnectionString());
        DataSet LoanData = new DataSet();
        try
        {
            string LoanNumber = Request.QueryString["param0"];
            Con.Open();
            SqlCommand cmd = new SqlCommand("c_GetProductAndResetDate", Con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Loannumber", LoanNumber);
            SqlDataAdapter SqlDA = new SqlDataAdapter(cmd);
            SqlDA.Fill(LoanData);

            if (LoanData.Tables.Count > 0)
            {
                if (LoanData.Tables[0].Rows.Count > 0)
                {
                    if (LoanData.Tables[0].Rows[0]["product"] != DBNull.Value) { Product = LoanData.Tables[0].Rows[0]["product"].ToString(); }
                    if (LoanData.Tables[0].Rows[0]["NextResetDate"] != DBNull.Value) 
                    {
                        ResetDate = Convert.ToDateTime(LoanData.Tables[0].Rows[0]["NextResetDate"].ToString()).ToString("dd/MM/yyyy");
                    }
                    if (LoanData.Tables[1].Rows[0]["Installment"] != DBNull.Value) { sInstallment = LoanData.Tables[1].Rows[0]["Installment"].ToString(); }
                }
            }

            if (bIsInterestOnly)
            {
                Product += " with interest only";
            }
        }
        finally
        {
            if (Con.State == ConnectionState.Open)
            {
                Con.Close();
            }
        }
    }
}
