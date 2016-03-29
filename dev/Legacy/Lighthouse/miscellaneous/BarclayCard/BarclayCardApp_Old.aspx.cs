#region Amendment History
/// 
/// Date        Amended By      Description
/// ----        ----------      -----------
/// 02/05/06    Craig Fraser    Moved "not interested" checkbox 
///                             to the top of the screen.
/// 04/05/06    Craig Fraser    Allow Credit Limit to be changed (even if not 0) 
///                             when application is overridden (Kevin Post). Previously
///                             only allowed to change if the limit was 0.
/// 18/05/06    Craig Fraser    Fixed bug that prevented the SAVE button being enabled when the "Not Interested" checkbox was ticked.
/// 15/06/06    Craig Fraser    (Helpdesk Ticket 14266) Fixed bug that fired the APO Validation when "Other" was selected as Payment Type. 
///                             Should only do APO validation if APO payment type is selected. 
/// 20/09/06    Craig Fraser    Changed to allow Gold Card for Client with un-registered Loans.
///                             Use vw_AllLoans insstead of vw_AllOpenLoans when getting Client information. This is so we can pick up
///                             Unregistered Loans aswell. 
///                             For Prospects, dont do the check to see if loan has been registered more than 3 months ago.
/// 12/10/06    Craig Fraser    Added a Re-Submit Button - This will reset the Submit Date and allow the application to be edited.
///                             It will then be picked up and re-submitted on the next Standard Bank Extract run.
///                             Only available to Gold Card Managers
/// 19/12/06    Craig Fraser    Changed to join to vw_fLatestBond instead of Bond.
/// 23/05/07    Craig Fraser    NCA Changes.
///                             1. Make the "Qualified Credit Limit" textbox editable by default.
///                             2. Hide the "Override Application" button
///                             3. Hide the "Refusal Message"
///                             3. Show the "Override Reason" textbox but make this non-mandatory
///                             4. Hide the "View Limit Matrix" button
#endregion

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
 
public partial class BarclayCardApp_Old_aspx : System.Web.UI.Page
{
	private string sConnectionString = "";
	private SqlConnection oConn = new SqlConnection();
	private DataSet oDS = new DataSet();
    private DataSet dsClient = new DataSet();
    private string sLoanNumber = "";
    private string sProspectNumber = "";
    private bool bLoanAlreadyRegistered;

    protected void Page_Load(object sender, EventArgs e)
    {
        #region Setup the Connection Strings
        sConnectionString = DBConnection.ConnectionString();
        oGridDataSouce.ConnectionString = sConnectionString;

        oSqlDSACBBank.ConnectionString = sConnectionString;
        oSqlDSACBBranch.ConnectionString = sConnectionString;
        oSqlDSACBType.ConnectionString = sConnectionString;
        oSqlDSAPOType.ConnectionString = sConnectionString;
        oSqlDSPaymentMethod.ConnectionString = sConnectionString;
        oSqlDSCardType.ConnectionString = sConnectionString;
        oSqlDSYesNo.ConnectionString = sConnectionString;
        #endregion

        HyperLink1.NavigateUrl = ConfigurationManager.AppSettings["GoldCardPortalPath"].ToString().Replace("$", "&");
        HyperLink1.Target = ConfigurationManager.AppSettings["GoldCardPortalTarget"].ToString();

        #region "Hide" the checkboxes used for validation
        chkLoanRegisteredMoreThan3Months.Style.Add("display", "none");
        chkDisplayDetailsTable.Style.Add("display", "none");
        chkGoldCardManager.Style.Add("display", "none");
        #endregion

        // If the ReSubmit button was pressed, then reset the SubmitDate
        if (Request.Form[btnReSubmit.UniqueID] != null)
        {
            ReSubmit();
        }

        if (!IsPostBack || Request.Form[btnReSubmit.UniqueID] != null)
        {

            chkGoldCardManager.Checked = BarclayCardFunctions.CheckUserSecurity(Context.User.Identity.Name);

            // Should add to viewstate.
            sLoanNumber = Request["param1"] == "0" ? "" : Request["param1"].Trim();
            sProspectNumber = Request["param0"].Trim();

            if (GetData())
            {
                setFormFields();
                showAppForm();

                SetControlsEditable(false);
            }
            else
                pMessage.Visible = false;

            checkValidationRequirements();
        }
    } 

