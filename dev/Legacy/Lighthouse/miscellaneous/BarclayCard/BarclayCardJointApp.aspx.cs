#region Amendment History
/// 
/// Date        Amended By      Description
/// ----        ----------      -----------
/// 12/04/06    Craig Fraser    Enable/Disable Edit Button
///                             If client has elected one primary and one secondary card, and application 
///                             has been submitted to Bank then disable edit button. If client has 
///                             elected 2 primary cards and only one application has been submitted to 
///                             bank then application not yet submitted to bank should be editable but 
///                             application submitted to bank should not be editable
/// 02/05/06    Craig Fraser    If there are no secondary bond holder details then check the "not interested" 
///                             checkbox and disable all other secondary bond holder fields. Also moved "not interested" checkbox 
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
/// 19/12/06    Craig Fraser    Changed to join to vw_fLatestBond instead of Bond.
/// 23/05/07    Craig Fraser    NCA Interim Changes.
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
using System.Collections;

public partial class BarclayCardJointApp_aspx : System.Web.UI.Page
{
    private string sConnectionString = "";
    private SqlConnection oConn = new SqlConnection();
    private DataSet dsBarclayCard = new DataSet();
    private DataSet dsClient = new DataSet();
    private string sLoanNumber = "";
    private string sProspectNumber = "";
    private bool bNewApplication, bLoanAlreadyRegistered, bPrimaryCard,bSecondaryCard;
    private string sSecondaryMsg = "All Details below as per Primary Card Holder";
    private int iPrimaryCardCount,iSecondaryCardCount, iPrimaryCardSubmittedCount, iSecondaryCardSubmittedCount;

    protected void Page_Load(object sender, EventArgs e)
    {
        #region Setup the Connection Strings
        sConnectionString = DBConnection.ConnectionString();

        //sConnectionString = "Server=SAHLS303;Trusted_Connection=False;Database=SAHLDB;User=CraigF;Password=";


        //Sets all the applications SqlDataSource objects connection string properties.
        oSqlDSMatrix.ConnectionString = sConnectionString;
        oSqlDSACBBank.ConnectionString = sConnectionString;
        oSqlDSACBBranch.ConnectionString = sConnectionString;
        oSqlDSACBBranch2.ConnectionString = sConnectionString;
        oSqlDSACBType.ConnectionString = sConnectionString;
        oSqlDSAPOType.ConnectionString = sConnectionString;
        oSqlDSPaymentMethod.ConnectionString = sConnectionString;
        oSqlDSYesNo.ConnectionString = sConnectionString;
        oSqlDSCardType.ConnectionString = sConnectionString;
        #endregion

        HyperLink1.NavigateUrl = ConfigurationManager.AppSettings["GoldCardPortalPath"].ToString().Replace("$", "&");
        HyperLink1.Target = ConfigurationManager.AppSettings["GoldCardPortalTarget"].ToString();


        CreateAndRegisterJavaScripts();

        #region "Hide" the checkboxes used for validation
        chkLoanRegisteredMoreThan3Months.Style.Add("display", "none");
        chkDisplayDetailsTable.Style.Add("display", "none");
        chkGoldCardManager.Style.Add("display", "none");
        #endregion

        // If the ReSubmit button was pressed, then reset the SubmitDate
        if (Request.Form[btnReSubmit.UniqueID] != null)
            ReSubmit();

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

                SetControlsEditable(false,false);
            }
            else
                pMessage.Visible = false;

