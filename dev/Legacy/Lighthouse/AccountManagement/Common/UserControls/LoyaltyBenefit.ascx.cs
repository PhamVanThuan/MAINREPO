using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class LoyaltyBenefit_ascx : System.Web.UI.UserControl
{
    public double AccumulatedTotal;
    public string sAccumulatedTotal;
    public string PaymentDate;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        GetLoyaltyBenefitInfo();
    }

    private void GetLoyaltyBenefitInfo()
    {
        SqlConnection Con = new SqlConnection(DBConnection.ConnectionString());
        DataSet Benefit = new DataSet();

        SqlConnection Con2AM = new SqlConnection(DBConnection.ConnectionString().Replace("SAHLDB", "2am"));
        DataSet BenefitThisMonth = new DataSet();
        try
        {
            string AccountKey = Request.QueryString["param0"];
            Int32 FinancialServiceKey = 0;
            DateTime FromDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            DateTime ToDate = DateTime.Today;



            Con.Open();
            SqlCommand cmd = new SqlCommand("c_LoanGetLoyaltyBenefitToDate", Con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@AccountKey", AccountKey);
            SqlDataAdapter SqlDA = new SqlDataAdapter(cmd);
            SqlDA.Fill(Benefit);

            if (Benefit.Tables.Count > 0)
            {
                if (Benefit.Tables[0].Rows.Count > 0)
                {
                    AccumulatedTotal = Convert.ToDouble(Benefit.Tables[0].Rows[0]["CurrentLoyaltyBenefit"]);
                    PaymentDate = Convert.ToDateTime(Benefit.Tables[0].Rows[0]["NextPaymentDate"]).ToString(Constants.DATE_FORMAT);
                    FinancialServiceKey = Convert.ToInt32(Benefit.Tables[0].Rows[0]["FinancialServiceKey"].ToString());
                }
            }

            //EXEC @RC = [2AM].[dbo].[pLoanCalcAccruedInterest] @FinancialServiceKey, @FromDate, @ToDate, @Interest OUTPUT , @LoyaltyBenefit OUTPUT
            Con2AM.Open();
            SqlCommand cmd2AM = new SqlCommand("pLoanCalcAccruedInterest", Con2AM);
            cmd2AM.CommandType = CommandType.StoredProcedure;
            cmd2AM.Parameters.AddWithValue("@FinancialServiceKey", FinancialServiceKey);
            cmd2AM.Parameters.AddWithValue("@FromDate", FromDate);
            cmd2AM.Parameters.AddWithValue("@ToDate", ToDate);
            
            SqlParameter Interest = cmd2AM.Parameters.Add("@Interest", SqlDbType.Int);
            Interest.Direction = ParameterDirection.Output;
            SqlParameter LoyaltyBenefit = cmd2AM.Parameters.Add("@LoyaltyBenefit", SqlDbType.Float);
            LoyaltyBenefit.Direction = ParameterDirection.Output;

            cmd2AM.ExecuteNonQuery();
            
            //SqlDataAdapter SqlDA2AM = new SqlDataAdapter(cmd2AM);
            //SqlDA2AM.Fill(BenefitThisMonth);

            AccumulatedTotal += Convert.ToDouble(@LoyaltyBenefit.Value);
            sAccumulatedTotal = AccumulatedTotal.ToString(Constants.CURRENCY_FORMAT);

        }
        finally
        {
                Con.Close();
                Con2AM.Close();
        }
    }
}