    protected void ddPayMethod_SelectedIndexChanged(object sender, EventArgs e)
    {
        checkValidationRequirements();
    } 

    protected void ddBank_SelectedIndexChanged(object sender, EventArgs e)
    {
        checkValidationRequirements();
    }

    protected void ddAPOType_SelectedIndexChanged(object sender, EventArgs e)
    {
        checkValidationRequirements();
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        if (btnUpdate.Text == "Edit")
        {
            SetControlsEditable(true);

            // *CF* 02/05/06
            if (txtApplicationSubmittedOn.Text != "")
                chkNotInterested.Enabled = false;
            else
                chkNotInterested.Enabled = true;

            checkValidationRequirements();

            btnUpdate.Text = "Cancel";

        }
        else // The button is in 'Cancel' mode
        {
            sLoanNumber = Request["param1"] == "0" ? "" : Request["param1"].Trim();
            sProspectNumber = Request["param0"].Trim();

            if (GetData())
            {
                setFormFields();
                showAppForm();

                SetControlsEditable(false);
            }

            if (txtReasonOveride.Text == "")
            {
                txtReasonOveride.Visible = false;
                lblOverrideReason.Visible = false;
            }

            btnUpdate.Text = "Edit";
        }

    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        SaveData();

        SetMessage("Your data has successfully been captured");

    }

    protected void btnOveride_Click(object sender, EventArgs e)
    {
        // *CF* 04/05/06 Allow Credit Limit to be changed if overriden
        txtCreditLimit.Enabled = true;
        txtCreditLimit.ForeColor = System.Drawing.Color.Red;

        txtReasonOveride.Visible = true;
        lblOverrideReason.Visible = true;
        oRVOverrideReason.Enabled = true;
        btnOveride.Visible = false;
    }

    protected void bMsgReturn_Click(object sender, EventArgs e)
    {
        showAppForm();

        SetControlsEditable(false);

        btnUpdate_Click(sender, e);

    }


