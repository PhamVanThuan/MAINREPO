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

public partial class ArrearGrid_SendSMS : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack )
        {
            txtSMSDate.Text = DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;
        }

    }
    private void GeteWorkCases()
    {
        
        txtTotalCases.Text = "0";

        string sSelectSQL = "SELECT count(*) FROM [e-work]..eFolder (nolock) " +
                        "where " +
                        "eMapName = 'LossControl' " +
                        "and " +
                        "convert(varchar,eCreationTime,103) = convert(varchar,'" + txtSMSDate.Text + "',103) ";

        System.Data.SqlClient.SqlDataAdapter sqlAd = new System.Data.SqlClient.SqlDataAdapter(sSelectSQL, DBConnection.ConnectionString());
        DataTable dt = new DataTable();

        try
        {
            sqlAd.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                txtTotalCases.Text = Convert.ToString(dt.Rows[0][0].ToString());
            }

        }
        catch (Exception ex)
        {
            lblErr.Visible = true;
            lblErr.Text = ex.Message;
        }
    
    }

    protected void btnSearchSMS_Click(object sender, EventArgs e)
    {

            System.Text.StringBuilder sbSQL = new System.Text.StringBuilder("");
            string sSMSDate = DateTime.Now.Day + "/" + DateTime.Now.Month + "/" +DateTime.Now.Year;
            
            try 
	        {
                lblErr.Visible = false;
                sSMSDate = Convert.ToDateTime(txtSMSDate.Text).ToString("dd/MM/yyyy");
	        }
	        catch (Exception ex)
	        {
                lblErr.Visible = true;
                lblErr.Text = ex.Message;
                return;
	        }

            sbSQL.Append("select " +
                        "cel.EmailSubject as Subject, " +
                        "cel.EmailBody as Message, " +
                        "cel.Cellphone as [Cell Phone], " +
                        "cel.LoanNumber as [Loan Number], " +
                        "cel.InsertedDate as [Inserted Date], " +
                        "cel.SentToClient as [Sent To Client], " +
                        "cel.SentDate as [Sent Date] " +
                        "from " +
                        "ClientEmail_LossControl cel (nolock) " +
                        "where " +
                        "convert(varchar,InsertedDate,103) = convert(varchar,'" + txtSMSDate.Text + "',103) ");

            // want to set up and bind the grid
            SqlDataSource sqlDS = new SqlDataSource();
            sqlDS.ConnectionString = DBConnection.ConnectionString();

            sqlDS.SelectCommand = sbSQL.ToString();

            gvSMS.DataSource = sqlDS;

            gvSMS.DataBind();

            // obtain total no. of e-Works cases loaded for the date selected
            GeteWorkCases();
    
            // obtain total no. of sms to be sent
            txtTotalSms.Text = Convert.ToString(gvSMS.Rows.Count);

    }

    protected void btnProcessSMS_Click(object sender, EventArgs e)
    {
        // Create and open connection            
        SqlConnection oSQLConnection = new SqlConnection(DBConnection.ConnectionString());
        oSQLConnection.Open();

        SqlTransaction oSQLTran;
        oSQLTran = oSQLConnection.BeginTransaction();

        SqlCommand oSQLCom = oSQLConnection.CreateCommand();
        oSQLCom.Connection = oSQLConnection;
        oSQLCom.Transaction = oSQLTran;

        try
        {
            // Insert into ClientEmail table
            oSQLCom.CommandText = "insert into " +
                "ClientEmail	(EmailSubject, EmailBody, EmailAttachment1, LoanNumber, Cellphone, EmailTo, EmailFrom) " +
                "select EmailSubject, EmailBody, EmailAttachment1, LoanNumber, Cellphone, EmailTo, EmailFrom " +
                "from ClientEmail_LossControl " +
                "where " +
                "SentToClient = 0 and " +
                "convert(varchar,InsertedDate,103) = convert(varchar,'" + txtSMSDate.Text + "',103) ";

            oSQLCom.ExecuteNonQuery();

            // Update the ClientEmail_LossControl table
            oSQLCom.CommandText = "update ClientEmail_LossControl " +
                "set SentToClient = 1, " +
                "SentDate = getdate() " +
                "where " +
                "convert(varchar,InsertedDate,103) = convert(varchar,'" + txtSMSDate.Text + "',103) ";

            oSQLCom.ExecuteNonQuery();

            oSQLTran.Commit();
        }
        catch (Exception ex)
        {
            lblErr.Visible = true;
            lblErr.Text = ex.Message;
            
            oSQLTran.Rollback();
        }

        oSQLConnection.Close();        
        
    }
}
