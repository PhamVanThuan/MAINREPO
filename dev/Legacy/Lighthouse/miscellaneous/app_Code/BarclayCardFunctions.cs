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

/// <summary>
/// Summary description for BarclayCardFunctions
/// </summary>
public class BarclayCardFunctions
{
	public BarclayCardFunctions()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    #region FUNCTION : CheckUserSecurity
    public static bool CheckUserSecurity(string sLoginName)
    {
        bool bAllowed = false;

        try
        {
            // Get the the list of Active Directory Groups which have access to Override Gold Card applications
            string[] sGroupList = ConfigurationManager.AppSettings.GetValues("GoldCardADOverrideGroups")[0].Split(',');

            // Loop thru each Group and check if the logged in user belongs to any of them
            foreach (string sGrp in sGroupList)
            {
                // Check whether the Logged-on User belongs to this AD Group
                if (ActiveDirectoryFunctions.CheckUserInGroup(sGrp, sLoginName))
                {
                    // If the user exists in group then allow access to Override Application
                    bAllowed = true;
                    break;
                }
            }
        }
        catch
        {
        }

        return bAllowed;
    }
    #endregion

    #region FUNCTION : GetClientDetails
    public static DataSet GetClientDetails(string sLoanNumber, string sProspectNumber)
    {
        DataSet dsClient = new DataSet();
        SqlConnection oConn = new SqlConnection();

        try
        {
            string sSQL = "";
            if (sLoanNumber != "") // This is a Client
            {
                sSQL = "SELECT Client.ClientSalutation,Client.ClientFirstNames,Client.ClientSurname,Client.ClientIDNumber"
                + ",Client.ClientSpouseIDNumber,Client.ClientIncome,Client.ClientSpouseIncome"
                + ",LoanOpenDate,ACBBankDescription,ACBTypeDescription,ACBBranchCode,ACBBranchDescription,LoanACBAccountNumber"
                + ",isnull(vw_AllOpenLoanAccounts.LoanNumber,0) as 'RegisteredLoanNumber' "
                + "FROM Client "
                + "LEFT JOIN vw_AllLoans ON vw_AllLoans.ClientNumber = Client.ClientNumber "
                + "LEFT JOIN vw_AllOpenLoanAccounts ON vw_AllOpenLoanAccounts.LoanNumber = vw_AllLoans.LoanNumber "
                + "WHERE vw_AllLoans.LoanNumber = '" + sLoanNumber + "'";
            }
            else // This is a Prospect
            {
                sSQL = "SELECT ProspectSalutation,ProspectFirstNames,ProspectSurname,ProspectIDNumber"
                + ",ProspectSpouseIDNumber,ProspectIncome,ProspectSpouseIncome "
                 + "FROM Prospect WHERE Prospect.ProspectNumber = '" + sProspectNumber + "'";
            }

            if (oConn.State != ConnectionState.Open)
            {
                oConn.ConnectionString = DBConnection.ConnectionString();
                oConn.Open();
            }

            SqlDataAdapter oDA = new SqlDataAdapter(sSQL, oConn);

            dsClient = new DataSet();
            oDA.Fill(dsClient);

            oConn.Close();
        }
        catch (Exception ex)
        {
            if (oConn.State == ConnectionState.Open)
                oConn.Close();
        }
        return dsClient;

    }
    #endregion
}