    private void setFormFields()
    {
        #region Set Loan or Prospect - Distinguishes the application forms indentifying number as prospect or loan.
        if (sLoanNumber != "")
        {
            lblProspLoanNum.Text = "Loan Number";
            lblProspLoanNumVal.Text = sLoanNumber;
        }
        else
        {
            lblProspLoanNum.Text = "Prospect Number";
            lblProspLoanNumVal.Text = sProspectNumber;
        }
        #endregion

        ///Updated by SharonM on 1/2/06
        ///
        // IF this is a new applcation, then get the Client/Prospect record and display on the screen. 
        // Also retreive their Banking information from Loan table
        // This info will be saved to the Barclaycard table when the user clicks "Save"
        // If this is an existing application then we use the information already on the Barclaycard table to populate the screen.

        ddCardType.SelectedValue = "10";        //set cardttype to "PRIMARY" since single account holder

        GetClientDetails();

        DataRow oDR = oDS.Tables["BCTable"].Rows[0];

        // If Loan is Un Registered, then display "(Not Registered)" next to the Loan Number
        lblLoanUnregistered.Visible = false;
        try
        {
            if (oDR["LoanRegistered"].ToString() == "0")
                lblLoanUnregistered.Visible = true;
        }
        catch { }

        // check to see if loan has been registered more than 3 months ago
        if (sLoanNumber != "") // added by Craig Fraser 20/09/2006
        {
            DateTime dtOpen = Convert.ToDateTime(dsClient.Tables[0].Rows[0]["LoanOpenDate"]);
            DateTime dt3MonthsAgo = System.DateTime.Now.AddMonths(-3);

            if (dtOpen <= dt3MonthsAgo)
                chkLoanRegisteredMoreThan3Months.Checked = true;
            else
                chkLoanRegisteredMoreThan3Months.Checked = false;
        }

        #region Personal Details
        if (oDR["Salutation"] != System.DBNull.Value)
            txtSalutation.Text = oDR["Salutation"].ToString();
        else
        {
            if (sLoanNumber != "") // This is a Client
                txtSalutation.Text = dsClient.Tables[0].Rows[0]["ClientSalutation"].ToString();
            else
                txtSalutation.Text = dsClient.Tables[0].Rows[0]["ProspectSalutation"].ToString();
        }

        if (oDR["FirstNames"] != System.DBNull.Value)
            txtFirstNames.Text = oDR["FirstNames"].ToString();
        else
        {
            if (sLoanNumber != "") // This is a Client
                txtFirstNames.Text = dsClient.Tables[0].Rows[0]["ClientFirstNames"].ToString();
            else
                txtFirstNames.Text = dsClient.Tables[0].Rows[0]["ProspectFirstNames"].ToString();
        }

        if (oDR["Surname"] != System.DBNull.Value)
            txtSurname.Text = oDR["Surname"].ToString();
        else
        {
            if (sLoanNumber != "") // This is a Client
                txtSurname.Text = dsClient.Tables[0].Rows[0]["ClientSurname"].ToString();
            else
                txtSurname.Text = dsClient.Tables[0].Rows[0]["ProspectSurname"].ToString();
        }

        if (oDR["IDNumber"] != System.DBNull.Value)
            txtIDnumber.Text = oDR["IDNumber"].ToString();
        else
        {
            if (sLoanNumber != "") // This is a Client
                txtIDnumber.Text = dsClient.Tables[0].Rows[0]["ClientIDNumber"].ToString();
            else
                txtIDnumber.Text = dsClient.Tables[0].Rows[0]["ProspectIDNumber"].ToString();
        }

        if (oDR["Salary"] != System.DBNull.Value)
            txtSalary.Text = oDR["Salary"].ToString();
        else
        {
            if (sLoanNumber != "") // This is a Client
                txtSalary.Text = dsClient.Tables[0].Rows[0]["ClientIncome"].ToString();
            else
                txtSalary.Text = dsClient.Tables[0].Rows[0]["ProspectIncome"].ToString();
        }
        
        #endregion

        #region Banking Details
        if (oDR["ACBBank"] != System.DBNull.Value)
        {
            ddBank.SelectedValue = oDR["ACBBank"].ToString();
            ddAccountType.SelectedValue = oDR["ACBType"].ToString();
            txtAccountNum.Text = oDR["AccountNumber"].ToString();

            ddBank.DataBind();

            if (oDR["ACBBranch"].ToString() != "")
            {
                ddBranch.SelectedValue = oDR["ACBBranch"].ToString();
                ddBranch.DataBind();
            }

        }
        else
        {
            if (sLoanNumber != "") // This is a Client
            {
                ddBank.DataBind();
                SetSelectedText(ddBank, dsClient.Tables[0].Rows[0]["ACBBankDescription"].ToString());
                SetSelectedText(ddBranch, dsClient.Tables[0].Rows[0]["ACBBranchDescription"].ToString());
                SetSelectedText(ddAccountType, dsClient.Tables[0].Rows[0]["ACBTypeDescription"].ToString());
                txtAccountNum.Text = dsClient.Tables[0].Rows[0]["LoanACBAccountNumber"].ToString();
            }
        }


        #endregion

        chkNotInterested.Checked = Convert.ToBoolean(oDR["NotInterested"]);

        txtReasonOveride.Text = oDR["OverwriteReason"].ToString();
        if (txtReasonOveride.Text == "")
        {
            txtReasonOveride.Visible = false;
            lblOverrideReason.Visible = false;
        }
        else
        {
            txtReasonOveride.Visible = true;
            lblOverrideReason.Visible = true;
        }

        if (oDR["APOAmount"] != System.DBNull.Value)
            txtAPOAmount.Text = oDR["APOAmount"].ToString();
        else
            txtAPOAmount.Text = "0";

        txtAPODay.Text = oDR["APODay"].ToString();
        txtAPODay.Text = txtAPODay.Text == "0" ? "" : txtAPODay.Text;

        ddGarageCard.SelectedValue = oDR["RequireGarageCard"].ToString();

        ddCardType.SelectedValue = "10";        //set cardttype to "PRIMARY" since single account holder

        ddPayMethod.SelectedValue = oDR["PaymentType"].ToString();
        ddAPOType.SelectedValue = oDR["APOType"].ToString();

        ddGroupProductsSrevices.SelectedValue = oDR["GroupProductsServices"].ToString();
        ddResearchPermission.SelectedValue = oDR["ResearchPermissions"].ToString();
        ddOtherProductsServices.SelectedValue = oDR["OtherCompanies"].ToString();
        ddSignedApp.SelectedValue = oDR["SignedApplication"].ToString();

        txtCreditLimit.Text = oDR["BarclayCardLimit"].ToString();

        // CF 23/05/07
        // setRefusalMsg();

        if (oDR["SubmitDate"] != System.DBNull.Value)
        {
            txtApplicationSubmittedOn.Text = oDR["SubmitDate"].ToString();
            this.btnUpdate.Enabled = false;
            lblNoUpdate.Visible = true;
        }
        else
        {
            txtApplicationSubmittedOn.Text = "";
            this.btnUpdate.Enabled = true;
            lblNoUpdate.Visible = false;
        }
    }