            checkValidationRequirementsPrincipal();
            checkValidationRequirementsSecondary();

        }
    }

    protected void ddAPOType_SelectedIndexChanged(object sender, EventArgs e)
    {
        checkValidationRequirementsPrincipal();
    }

    protected void ddAPOType2_SelectedIndexChanged(object sender, EventArgs e)
    {
        checkValidationRequirementsSecondary();
    }

    protected void ddPayMethod_SelectedIndexChanged(object sender, EventArgs e)
    {
        checkValidationRequirementsPrincipal();
    }

    protected void ddPayMethod2_SelectedIndexChanged(object sender, EventArgs e)
    {
        checkValidationRequirementsSecondary();
    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        if (btnEdit.Text == "Edit")
        {
            SetControlsEditable(true,true);

            // *CF* 02/05/06
            if (txtSubmittedDate.Text != "")
                chkNotInterested.Enabled = false;
            else
                chkNotInterested.Enabled = true;

            if (txtSubmittedDate2.Text != "")
                chkNotInterested2.Enabled = false;
            else
                chkNotInterested2.Enabled = true;

            checkValidationRequirementsPrincipal();
            checkValidationRequirementsSecondary();

            btnEdit.Text = "Cancel";
        }
        else // The button is in 'Cancel' mode
        {
            sLoanNumber = Request["param1"] == "0" ? "" : Request["param1"].Trim();
            sProspectNumber = Request["param0"].Trim();

            if (GetData())
            {
                setFormFields();
                showAppForm();

                SetControlsEditable(false,false);
            }

            //if (txtOverrideReason2.Text == "")
            //{
            //    txtOverrideReason2.Visible = false;
            //    lblOverrideReason2.Visible = false;
            //}
            //if (txtOverrideReason.Text == "")
            //{
            //    txtOverrideReason.Visible = false;
            //    lblOverrideReason.Visible = false;
            //}

            btnEdit.Text = "Edit";
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        SaveData();

        SetMessage("Your data has successfully been captured");
    }

    protected void ddCardType_SelectedIndexChanged(object sender, EventArgs e)
    {
        oCVCardType.Enabled = true;

        switch (ddCardType.SelectedValue)
        {
            case "-1":
                oCVCardType.IsValid = false;
                oCVCardType.ErrorMessage = "Card Type must be Selected";
                break;
            case "11":
                if (ddCardType2.SelectedValue == "11")
                {
                    oCVCardType.ErrorMessage = "Secondary Card Holder already assigned";
                    oCVCardType.IsValid = false;
                }
                else
                {
                    DisplaySecondaryCardHolderFields(enums.enBondHolderType.Principal, false);
                    oCVCardType.Enabled = false;
                    oCVCardType.IsValid = true;
                }
                break;
            case "10":
                DisplaySecondaryCardHolderFields(enums.enBondHolderType.Principal, true);
                break;
        }
    }

    protected void ddCardType2_SelectedIndexChanged(object sender, EventArgs e)
    {
        oCVCardType2.Enabled = true;

        switch (ddCardType2.SelectedValue)
        {
            case "-1":
                oCVCardType2.IsValid = false;
                oCVCardType2.ErrorMessage = "Card Type must be Selected";
                break;
            case "11":
                if (ddCardType.SelectedValue == "11")
                {
                    oCVCardType2.ErrorMessage = "Secondary Card Holder already assigned";
                    oCVCardType2.IsValid = false;
                }
                else
                {
                    DisplaySecondaryCardHolderFields(enums.enBondHolderType.Secondary, false);
                    oCVCardType2.Enabled = false;
                    oCVCardType2.IsValid = true;
                }
                break;
            case "10":
                DisplaySecondaryCardHolderFields(enums.enBondHolderType.Secondary, true);
                break;

        }
    }

    protected void ddSignedApp_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddCardType2.SelectedValue == "11") // Secondary
        {
            ddSignedApp2.SelectedIndex = ddSignedApp.SelectedIndex;
        }

    }

    protected void ddSignedApp2_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddCardType.SelectedValue == "11") // Secondary
        {
            ddSignedApp.SelectedIndex = ddSignedApp2.SelectedIndex;
        }

    }

    protected void bMsgReturn_Click(object sender, EventArgs e)
    {
        showAppForm();

        SetControlsEditable(false,false);

        btnEdit_Click(sender, e);

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

        // If this is a new application, then get the Client/Prospect record and split the Principal/Secondary bond holders
        // information out and display on the screen. Also retreive their Banking information.
        // This info will be saved to the Barclaycard table when the used clicks "Save"
        // If this is an existing application then we use the information already on the Barclaycard table to populate the screen.
        if (dsBarclayCard.Tables["BCTable"].Rows[0]["FirstNames"] == System.DBNull.Value
         || dsBarclayCard.Tables["BCTable"].Rows[0]["FirstNames"].ToString() == "")
            SetNewClientInformation();
        else
            GetClientDetails();

        // If Loan is Un Registered, then display "(Not Registered)" next to the Loan Number
        DataRow r = dsBarclayCard.Tables["BCTable"].Rows[0];
        lblLoanUnregistered.Visible = false;
        try
        {
            if (r["LoanRegistered"].ToString() == "0")
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

        // Should be 2 rows  - one for Principal and one for Secondary Bond Holder
        iPrimaryCardCount = 0; iSecondaryCardCount = 0;
        iPrimaryCardSubmittedCount = 0; iSecondaryCardSubmittedCount = 0;
        int iRow = 0;
        foreach (DataRow oDR in dsBarclayCard.Tables["BCTable"].Rows)
        {
            bPrimaryCard=false;
            bSecondaryCard=false;

            object oSecondaryBondHolder = oDR["SecondaryBondHolder"];
            if (oSecondaryBondHolder != System.DBNull.Value && (bool)oSecondaryBondHolder == true) 
            {
                #region Secondary Bond Holder

                txtBarclayCardKey2.Text = oDR["BarclayCardkey"].ToString();

                ddCardType2.SelectedValue = oDR["CardType"].ToString();
                switch (oDR["CardType"].ToString())
                {
                    case "-1":
                    case "11": // Secondary Card Holder
                        ddCardType2.SelectedValue = "11"; 
                        lblSecondaryMsg2.Text = sSecondaryMsg;
                        bSecondaryCard=true;
                        iSecondaryCardCount++;
                        break;
                    case "10": // Primary Card Holder
                        lblSecondaryMsg2.Text = "";
                        bPrimaryCard=true;
                        iPrimaryCardCount++;
                        break;
                }

                if (!bNewApplication)
                {
                    #region Existing Application
                    txtSalutation2.Text = oDR["Salutation"].ToString();
                    txtFirstName2.Text = oDR["FirstNames"].ToString();
                    txtSurname2.Text = oDR["Surname"].ToString();
                    txtIDNumber2.Text = oDR["IDNumber"].ToString();
                    txtSalary2.Text = oDR["Salary"].ToString();
                    chkNotInterested2.Checked = Convert.ToBoolean(oDR["NotInterested"]);
                    txtOverrideReason2.Text = oDR["OverwriteReason"].ToString();
                    //if (txtOverrideReason2.Text == "")
                    //{
                    //    txtOverrideReason2.Visible = false;
                    //    lblOverrideReason2.Visible = false;
                    //}
                    //else 
                    //{
                    //    txtOverrideReason2.Visible = true;
                    //    lblOverrideReason2.Visible = true;
                    //}

                    if (lblSecondaryMsg2.Text == "")
                    {
                        txtAPOAmount2.Text = oDR["APOAmount"].ToString();
                        txtAPODay2.Text = oDR["APODay"].ToString();
                        txtAPODay2.Text = txtAPODay2.Text == "0" ? "" : txtAPODay2.Text;
                        txtAccountNum2.Text = oDR["AccountNumber"].ToString();

                        ddGarageCard2.SelectedValue = oDR["RequireGarageCard"].ToString();
                        ddPayMethod2.SelectedValue = oDR["PaymentType"].ToString();
                        ddAPOType2.SelectedValue = oDR["APOType"].ToString();
                        ddBank2.SelectedValue = oDR["ACBBank"].ToString();
                        ddBank2.DataBind();

                        if (oDR["ACBBranch"].ToString() != "")
                        {
                            ddBranch2.SelectedValue = oDR["ACBBranch"].ToString();
                            ddBranch2.DataBind();
                        }

                        ddAccountType2.SelectedValue = oDR["ACBType"].ToString();
                        ddGroupProductsServices2.SelectedValue = oDR["GroupProductsServices"].ToString();
                        ddResearchPermission2.SelectedValue = oDR["ResearchPermissions"].ToString();
                        ddOtherProductsServices2.SelectedValue = oDR["OtherCompanies"].ToString();
                    }

                    ddSignedApp2.SelectedValue = oDR["SignedApplication"].ToString();
                    #endregion
                }

                txtCreditLimit2.Text = oDR["BarclayCardLimit"].ToString();

                // CF 23/05/07
                //setRefusalMsg(true, iRow);

                if (oDR["SubmitDate"] != System.DBNull.Value)
                {
                    // *CF* 12/04/06
                    if (bSecondaryCard)
                        iSecondaryCardSubmittedCount++;
                    else if (bPrimaryCard)
                        iPrimaryCardSubmittedCount++;

                    txtSubmittedDate2.Text = oDR["SubmitDate"].ToString();
                }
                else
                    txtSubmittedDate2.Text = "";

                // *CF* 02/05/06
                if (txtFirstName2.Text.Length<1)
                    chkNotInterested2.Checked = true;

                #endregion
            }
            else 
            {
                #region Principal Bond Holder

                txtBarclayCardKey1.Text = oDR["BarclayCardkey"].ToString();

                ddCardType.SelectedValue = oDR["CardType"].ToString();
                switch (oDR["CardType"].ToString())
                {
                    case "11": // Secondary Card Holder
                        lblSecondaryMsg.Text = sSecondaryMsg;
                        bSecondaryCard = true;
                        iSecondaryCardCount++;
                        break;
                    case "-1":
                    case "10": // Primary Card Holder
                        ddCardType.SelectedValue = "10";
                        lblSecondaryMsg.Text = "";
                        bPrimaryCard = true;
                        iPrimaryCardCount++;
                        break;
                }

                if (!bNewApplication)
                {
                    #region Existing Application
                    txtSalutation.Text = oDR["Salutation"].ToString();
                    txtFirstName.Text = oDR["FirstNames"].ToString();
                    txtSurname.Text = oDR["Surname"].ToString();
                    txtIDNumber.Text = oDR["IDNumber"].ToString();
                    txtSalary.Text = oDR["Salary"].ToString();
                    chkNotInterested.Checked = Convert.ToBoolean(oDR["NotInterested"]);
                    txtOverrideReason.Text = oDR["OverwriteReason"].ToString();
                    //if (txtOverrideReason.Text == "")
                    //{
                    //    txtOverrideReason.Visible = false;
                    //    lblOverrideReason.Visible = false;
                    //}
                    //else
                    //{
                    //    txtOverrideReason.Visible = true;
                    //    lblOverrideReason.Visible = true;
                    //}

                    if (lblSecondaryMsg.Text == "")
                    {
                        txtAPOAmount.Text = oDR["APOAmount"].ToString();
                        txtAPODay.Text = oDR["APODay"].ToString();
                        txtAPODay.Text = txtAPODay.Text == "0" ? "" : txtAPODay.Text;
                        txtAccountNum.Text = oDR["AccountNumber"].ToString();

                        ddGarageCard.SelectedValue = oDR["RequireGarageCard"].ToString();
                        ddPayMethod.SelectedValue = oDR["PaymentType"].ToString();
                        ddAPOType.SelectedValue = oDR["APOType"].ToString();
                        ddBank.SelectedValue =  oDR["ACBBank"].ToString();
                        //ddBank.SelectedValue = Convert.ToInt32(oDR["ACBBank"]) < 0 ? "0" : oDR["ACBBank"].ToString();
                        ddBank.DataBind();

                        if (oDR["ACBBranch"].ToString() != "")
                        {
                            ddBranch.SelectedValue = oDR["ACBBranch"].ToString();
                            ddBranch.DataBind();
                        }

                        ddAccountType.SelectedValue = oDR["ACBType"].ToString();
                        ddGroupProductsServices.SelectedValue = oDR["GroupProductsServices"].ToString();
                        ddResearchPermission.SelectedValue = oDR["ResearchPermissions"].ToString();
                        ddOtherProductsServices.SelectedValue = oDR["OtherCompanies"].ToString();
                    }
                    ddSignedApp.SelectedValue = oDR["SignedApplication"].ToString();
                    #endregion
                }

                txtCreditLimit.Text = oDR["BarclayCardLimit"].ToString();

                // CF 23/05/07
                //setRefusalMsg(false, iRow);

                if (oDR["SubmitDate"] != System.DBNull.Value)
                {
                    // *CF* 12/04/06
                    if (bSecondaryCard)
                        iSecondaryCardSubmittedCount++;
                    else if (bPrimaryCard)
                        iPrimaryCardSubmittedCount++;

                    txtSubmittedDate.Text = oDR["SubmitDate"].ToString();
                }
                else
                    txtSubmittedDate.Text = "";

                // *CF* 02/05/06
                if (txtFirstName.Text.Length<1)
                    chkNotInterested.Checked = true;

                #endregion
            }
            iRow++;
        }

        #region Enable / Disable Edit Button *CF* 12/04/06
        if (iPrimaryCardCount == 2) // Both applications are for primary cards
        {
            // If both primary card applications have been submitted then dont allow editing.
            // If onyl one has been submitted then allow editing but only on the one that hasnt been submitted
            if (iPrimaryCardSubmittedCount == 2)
            {
                this.btnEdit.Enabled = false;
                lblEditMessage.Visible = true;
            }
            else
            {
                this.btnEdit.Enabled = true;
                lblEditMessage.Visible = false;
            }

        }
        else // One primary and one secondary card
        {
            // If either application has been submitted to bank then dont allow editing
            if (iPrimaryCardSubmittedCount > 0 || iSecondaryCardSubmittedCount > 0)
            {
                this.btnEdit.Enabled = false;
                lblEditMessage.Visible = true;
            }
            else
            {
                this.btnEdit.Enabled = true;
                lblEditMessage.Visible = false;
            }

        }
        #endregion
    }

    private void DisplaySecondaryCardHolderFields(enums.enBondHolderType enType, bool bEnabled)
    {
        switch (enType)
        {
            case enums.enBondHolderType.Principal:
                ddGarageCard.Enabled = bEnabled;
                ddPayMethod.Enabled = bEnabled;
                ddAPOType.Enabled = bEnabled;
                txtAPOAmount.Enabled = bEnabled;
                txtAPODay.Enabled = bEnabled;
                ddBank.Enabled = bEnabled;
                ddBranch.Enabled = bEnabled;
                ddAccountType.Enabled = bEnabled;
                txtAccountNum.Enabled = bEnabled;
                ddGroupProductsServices.Enabled = bEnabled;
                ddResearchPermission.Enabled = bEnabled;
                ddOtherProductsServices.Enabled = bEnabled;
                ddSignedApp.Enabled = bEnabled;

                break;
            case enums.enBondHolderType.Secondary:
                ddGarageCard2.Enabled = bEnabled;
                ddPayMethod2.Enabled = bEnabled;
                ddAPOType2.Enabled = bEnabled;
                txtAPOAmount2.Enabled = bEnabled;
                txtAPODay2.Enabled = bEnabled;
                ddBank2.Enabled = bEnabled;
                ddBranch2.Enabled = bEnabled;
                ddAccountType2.Enabled = bEnabled;
                txtAccountNum2.Enabled = bEnabled;
                ddGroupProductsServices2.Enabled = bEnabled;
                ddResearchPermission2.Enabled = bEnabled;
                ddOtherProductsServices2.Enabled = bEnabled;
                ddSignedApp2.Enabled = bEnabled;

                break;

        }
    }

    private void SetNewClientInformation()
    {
        bNewApplication = true;

        #region Get Client / Prospect master record and Banking Details (No bank details for Prospect)
        GetClientDetails();
        #endregion

        DataRow drClient = dsClient.Tables[0].Rows[0];

        if (sLoanNumber != "") // This is a Client
        {
            #region Populate Client Personal Details on the page
            string[] sSalutation = drClient["ClientSalutation"].ToString().Split('&');
            txtSalutation.Text = sSalutation[0].Trim();
            if (sSalutation.Length > 1)
                txtSalutation2.Text = sSalutation[1].Trim();

            string[] sFirstNames = drClient["ClientFirstNames"].ToString().Split('&');
            txtFirstName.Text = sFirstNames[0].Trim();
            if (sFirstNames.Length > 1)
                txtFirstName2.Text = sFirstNames[1].Trim();

            string[] sSurname = drClient["ClientSurname"].ToString().Split('&');
            txtSurname.Text = sSurname[0].Trim();
            if (sSurname.Length > 1)
                txtSurname2.Text = sSurname[1].Trim();
            else
                txtSurname2.Text = sSurname[0].Trim();

            txtIDNumber.Text = drClient["ClientIDNumber"].ToString();
            txtIDNumber2.Text = drClient["ClientSpouseIDNumber"].ToString();

            txtSalary.Text = drClient["ClientIncome"].ToString();
            txtSalary2.Text = drClient["ClientSpouseIncome"].ToString();
            #endregion

            #region Populate Client Banking Details on the page
            SetSelectedText(ddBank, drClient["ACBBankDescription"].ToString());
            SetSelectedText(ddBranch, drClient["ACBBranchDescription"].ToString());
            SetSelectedText(ddAccountType, drClient["ACBTypeDescription"].ToString());
            txtAccountNum.Text = drClient["LoanACBAccountNumber"].ToString();

            #endregion
        }
        else // This is Prospect
        {
            #region Populate Prospect Personal Details on the page
            string[] sSalutation = drClient["ProspectSalutation"].ToString().Split('&');
            txtSalutation.Text = sSalutation[0].Trim();
            if (sSalutation.Length > 1)
                txtSalutation2.Text = sSalutation[1].Trim();

            string[] sFirstNames = drClient["ProspectFirstNames"].ToString().Split('&');
            txtFirstName.Text = sFirstNames[0].Trim();
            if (sFirstNames.Length > 1)
                txtFirstName2.Text = sFirstNames[1].Trim();

            string[] sSurname = drClient["ProspectSurname"].ToString().Split('&');
            txtSurname.Text = sSurname[0].Trim();
            if (sSurname.Length > 1)
                txtSurname2.Text = sSurname[1].Trim();
            else
                txtSurname2.Text = sSurname[0].Trim();

            txtIDNumber.Text = drClient["ProspectIDNumber"].ToString();
            txtIDNumber2.Text = drClient["ProspectSpouseIDNumber"].ToString();

            txtSalary.Text = drClient["ProspectIncome"].ToString();
            txtSalary2.Text = drClient["ProspectSpouseIncome"].ToString();

            #endregion
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

    private void SetMessage(string strMessage)
    {
        lblMsg.Text = strMessage;
        pMessage.Visible = true;
        ShowDetailPanels(false);
        lblProspLoanNum.Visible = false;
        lblProspLoanNumVal.Visible = false;

    }
    private void ShowDetailPanels(bool bVisible)
    {
        chkDisplayDetailsTable.Checked = bVisible;

        pnlDetails1.Visible = bVisible;
        pnlDetails1b.Visible = bVisible;
        pnlDetails1c.Visible = bVisible;

        pnlDetails2.Visible = bVisible;
        pnlDetails2b.Visible = bVisible;
        pnlDetails2c.Visible = bVisible;

        lblPrincipal.Visible = bVisible;
        lblSecondary.Visible = bVisible;
        lblSigned.Visible = bVisible;
        btnEdit.Visible = bVisible;
        btnSave.Visible = bVisible;


    }

    private bool GetData()
    {
        string sSQL = pageSourceSQL();
        bool bReturn = true;

        oConn.ConnectionString = sConnectionString;
        oConn.Open();

        SqlDataAdapter oDA = new SqlDataAdapter(sSQL, oConn);

        if (oDA.Fill(dsBarclayCard, "BCTable") == 0)
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
        oDA.Fill(dsBarclayCard, "BCTable");

        // Should be 2 rows  - one for Principal and one for Secondary Bond Holder
        foreach (DataRow oDR in dsBarclayCard.Tables["BCTable"].Rows)
        {
            object oSecondCardHolder = oDR["SecondaryBondHolder"];
            if (oSecondCardHolder != System.DBNull.Value && (bool)oSecondCardHolder == true) // Secondary Card Holder
            {
                #region Secondary Bond Holder
                oDR["Salutation"] = txtSalutation2.Text;
                oDR["FirstNames"] = txtFirstName2.Text;
                oDR["Surname"] = txtSurname2.Text;
                oDR["IDNumber"] = txtIDNumber2.Text;
                if (txtSalary2.Text.Trim() != "") 
                    oDR["Salary"] = txtSalary2.Text;
                else
                    oDR["Salary"] = 0;


                oDR["CardType"] = ddCardType2.SelectedValue;
                oDR["SecondaryBondHolder"] = true;

                // If the guy is a secondary card holder then store the details from the primary card holder record
                if (ddCardType2.SelectedValue == "11") // Secondary Card Holder
                {
                    oDR["BarclayCardLimit"] = txtCreditLimit.Text;

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

                    oDR["GroupProductsServices"] = ddGroupProductsServices.SelectedValue;
                    oDR["ResearchPermissions"] = ddResearchPermission.SelectedValue;
                    oDR["OtherCompanies"] = ddOtherProductsServices.SelectedValue;
                }
                else
                {
                    if (txtAPOAmount2.Text.Trim() != "")
                        oDR["APOAmount"] = txtAPOAmount2.Text;
                    else
                        oDR["APOAmount"] = 0;

                    if (txtAPODay2.Text.Trim() != "")
                        oDR["APODay"] = txtAPODay2.Text;
                    else
                        oDR["APODay"] = 0;

                    oDR["AccountNumber"] = txtAccountNum2.Text;

                    oDR["RequireGarageCard"] = ddGarageCard2.SelectedValue;
                    oDR["PaymentType"] = ddPayMethod2.SelectedValue;
                    oDR["APOType"] = ddAPOType2.SelectedValue;

                    oDR["ACBBank"] = ddBank2.SelectedValue;
                    oDR["ACBBranch"] = ddBranch2.SelectedValue;
                    oDR["ACBType"] = ddAccountType2.SelectedValue;

                    oDR["GroupProductsServices"] = ddGroupProductsServices2.SelectedValue;
                    oDR["ResearchPermissions"] = ddResearchPermission2.SelectedValue;
                    oDR["OtherCompanies"] = ddOtherProductsServices2.SelectedValue;
                }

                oDR["SignedApplication"] = ddSignedApp2.SelectedValue;
                // UserID is used to hold the userid of the consultant who created the application and should
                // therefore not be overwritten when amendments are made
                if (oDR["UserID"] == System.DBNull.Value || oDR["UserID"].ToString() == "")
                    oDR["UserID"] = Context.User.Identity.Name.ToLower().Replace("sahl\\", "");

                oDR["AmendedBy"] = Context.User.Identity.Name.ToLower().Replace("sahl\\", "");
                oDR["AmendedDate"] = System.DateTime.Now;

                oDR["NotInterested"] = chkNotInterested2.Checked;

                oDR["OverwriteReason"] = txtOverrideReason2.Text;
                oDR["BarclayCardLimit"] = Convert.ToDouble(txtCreditLimit2.Text.ToString());
                if (txtOverrideReason2.Text != "")
                    oDR["Overwrite"] = true;
                else
                    oDR["Overwrite"] = false;

                #endregion
            }
            else // Principal Bond Holder
            {
                #region Principal Bond Holder
                oDR["Salutation"] = txtSalutation.Text;
                oDR["FirstNames"] = txtFirstName.Text;
                oDR["Surname"] = txtSurname.Text;
                oDR["IDNumber"] = txtIDNumber.Text;
                if (txtSalary.Text.Trim() != "") 
                    oDR["Salary"] = txtSalary.Text;
                else
                    oDR["Salary"] = 0;


                oDR["CardType"] = ddCardType.SelectedValue;
                oDR["SecondaryBondHolder"] = false;

                // If the guy is a secondary card holder then store the details from the primary card holder record
                if (ddCardType.SelectedValue == "11") // Secondary Card Holder
                {
                    oDR["BarclayCardLimit"] = txtCreditLimit2.Text;

                    if (txtAPOAmount2.Text.Trim() != "")
                        oDR["APOAmount"] = txtAPOAmount2.Text;
                    else
                        oDR["APOAmount"] = 0;

                    if (txtAPODay2.Text.Trim() != "")
                        oDR["APODay"] = txtAPODay2.Text;
                    else
                        oDR["APODay"] = 0;

                    oDR["AccountNumber"] = txtAccountNum2.Text;

                    oDR["RequireGarageCard"] = ddGarageCard2.SelectedValue;
                    oDR["PaymentType"] = ddPayMethod2.SelectedValue;
                    oDR["APOType"] = ddAPOType2.SelectedValue;

                    oDR["ACBBank"] = ddBank2.SelectedValue;
                    oDR["ACBBranch"] = ddBranch2.SelectedValue;
                    oDR["ACBType"] = ddAccountType2.SelectedValue;

                    oDR["GroupProductsServices"] = ddGroupProductsServices2.SelectedValue;
                    oDR["ResearchPermissions"] = ddResearchPermission2.SelectedValue;
                    oDR["OtherCompanies"] = ddOtherProductsServices2.SelectedValue;
                }
                else
                {

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

                    oDR["GroupProductsServices"] = ddGroupProductsServices.SelectedValue;
                    oDR["ResearchPermissions"] = ddResearchPermission.SelectedValue;
                    oDR["OtherCompanies"] = ddOtherProductsServices.SelectedValue;
                }


                oDR["SignedApplication"] = ddSignedApp.SelectedValue;

                // UserID is used to hold the userid of the consultant who created the application and should
                // therefore not be overwritten when amendments are made
                if (oDR["UserID"] == System.DBNull.Value || oDR["UserID"].ToString() == "")
                    oDR["UserID"] = Context.User.Identity.Name.ToLower().Replace("sahl\\", "");

                oDR["AmendedBy"] = Context.User.Identity.Name.ToLower().Replace("sahl\\", "");
                oDR["AmendedDate"] = System.DateTime.Now;


                oDR["NotInterested"] = chkNotInterested.Checked;

                oDR["OverwriteReason"] = txtOverrideReason.Text;
                oDR["BarclayCardLimit"] = Convert.ToDouble(txtCreditLimit.Text.ToString());
                if (txtOverrideReason.Text != "")
                    oDR["Overwrite"] = true;
                else
                    oDR["Overwrite"] = false;

                #endregion
            }
        }

        SqlCommandBuilder oSCMD = new SqlCommandBuilder(oDA);
        oDA.Update(dsBarclayCard.Tables["BCTable"]);

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
        // 4. A Secondary Card holder - his credit limit will be the same as the primary card holders
        //if (txtOverrideReason.Text.Trim().Length == 0 && ddSignedApp.SelectedValue != "1" && !bLoanAlreadyRegistered
        //    && ddCardType2.SelectedValue != "11")
        //    RecalculateCreditLimit(enums.enBondHolderType.Principal);

        // Call the Stored Procedure to recalculate the credit limit for the Secondary Bond Holder if necessary
        // This must be not be called in the following instances.
        // 1. When the Application has been Overridden
        // 2. When a signed application has been received
        // 3. When the clients loan has been registered
        // 4. A Secondary Card holder - his credit limit will be the same as the primary card holders
        //if (txtOverrideReason2.Text.Trim().Length == 0 && ddSignedApp2.SelectedValue != "1" && !bLoanAlreadyRegistered
        //    && ddCardType2.SelectedValue != "11")
        //    RecalculateCreditLimit(enums.enBondHolderType.Secondary);

        oConn.Close();
    }

    private bool RecalculateCreditLimit(enums.enBondHolderType enBondHolder)
    {
        string sStoredProc = "";
        try
        {
            if (enBondHolder == enums.enBondHolderType.Principal)
                sStoredProc = "bc_AddUpdateBarclayCardRecord";
            else
                sStoredProc = "bc_AddUpdateBarclayCardRecordSecondary";


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
        ShowDetailPanels(true);
        pMessage.Visible = false;
        lblProspLoanNumVal.Visible = true;
        lblProspLoanNum.Visible = true;
    }

    private string pageSourceSQL()
    {
        string sLoanNum = sLoanNumber == "" ? "null" : sLoanNumber;
        string sProspNum = sProspectNumber == "" ? "null" : sProspectNumber;

        return string.Format("select top 2 barclaycardkey,isnull(RequireGarageCard, -1) as RequireGarageCard," +
                                    "BarclayCardLimit," +
                                    "Salutation," +
                                    "FirstNames," +
                                    "Surname," +
                                    "IDNumber," +
                                    "Salary," +
                                    "SecondaryBondHolder," +
                                    "isnull(CardType, -1) as CardType," +
                                    "isnull(PaymentType, -1) as PaymentType," +
                                    "isnull(APOType, -1) as APOType," +
                                    "isnull(APOAmount, 0) as APOAmount," +
                                    "isnull(APODay, 0) as APODay," +
                                    "isnull(ACBBank, -1) as ACBBank," +
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
                                    "or convert(varchar, bc.LoanNumber) = '{1}'", sProspNum, sLoanNum) +
                                    " order by BarclayCardKey asc";
    }

    private string pageUpdateSourceSQL()
    {
        string sLoanNum = Request["param1"] == "0" ? "" : Request["param1"].Trim();
        sLoanNum = sLoanNum == "" ? "null" : sLoanNum;

        string sProspNum = Request.QueryString["param0"] == "" ? "null" : Request.QueryString["param0"].Trim();

        return string.Format("select * from BarclayCard " +
                             "where (convert(varchar,ProspectNumber) = '{0}' " +
                             "or convert(varchar, LoanNumber) = '{1}') and BarclayCardKey in ({2},{3})", sProspNum, sLoanNum, Convert.ToInt32(txtBarclayCardKey1.Text), Convert.ToInt32(txtBarclayCardKey2.Text));
    }

    private void checkValidationRequirementsPrincipal()
    {
        if (ddPayMethod.SelectedValue == "5") // APO
            setValidationControlsPrincipal(true);
        else
            setValidationControlsPrincipal(false);

        if (ddAPOType.SelectedValue == "8")
            oRfvAPOAmount.Enabled = true;
        else
            oRfvAPOAmount.Enabled = false;

        if (ddAPOType.SelectedValue == "7" ||
            ddAPOType.SelectedValue == "8" ||
            ddAPOType.SelectedValue == "9")
            setValidationControlsPrincipal(true);
        else
            setValidationControlsPrincipal(false);
    }

    private void checkValidationRequirementsSecondary()
    {
        if (ddPayMethod2.SelectedValue == "5") // APO
            setValidationControlsSecondary(true);
        else
            setValidationControlsSecondary(false);

        if (ddAPOType2.SelectedValue == "8")
            oRfvAPOAmount2.Enabled = true;
        else
            oRfvAPOAmount2.Enabled = false;

        if (ddAPOType2.SelectedValue == "7" ||
            ddAPOType2.SelectedValue == "8" ||
            ddAPOType2.SelectedValue == "9")
            setValidationControlsSecondary(true);
        else
            setValidationControlsSecondary(false);
    }

    private void setValidationControlsPrincipal(bool a_Enabled)
    {
        oRVBank.Enabled = a_Enabled;
        oRVBranch.Enabled = a_Enabled;
        oRVAccountType.Enabled = a_Enabled;
        oRVAPOType.Enabled = a_Enabled;
        oRfvAPODay.Enabled = a_Enabled;
        oRfvAccountNumber.Enabled = a_Enabled;
    }
    private void setValidationControlsSecondary(bool a_Enabled)
    {
        oRVBank2.Enabled = a_Enabled;
        oRVBranch2.Enabled = a_Enabled;
        oRVAccountType2.Enabled = a_Enabled;
        oRVAPOType2.Enabled = a_Enabled;
        oRfvAPODay2.Enabled = a_Enabled;
        oRfvAccountNumber2.Enabled = a_Enabled;
    }

    private void SetControlsEditable(bool bPrimaryEditable,bool bSecondaryEditable)
    {
        // *CF* 18/05/06
        if (bPrimaryEditable || bSecondaryEditable)
            btnSave.Enabled = true;
        else
            btnSave.Enabled = false;

        // *CF* 12/04/06
        if (bPrimaryEditable && txtSubmittedDate.Text != "")
            bPrimaryEditable=false;
        if (bSecondaryEditable && txtSubmittedDate2.Text != "")
            bSecondaryEditable = false;

        // *CF* 02/05/06
        if (bPrimaryEditable && chkNotInterested.Checked == true)
            bPrimaryEditable = false;
        if (bSecondaryEditable && chkNotInterested2.Checked == true)
            bSecondaryEditable = false;

        EnableControls(pnlDetails1, bPrimaryEditable);
        EnableControls(pnlDetails1b, bPrimaryEditable);
        EnableControls(pnlDetails1c, bPrimaryEditable);

        EnableControls(pnlDetails2, bSecondaryEditable);
        EnableControls(pnlDetails2b, bSecondaryEditable);
        EnableControls(pnlDetails2c, bSecondaryEditable);

        txtSalary.Enabled = false;
        txtSalary2.Enabled = false;

        if (ddCardType.SelectedValue == "11")
            DisplaySecondaryCardHolderFields(enums.enBondHolderType.Principal, false);
        if (ddCardType2.SelectedValue == "11")
            DisplaySecondaryCardHolderFields(enums.enBondHolderType.Secondary, false);

        // Re-Submit button shoud only be displayed if:
        // 1) User is Authorised to Re-Submit (They belong to GoldCardManager AD Group)
        // 2) The application has already been submitted to the bank
        if (chkGoldCardManager.Checked && (txtSubmittedDate.Text != "" || txtSubmittedDate2.Text != ""))
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

                    case "RegularExpressionValidator":
                        RegularExpressionValidator rev = (RegularExpressionValidator)c;
                        rev.Enabled = bEnabled;
                        break;

                    default:
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    private void CreateAndRegisterJavaScripts()
    {
        string sFormName = "window." + this.Form.Name;

        #region Card Type 1 Validation JavaScript
        string sJavaScript =
        "function PrincipalBondHolderCardTypeValidate (source, arguments)"
        + "{"
            + "var e1 = document.getElementById('" + this.ddCardType.ClientID + "');"
            + "var e2 = document.getElementById('" + this.ddCardType2.ClientID + "');"
            + "var cardtype1 = e1.options[e1.selectedIndex].value;"
            + "var cardtype2 = e2.options[e2.selectedIndex].value;"

            + "if (cardtype1==10)"
            + "{"
                #region Set secondary fields visible
                + sFormName + ".document.all('" + this.lblSecondaryMsg.ClientID + "').style. display = 'none';"
                + sFormName + ".document.all('" + this.ddGarageCard.ClientID + "').disabled = false;"
                + sFormName + ".document.all('" + this.ddPayMethod.ClientID + "').disabled = false;"
                + sFormName + ".document.all('" + this.ddAPOType.ClientID + "').disabled = false;"
                + sFormName + ".document.all('" + this.txtAPOAmount.ClientID + "').disabled = false;"
                + sFormName + ".document.all('" + this.txtAPODay.ClientID + "').disabled = false;"
                + sFormName + ".document.all('" + this.ddBank.ClientID + "').disabled = false;"
                + sFormName + ".document.all('" + this.ddBranch.ClientID + "').disabled = false;"
                + sFormName + ".document.all('" + this.ddAccountType.ClientID + "').disabled = false;"
                + sFormName + ".document.all('" + this.txtAccountNum.ClientID + "').disabled = false;"
                + sFormName + ".document.all('" + this.ddGroupProductsServices.ClientID + "').disabled = false;"
                + sFormName + ".document.all('" + this.ddResearchPermission.ClientID + "').disabled = false;"
                + sFormName + ".document.all('" + this.ddOtherProductsServices.ClientID + "').disabled = false;"

                // Set Validators
                + "var a1 = document.getElementById('" + this.ddAPOType.ClientID + "');"
                + "var apotype1 = a1.options[a1.selectedIndex].value;"
                + "if (apotype1=='8')" // Fixed
                + "{"
                + "ValidatorEnable(" + this.oRVAPOAmount.ClientID + ",1);"
                + "ValidatorEnable(" + this.oRfvAPOAmount.ClientID + ",1);"
                + "}"
                + "else"
                + "{"
                + "ValidatorEnable(" + this.oRVAPOAmount.ClientID + ",0);"
                + "ValidatorEnable(" + this.oRfvAPOAmount.ClientID + ",0);"
                + "}"

                + "var p1 = document.getElementById('" + this.ddPayMethod.ClientID + "');"
                + "var paymethod1 = p1.options[p1.selectedIndex].value;"
                + "if (paymethod1=='5' || apotype1=='7' || apotype1=='8' || apotype1=='9')"
                + "{"
                + "ValidatorEnable(" + this.oRVAPOAmount.ClientID + ",1);"
                + "ValidatorEnable(" + this.oRfvAPOAmount.ClientID + ",1);"
                + "ValidatorEnable(" + this.oRVAPODay.ClientID + ",1);"
                + "ValidatorEnable(" + this.oRfvAPODay.ClientID + ",1);"
                + "ValidatorEnable(" + this.oRVBank.ClientID + ",1);"
                + "ValidatorEnable(" + this.oRVBranch.ClientID + ",1);"
                + "ValidatorEnable(" + this.oRVAccountType.ClientID + ",1);"
                + "ValidatorEnable(" + this.oRfvAccountNumber.ClientID + ",1);"
                + "}"
                + "else"
                + "{"
                + "ValidatorEnable(" + this.oRVAPOAmount.ClientID + ",0);"
                + "ValidatorEnable(" + this.oRfvAPOAmount.ClientID + ",0);"
                + "ValidatorEnable(" + this.oRVAPODay.ClientID + ",0);"
                + "ValidatorEnable(" + this.oRfvAPODay.ClientID + ",0);"
                + "ValidatorEnable(" + this.oRVBank.ClientID + ",0);"
                + "ValidatorEnable(" + this.oRVBranch.ClientID + ",0);"
                + "ValidatorEnable(" + this.oRVAccountType.ClientID + ",0);"
                + "ValidatorEnable(" + this.oRfvAccountNumber.ClientID + ",0);"
                + "}"        

                #endregion
            + "}"
            + "else if (cardtype1==11)"
            + "{"
                + "if (cardtype1==cardtype2)"
                + "{"
                    + "arguments.IsValid = false;"
                    + "return false;"
                + "}"
                + "else"
                + "{"
                    #region reset secondary fields
                    + sFormName + ".document.all('" + this.lblSecondaryMsg.ClientID + "').style.display = 'block';"
                    + sFormName + ".document.all('" + this.lblSecondaryMsg.ClientID + "').innerText = 'All Details below as per Primary Card Holder';"
                    + "var c=document.getElementById('" + this.ddGarageCard.ClientID + "');"
                    + "c.value = '';"
                    + "c.disabled = 'true';"
                    + "var c=document.getElementById('" + this.ddPayMethod.ClientID + "');"
                    + "c.value = '';"
                    + "c.disabled = 'true';"
                    + "var c=document.getElementById('" + this.ddAPOType.ClientID + "');"
                    + "c.value = '';"
                    + "c.disabled = 'true';"
                    + "var c=document.getElementById('" + this.txtAPOAmount.ClientID + "');"
                    + "c.value = '';"
                    + "c.disabled = 'true';"
                    + "var c=document.getElementById('" + this.txtAPODay.ClientID + "');"
                    + "c.value = '';"
                    + "c.disabled = 'true';"
                    + "var c=document.getElementById('" + this.ddBank.ClientID + "');"
                    + "c.value = '';"
                    + "c.disabled = 'true';"
                    + "var c=document.getElementById('" + this.ddBranch.ClientID + "');"
                    + "c.value = '';"
                    + "c.disabled = 'true';"
                    + "var c=document.getElementById('" + this.ddAccountType.ClientID + "');"
                    + "c.value = '';"
                    + "c.disabled = 'true';"
                    + "var c=document.getElementById('" + this.txtAccountNum.ClientID + "');"
                    + "c.value = '';"
                    + "c.disabled = 'true';"
                    + "var c=document.getElementById('" + this.ddGroupProductsServices.ClientID + "');"
                    + "c.value = '';"
                    + "c.disabled = 'true';"
                    + "var c=document.getElementById('" + this.ddResearchPermission.ClientID + "');"
                    + "c.value = '';"
                    + "c.disabled = 'true';"
                    + "var c=document.getElementById('" + this.ddOtherProductsServices.ClientID + "');"
                    + "c.value = '';"
                    + "c.disabled = 'true';"

                    + "ValidatorEnable(" + this.oRVAPOAmount.ClientID + ",0);"
                    + "ValidatorEnable(" + this.oRfvAPOAmount.ClientID + ",0);"
                    + "ValidatorEnable(" + this.oRVAPODay.ClientID + ",0);"
                    + "ValidatorEnable(" + this.oRfvAPODay.ClientID + ",0);"
                    + "ValidatorEnable(" + this.oRVBank.ClientID + ",0);"
                    + "ValidatorEnable(" + this.oRVBranch.ClientID + ",0);"
                    + "ValidatorEnable(" + this.oRVAccountType.ClientID + ",0);"
                    + "ValidatorEnable(" + this.oRfvAccountNumber.ClientID + ",0);"

                    #endregion
                    + "arguments.IsValid = true;"
                    + "return true;"
                + "}"
            + "}"
        + "}";

        #endregion

        RegisterClientSideJavaScript("PrincipalBondHolderCardTypeValidate", sJavaScript);

        #region Card Type 2 Validation JavaScript
        sJavaScript =
        "function SecondaryBondHolderCardTypeValidate (source, arguments)"
        + "{"
            + "var e1 = document.getElementById('" + this.ddCardType.ClientID + "');"
            + "var e2 = document.getElementById('" + this.ddCardType2.ClientID + "');"
            + "var cardtype1 = e1.options[e1.selectedIndex].value;"
            + "var cardtype2 = e2.options[e2.selectedIndex].value;"

            + "if (cardtype2==10)"
            + "{"
                #region Set secondary fields visible
                + sFormName + ".document.all('" + this.lblSecondaryMsg2.ClientID + "').style.display = 'none';"
                + sFormName + ".document.all('" + this.ddGarageCard2.ClientID + "').disabled = false;"
                + sFormName + ".document.all('" + this.ddPayMethod2.ClientID + "').disabled = false;"
                + sFormName + ".document.all('" + this.ddAPOType2.ClientID + "').disabled = false;"
                + sFormName + ".document.all('" + this.txtAPOAmount2.ClientID + "').disabled = false;"
                + sFormName + ".document.all('" + this.txtAPODay2.ClientID + "').disabled = false;"
                + sFormName + ".document.all('" + this.ddBank2.ClientID + "').disabled = false;"
                + sFormName + ".document.all('" + this.ddBranch2.ClientID + "').disabled = false;"
                + sFormName + ".document.all('" + this.ddAccountType2.ClientID + "').disabled = false;"
                + sFormName + ".document.all('" + this.txtAccountNum2.ClientID + "').disabled = false;"
                + sFormName + ".document.all('" + this.ddGroupProductsServices2.ClientID + "').disabled = false;"
                + sFormName + ".document.all('" + this.ddResearchPermission2.ClientID + "').disabled = false;"
                + sFormName + ".document.all('" + this.ddOtherProductsServices2.ClientID + "').disabled = false;"

                // Set Validators
                + "var a2 = document.getElementById('" + this.ddAPOType2.ClientID + "');"
                + "var apotype2 = a2.options[a2.selectedIndex].value;"
                + "if (apotype2=='8')" // Fixed
                + "{"
                + "ValidatorEnable(" + this.oRVAPOAmount2.ClientID + ",1);"
                + "ValidatorEnable(" + this.oRfvAPOAmount2.ClientID + ",1);"
                + "}"
                + "else"
                + "{"
                + "ValidatorEnable(" + this.oRVAPOAmount2.ClientID + ",0);"
                + "ValidatorEnable(" + this.oRfvAPOAmount2.ClientID + ",0);"
                + "}"

                + "var p2 = document.getElementById('" + this.ddPayMethod2.ClientID + "');"
                + "var paymethod2 = p2.options[p2.selectedIndex].value;"
                + "if (paymethod2=='5' || apotype2=='7' || apotype2=='8' || apotype2=='9')"
                + "{"
                + "ValidatorEnable(" + this.oRVAPOAmount2.ClientID + ",1);"
                + "ValidatorEnable(" + this.oRfvAPOAmount2.ClientID + ",1);"
                + "ValidatorEnable(" + this.oRVAPODay2.ClientID + ",1);"
                + "ValidatorEnable(" + this.oRfvAPODay2.ClientID + ",1);"
                + "ValidatorEnable(" + this.oRVBank2.ClientID + ",1);"
                + "ValidatorEnable(" + this.oRVBranch2.ClientID + ",1);"
                + "ValidatorEnable(" + this.oRVAccountType2.ClientID + ",1);"
                + "ValidatorEnable(" + this.oRfvAccountNumber2.ClientID + ",1);"
                + "}"
                + "else"
                + "{"
                + "ValidatorEnable(" + this.oRVAPOAmount.ClientID + ",0);"
                + "ValidatorEnable(" + this.oRfvAPOAmount.ClientID + ",0);"
                + "ValidatorEnable(" + this.oRVAPODay.ClientID + ",0);"
                + "ValidatorEnable(" + this.oRfvAPODay.ClientID + ",0);"
                + "ValidatorEnable(" + this.oRVBank.ClientID + ",0);"
                + "ValidatorEnable(" + this.oRVBranch.ClientID + ",0);"
                + "ValidatorEnable(" + this.oRVAccountType.ClientID + ",0);"
                + "ValidatorEnable(" + this.oRfvAccountNumber.ClientID + ",0);"
                + "}"        
                #endregion
            + "}"
            + "else if (cardtype2==11)"
            + "{"
                + "if (cardtype1==cardtype2)"
                + "{"
                    + "arguments.IsValid = false;"
                    + "return false;"
                + "}"
                + "else"
                + "{"
                    #region reset secondary fields
                    + sFormName + ".document.all('" + this.lblSecondaryMsg2.ClientID + "').style.display = 'block';"
                    + sFormName + ".document.all('" + this.lblSecondaryMsg2.ClientID + "').innerText = 'All Details below as per Primary Card Holder';"

                    + "var c=document.getElementById('" + this.ddGarageCard2.ClientID + "');"
                    + "c.value = '';"
                    + "c.disabled = 'true';"
                    + "var c=document.getElementById('" + this.ddPayMethod2.ClientID + "');"
                    + "c.value = '';"
                    + "c.disabled = 'true';"
                    + "var c=document.getElementById('" + this.ddAPOType2.ClientID + "');"
                    + "c.value = '';"
                    + "c.disabled = 'true';"
                    + "var c=document.getElementById('" + this.txtAPOAmount2.ClientID + "');"
                    + "c.value = '';"
                    + "c.disabled = 'true';"
                    + "var c=document.getElementById('" + this.txtAPODay2.ClientID + "');"
                    + "c.value = '';"
                    + "c.disabled = 'true';"
                    + "var c=document.getElementById('" + this.ddBank2.ClientID + "');"
                    + "c.value = '';"
                    + "c.disabled = 'true';"
                    + "var c=document.getElementById('" + this.ddBranch2.ClientID + "');"
                    + "c.value = '';"
                    + "c.disabled = 'true';"
                    + "var c=document.getElementById('" + this.ddAccountType2.ClientID + "');"
                    + "c.value = '';"
                    + "c.disabled = 'true';"
                    + "var c=document.getElementById('" + this.txtAccountNum2.ClientID + "');"
                    + "c.value = '';"
                    + "c.disabled = 'true';"
                    + "var c=document.getElementById('" + this.ddGroupProductsServices2.ClientID + "');"
                    + "c.value = '';"
                    + "c.disabled = 'true';"
                    + "var c=document.getElementById('" + this.ddResearchPermission2.ClientID + "');"
                    + "c.value = '';"
                    + "c.disabled = 'true';"
                    + "var c=document.getElementById('" + this.ddOtherProductsServices2.ClientID + "');"
                    + "c.value = '';"
                    + "c.disabled = 'true';"

                    + "ValidatorEnable(" + this.oRVAPOAmount2.ClientID + ",0);"
                    + "ValidatorEnable(" + this.oRfvAPOAmount2.ClientID + ",0);"
                    + "ValidatorEnable(" + this.oRVAPODay2.ClientID + ",0);"
                    + "ValidatorEnable(" + this.oRfvAPODay2.ClientID + ",0);"
                    + "ValidatorEnable(" + this.oRVBank2.ClientID + ",0);"
                    + "ValidatorEnable(" + this.oRVBranch2.ClientID + ",0);"
                    + "ValidatorEnable(" + this.oRVAccountType2.ClientID + ",0);"
                    + "ValidatorEnable(" + this.oRfvAccountNumber2.ClientID + ",0);"

                    #endregion
                    + "arguments.IsValid = true;"
                    + "return true;"
                + "}"
            + "}"
        + "}";

        #endregion

        RegisterClientSideJavaScript("SecondaryBondHolderCardTypeValidate", sJavaScript);

        #region Signed application 1 Javascript
        sJavaScript =
        "function SignedApp1 (source, arguments)"
        + "{"
            + "var c2 = document.getElementById('" + this.ddCardType2.ClientID + "');"
            + "var cardtype2 = c2.options[c2.selectedIndex].value;"
            + "var e1 = document.getElementById('" + this.ddSignedApp.ClientID + "');"
            + "var signed1 = e1.options[e1.selectedIndex].value;"
            + "if (cardtype2==11)" // Secondary
            + "{"
            + sFormName + ".document.all('" + this.ddSignedApp2.ClientID + "').value = signed1;"
            + "}"
        + "}";
        #endregion

        RegisterClientSideJavaScript("SignedApp1", sJavaScript);

        #region Signed application 2 Javascript
        sJavaScript =
        "function SignedApp2 (source, arguments)"
        + "{"
            + "var c1 = document.getElementById('" + this.ddCardType.ClientID + "');"
            + "var cardtype1 = c1.options[c1.selectedIndex].value;"
            + "var e2 = document.getElementById('" + this.ddSignedApp2.ClientID + "');"
            + "var signed2 = e2.options[e2.selectedIndex].value;"
            + "if (cardtype1==11)" // Secondary
            + "{"
            + sFormName + ".document.all('" + this.ddSignedApp.ClientID + "').value = signed2;"
            + "}"
        + "}";
        #endregion

        RegisterClientSideJavaScript("SignedApp2", sJavaScript);

    }
        
    private void RegisterClientSideJavaScript(string sScriptName, string sScriptText)
    {
        try
        {
            Type cstype = this.GetType();

            // Get a ClientScriptManager reference from the Page class.
            ClientScriptManager cs = Page.ClientScript;

            // Check to see if the startup script is already registered.
            if (!cs.IsStartupScriptRegistered(cstype, sScriptName))
            {
                cs.RegisterStartupScript(cstype, sScriptName, sScriptText, true);
            }
        }
        catch { }
    }

    protected void chkNotInterested2_CheckedChanged(object sender, EventArgs e)
    {
        bool bEnable = !chkNotInterested2.Checked;

        // Disable all fields except the chkNotInterested
        EnableControls(pnlDetails2, bEnable);
        EnableControls(pnlDetails2b, bEnable);
        EnableControls(pnlDetails2c, bEnable);
        //btnOverrideApplication2.Enabled = bEnable;

        chkNotInterested2.Enabled = true;

        checkValidationRequirementsSecondary(); // 15-06-2006 CF


    } 

    protected void chkNotInterested_CheckedChanged(object sender, EventArgs e)
    {
        bool bEnable = !chkNotInterested.Checked;

        // Disable all fields except the chkNotInterested
        EnableControls(pnlDetails1, bEnable);
        EnableControls(pnlDetails1b, bEnable);
        EnableControls(pnlDetails1c, bEnable);
        //btnOverrideApplication.Enabled = bEnable;

        chkNotInterested.Enabled = true;

        checkValidationRequirementsPrincipal(); // 15-06-2006 CF
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

            SqlCommand oCmd = new SqlCommand(sSQL, oConn);
            int irows = oCmd.ExecuteNonQuery();
            oCmd.Transaction.Commit();
        }
        catch
        {
        }
        oConn.Close();
    }

}