    private void SetSelectedText(DropDownList ddl, string sText)
    {
        try
        {
            ddl.DataBind();
            ListItem li = ddl.Items.FindByText(sText);
            ddl.SelectedValue = li.Value;
        }
        catch
        {
        }

    }

    private void GetClientDetails()
    {
        string sSQL = "";
        if (sLoanNumber != "") // This is a Client
        {
            sSQL = "SELECT Client.ClientSalutation,Client.ClientFirstNames,Client.ClientSurname,Client.ClientIDNumber"
            + ",Client.ClientSpouseIDNumber,Client.ClientIncome,Client.ClientSpouseIncome"
            + ",LoanOpenDate,ACBBankDescription,ACBTypeDescription,ACBBranchCode,ACBBranchDescription,LoanACBAccountNumber FROM Client "
            + "INNER JOIN vw_AllLoans ON vw_AllLoans.ClientNumber = Client.ClientNumber "
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
            oConn.ConnectionString = sConnectionString;
            oConn.Open();
        }

        SqlDataAdapter oDA = new SqlDataAdapter(sSQL, oConn);

        dsClient = new DataSet();
        oDA.Fill(dsClient);

        oConn.Close();

    }

    private void setRefusalMsg()
    {
        if (oDS.Tables["BCTable"].Rows[0]["BarclayCardDeclinedReason"].ToString() != "")
        {
            lblRefusalMessage.Text = string.Format("Refused: {0}", Convert.ToString(oDS.Tables["BCTable"].Rows[0]["BarclayCardDeclinedReason"]));
            lblRefusalMessage.Visible = true;
            txtCreditLimit.Enabled = false;
        }
        else
        {
            lblRefusalMessage.Visible = false;
        }
    } 

    private void SetMessage(string strMessage)
    {
        lblMsg.Text = strMessage;
        pMessage.Visible = true;
        pnlDetails.Visible = false;
        chkDisplayDetailsTable.Checked = false;

        lblProspLoanNum.Visible = false;
        lblProspLoanNumVal.Visible = false;

        btnUpdate.Visible = false;
        btnSave.Visible = false;
    } 

    private bool GetData()
    {
        string sSQL = pageSourceSQL();
        bool bReturn = true;

        oConn.ConnectionString = sConnectionString;
        oConn.Open();

        SqlDataAdapter oDA = new SqlDataAdapter(sSQL, oConn);

        if (oDA.Fill(oDS, "BCTable") == 0)
            bReturn = false;

        oConn.Close();

        return bReturn;
    } 

    private void SaveData()
    {
        string sSQL = pageUpdateSourceSQL();

        oConn.ConnectionString = sConnectionString;
        oConn.Open();

        SqlDataAdapter oDA = new SqlDataAdapter(sSQL, oConn);
        oDA.Fill(oDS, "BCTable");

        DataRow oDR = oDS.Tables["BCTable"].Rows[0];

        oDR["Salutation"] = txtSalutation.Text;
        oDR["FirstNames"] = txtFirstNames.Text;
        oDR["Surname"] = txtSurname.Text;
        oDR["IDNumber"] = txtIDnumber.Text;
        if (txtSalary.Text.Trim() != "") 
            oDR["Salary"] = txtSalary.Text;
        else
            oDR["Salary"] = 0;

         
        if (txtAPOAmount.Text.Trim() != "") 
            oDR["APOAmount"] = txtAPOAmount.Text;
        else
            oDR["APOAmount"] = 0;

        if (txtAPODay.Text.Trim() != "") 
            oDR["APODay"] = txtAPODay.Text;
        else
            oDR["APODay"] = 0;

        oDR["AccountNumber"] = txtAccountNum.Text;
        
        oDR["RequireGarageCard"] = ddGarageCard.SelectedValue;
        oDR["PaymentType"] = ddPayMethod.SelectedValue;
        oDR["APOType"] = ddAPOType.SelectedValue;

        oDR["ACBBank"] = ddBank.SelectedValue;
        oDR["ACBBranch"] = ddBranch.SelectedValue;
        oDR["ACBType"] = ddAccountType.SelectedValue;

        oDR["GroupProductsServices"] = ddGroupProductsSrevices.SelectedValue;
        oDR["ResearchPermissions"] = ddResearchPermission.SelectedValue;
        oDR["OtherCompanies"] = ddOtherProductsServices.SelectedValue;
        oDR["SignedApplication"] = ddSignedApp.SelectedValue;

        // UserID is used to hold the userid of the consultant who created the application and should
        // therefore not be overwritten when amendments are made
        if (oDR["UserID"] == System.DBNull.Value || oDR["UserID"].ToString() == "")
            oDR["UserID"] = Context.User.Identity.Name.ToLower().Replace("sahl\\", "");
 
        oDR["AmendedBy"] = Context.User.Identity.Name.ToLower().Replace("sahl\\", "");
        oDR["AmendedDate"] = System.DateTime.Now ;

        oDR["NotInterested"] = chkNotInterested.Checked;

        oDR["OverwriteReason"] = txtReasonOveride.Text;
        if (txtReasonOveride.Text != "")
        {
            oDR["BarclayCardLimit"] = Convert.ToDouble(txtCreditLimit.Text.ToString());
            oDR["Overwrite"] = true;
        }
        else
            oDR["Overwrite"] = false;

        SqlCommandBuilder oSCMD = new SqlCommandBuilder(oDA);
        oDA.Update(oDS.Tables["BCTable"]);

        if (lblProspLoanNum.Text == "Loan Number")
            sLoanNumber = lblProspLoanNumVal.Text;
        else
            sProspectNumber = lblProspLoanNumVal.Text;

        bLoanAlreadyRegistered = false;
        if (sLoanNumber != "") // client
        {
            GetClientDetails();
            if (dsClient.Tables[0].Rows.Count > 0)
                bLoanAlreadyRegistered = true;
        }


        // Call the Stored Procedure to recalculate the credit limit for the Principal Bond Holder if necessary
        // This must be not be called in the following instances.
        // 1. When the Application has been Overridden
        // 2. When a signed application has been received
        // 3. When the clients loan has been registered
        if (txtReasonOveride.Text.Trim().Length == 0 && ddSignedApp.SelectedValue != "1" && !bLoanAlreadyRegistered)
            RecalculateCreditLimit();

        oConn.Close();
    }

    private bool RecalculateCreditLimit()
    {
        string sStoredProc = "";
        try
        {
            sStoredProc = "bc_AddUpdateBarclayCardRecord";

            SqlCommand com = new SqlCommand(string.Format("exec " + sStoredProc + " {0}, '{1}', 10",
                        sProspectNumber == "" ? sLoanNumber : sProspectNumber,
                        sProspectNumber == "" ? "Client" : "Prospect"), oConn);

            com.ExecuteNonQuery();
        }
        catch
        {
            return false;
        }
        return true;
    }

    void showAppForm()
    {
        pnlDetails.Visible = true;
        chkDisplayDetailsTable.Checked = true;

        pMessage.Visible = false;

        lblProspLoanNum.Visible = true;
        lblProspLoanNumVal.Visible = true;

        btnUpdate.Visible = true;
        btnSave.Visible = true;    
    } 

    private string pageSourceSQL()
    {
        string sLoanNum = sLoanNumber == "" ? "null" : sLoanNumber;
        string sProspNum = sProspectNumber == "" ? "null" : sProspectNumber;

        return string.Format("select barclaycardkey,isnull(RequireGarageCard, -1) as RequireGarageCard," +
                                    "BarclayCardLimit," +
                                    "Salutation," +
                                    "FirstNames," +
                                    "Surname," +
                                    "IDNumber," +
                                    "Salary," +
                                    "SecondaryBondHolder," +
                                    "isnull(CardType, 10) as CardType," +
                                    "isnull(PaymentType, -1) as PaymentType," +
                                    "isnull(APOType, -1) as APOType," +
                                    "isnull(APOAmount, 0) as APOAmount," +
                                    "isnull(APODay, 0) as APODay," +
                                    "ACBBank," +
                                    "isnull(ACBBranch, -1) as ACBBranch," +
                                    "isnull(ACBType, -1) as ACBType," +
                                    "AccountNumber," +
                                    "SubmitDate," +
                                    "isnull(GroupProductsServices, -1) as GroupProductsServices," +
                                    "isnull(ResearchPermissions, -1) as ResearchPermissions," +
                                    "isnull(OtherCompanies, -1) as OtherCompanies," +
                                    "isnull(SignedApplication, -1) as SignedApplication," +
                                    "BarclayCardDeclinedReason," +
                                    "isnull(NotInterested, 0) as NotInterested," +
                                    "isnull(Overwrite, 0) as Overwrite," +
                                    "OverwriteReason," +
                                    "UserID," +
                                    "AmendedBy," +
                                    "AmendedDate, " +
                                    "case when b.LoanNumber is null then 0 else 1 end as LoanRegistered " + 
                                    "from BarclayCard bc " +
                                    "left outer join vw_fLatestBond b on b.LoanNumber = bc.LoanNumber " + 
                                    "where convert(varchar, ProspectNumber ) = '{0}' " +
                                    "or convert(varchar, bc.LoanNumber) = '{1}'", sProspNum, sLoanNum);

       
    
    
    } 

    private string pageUpdateSourceSQL()
    {
        string sLoanNum = Request["param1"] == "0" ? "" : Request["param1"].Trim();
        sLoanNum = sLoanNum == "" ? "null" : sLoanNum;

        string sProspNum = Request.QueryString["param0"] == "" ? "null" : Request.QueryString["param0"].Trim();

        return string.Format("select * from BarclayCard " +
                             "where convert(varchar,ProspectNumber) = '{0}' " +
                             "or convert(varchar, LoanNumber) = '{1}'", sProspNum, sLoanNum);
    } 

    private void checkValidationRequirements()
    {
        if (ddAPOType.SelectedValue == "8")
            oRfvAPOAmount.Enabled = true;
        else
            oRfvAPOAmount.Enabled = false;

        if (ddAPOType.SelectedValue == "7" ||
            ddAPOType.SelectedValue == "8" ||
            ddAPOType.SelectedValue == "9")
        {
            setValidationControls(true);
        }
        else
        {
            setValidationControls(false);
        }
    } 

    private void setValidationControls(bool a_Enabled)
    {
        oRVBank.Enabled = a_Enabled;
        oRVBranch.Enabled = a_Enabled;
        oRVAccountType.Enabled = a_Enabled;
        oRfvAPODay.Enabled = a_Enabled;
        oRfvAccountNumber.Enabled = a_Enabled;
    } 

    private void SetControlsEditable(bool bEditable)
    {
        btnSave.Enabled = bEditable;

        // *CF* 02/05/06
        if (bEditable && chkNotInterested.Checked == true)
            bEditable = false;

        EnableControls(pnlDetails, bEditable);

        // always set the credit limit to disabled unless they press the override button
        //txtCreditLimit.Enabled = false;

        // /Override button shoud only be displayed if:
        // 1) User is Authorised to Override (They belong to GoldCardManager AD Group)
        // 2) The application has not been submitted to the bank
        // 3) The Credit Limit is Zero - Allow amount and reason to be changed
        // 4) The loan is more than 3 months old - if the credit limit is not 0 then onyl allow reason to be changed
        //if (bEditable == true && chkGoldCardManager.Checked && txtApplicationSubmittedOn.Text == "" && (txtCreditLimit.Text == "0" || chkLoanRegisteredMoreThan3Months.Checked))
        //    btnOveride.Visible = true;
        //else
        //    btnOveride.Visible = false;

        oRVOverrideReason.Enabled = false;

        txtSalary.Enabled = false;
        ddCardType.Enabled = false;

        // Re-Submit button shoud only be displayed if:
        // 1) User is Authorised to Re-Submit (They belong to GoldCardManager AD Group)
        // 2) The application has already been submitted to the bank
        if (chkGoldCardManager.Checked && txtApplicationSubmittedOn.Text != "")
            btnReSubmit.Visible = true;
        else
            btnReSubmit.Visible = false;

    }

    private void EnableControls(Panel pPanel, bool bEnabled)
    {
        try
        {
            foreach (Control c in pPanel.Controls)
            {
                System.Type type = c.GetType();
                switch (type.Name)
                {
                    case "TextBox":
                        TextBox t = (TextBox)c;
                        t.Enabled = bEnabled;
                        break;
                    case "DropDownList":
                        DropDownList d = (DropDownList)c;
                        d.Enabled = bEnabled;
                        break;
                    case "CheckBox":
                        CheckBox cb = (CheckBox)c;
                        cb.Enabled = bEnabled;
                        break;

                    case "RequiredFieldValidator":
                        RequiredFieldValidator rv = (RequiredFieldValidator)c;
                        rv.Enabled = bEnabled;
                        break;
                    case "RangeValidator":
                        RangeValidator rav = (RangeValidator)c;
                        rav.Enabled = bEnabled;
                        break;
                    case "CompareValidator":
                        CompareValidator cv = (CompareValidator)c;
                        cv.Enabled = bEnabled;
                        break;

                    case "CustomValidator":
                        CustomValidator cuv = (CustomValidator)c;
                        cuv.Enabled = bEnabled;
                        break;

                    default:
                        break;
                }
            }
        }
        catch { }
    }

    protected void chkNotInterested_CheckedChanged(object sender, EventArgs e)
    {
        bool bEnable = !chkNotInterested.Checked;

        // Disable all fields except the chkNotInterested
        EnableControls(pnlDetails, bEnable);
        //EnableControls(pnlDetails1b, bEnable);
        //EnableControls(pnlDetails1c, bEnable);
        //btnOveride.Enabled = bEnable;

        chkNotInterested.Enabled = true;

        checkValidationRequirements(); // 15-06-2006 CF

    }
    
    private void ReSubmit()
    {
        string sLoanNum = Request["param1"] == "0" ? "" : Request["param1"].Trim();
        sLoanNum = sLoanNum == "" ? "null" : sLoanNum;

        string sProspNum = Request.QueryString["param0"] == "" ? "null" : Request.QueryString["param0"].Trim();

        string sSQL = string.Format("update [SAHLDB]..BarclayCard set SubmitDate = null " +
                             "where convert(varchar,ProspectNumber) = '{0}' " +
                             "or convert(varchar, LoanNumber) = '{1}'", sProspNum, sLoanNum);

        try        
        {
            oConn.ConnectionString = sConnectionString;
            oConn.Open();

            SqlCommand oCmd = new SqlCommand(sSQL,oConn);
            int irows = oCmd.ExecuteNonQuery();
            oCmd.Transaction.Commit();
        }
        catch 
        {
        }
        oConn.Close();
    }
}
