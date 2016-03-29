using System;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using System.Web.UI.WebControls;

namespace SAHL.Web.Views.Common
{
    public partial class SettlementBankingDetails : SAHLCommonBaseView, ISettlementBankingDetails
    {


        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!ShouldRunPage())
            //    return;

            //switch (m_ViewMode)
            //{
            //    case "display":
            //        {
            //            CancelButton.Visible = false;
            //            SubmitButton.Visible = false;
            //            Bank.Enabled = false;
            //            BranchCode.Enabled = false;
            //            BranchName.Enabled = false;
            //            AccountType.Enabled = false;
            //            AccountNumber.Enabled = false;
            //            AccountName.Enabled = false;
            //            break;
            //        }

            //    case "update":
            //        {
            //            CancelButton.Visible = true;
            //            SubmitButton.Visible = true;
            //            SubmitButton.Text = "Update";
            //            SubmitButton.AccessKey = "U";
            //            Bank.Enabled = true;
            //            BranchCode.Enabled = true;
            //            BranchName.Enabled = true;
            //            AccountType.Enabled = true;
            //            AccountNumber.Enabled = true;
            //            AccountName.Enabled = true;
            //            break;
            //        }

            //}

            //BranchCode.Attributes["onchange"] = "selectOtherEx(this,'" + BranchName.UniqueID + "');";
            //BranchName.Attributes["onchange"] = "selectOtherEx(this,'" + BranchCode.UniqueID + "');";

            //m_BankCode = "-select-";
            //if (Request.Form[Bank.UniqueID] != null)
            //    m_BankCode = Request.Form[Bank.UniqueID];
        }

        //private void bindControls(string BankCode)
        //{
        //    //m_BankCode = BankCode;

        //    //Bank.DataSource = m_Controller.Lookups.ACBBank;
        //    //Bank.DataTextField = m_Controller.Lookups.ACBBank.ACBBankDescriptionColumn.ToString();
        //    //Bank.DataValueField = m_Controller.Lookups.ACBBank.ACBBankCodeColumn.ToString();
        //    //Bank.DataBind();

        //    //// remove the Stop order option
        //    //for (int i = 0; i < m_Controller.Lookups.ACBBank.Rows.Count; i++)
        //    //{
        //    //    Lookup.ACBBankRow r = m_Controller.Lookups.ACBBank[i];
        //    //    if (r.ACBBankDescription == "Stop Order")
        //    //        Bank.Items[i].Enabled = false;
        //    //}


        //    //if (Request.Form[Bank.UniqueID] != null)
        //    //    m_BankCode = Request.Form[Bank.UniqueID];


        //    //bindBranch(m_BankCode);

        //    //AccountType.DataSource = m_Controller.Lookups.ACBType;
        //    //AccountType.DataTextField = m_Controller.Lookups.ACBType.ACBTypeDescriptionColumn.ToString();
        //    //AccountType.DataValueField = m_Controller.Lookups.ACBType.ACBTypeNumberColumn.ToString();
        //    //AccountType.DataBind();

        //    //for (int i = 0; i < m_Controller.Lookups.ACBType.Rows.Count; i++)
        //    //{
        //    //    Lookup.ACBTypeRow r = m_Controller.Lookups.ACBType[i];
        //    //    if (r.ACBTypeNumber == (int)SAHL.Datasets.ACBType.Unknown)
        //    //        AccountType.Items[i].Enabled = false;
        //    //    if (r.ACBTypeNumber == (int)SAHL.Datasets.ACBType.CreditCard)
        //    //        AccountType.Items[i].Enabled = false;
        //    //}

        //    //BankAccountStatus.DataSource = m_Controller.Lookups.GeneralStatus;
        //    //BankAccountStatus.DataTextField = m_Controller.Lookups.GeneralStatus.DescriptionColumn.ToString();
        //    //BankAccountStatus.DataValueField = m_Controller.Lookups.GeneralStatus.GeneralStatusKeyColumn.ToString();
        //    //BankAccountStatus.DataBind();
        //}

        //private void bindBranch(string bankCode)
        //{
        //    //if (!bankCode.Equals("-select-"))
        //    //{

        //    //    DataView dva = new DataView(m_Controller.Lookups.ACBBranch, "ActiveIndicator=0 AND ACBBankCode = " + bankCode, "ACBBranchDescription ASC", DataViewRowState.CurrentRows);
        //    //    BranchName.Items.Clear();
        //    //    for (int i = 0; i < dva.Count; i++)
        //    //    {
        //    //        BranchName.Items.Add(new ListItem(dva[i]["ACBBranchDescription"].ToString(), dva[i]["ACBBranchCode"].ToString()));
        //    //    }
        //    //    BranchName.Items.Insert(0, new ListItem("- Please Select -", "-select-"));


        //    //    DataView dvb = new DataView(m_Controller.Lookups.ACBBranch, "ActiveIndicator=0 AND ACBBankCode = " + bankCode, "ACBBranchCode ASC", DataViewRowState.CurrentRows);
        //    //    BranchCode.Items.Clear();
        //    //    for (int i = 0; i < dvb.Count; i++)
        //    //    {
        //    //        BranchCode.Items.Add(new ListItem(dvb[i]["ACBBranchCode"].ToString(), dvb[i]["ACBBranchCode"].ToString()));
        //    //    }
        //    //    BranchCode.Items.Insert(0, new ListItem("- Please Select -", "-select-"));


        //    //    if (m_Controller.SaveBankAccountStateCache != null && m_Controller.SaveBankAccountStateCache.m_SaveBranch != "")
        //    //    {
        //    //        BranchName.SelectedValue = m_Controller.SaveBankAccountStateCache.m_SaveBranch;
        //    //        BranchCode.SelectedValue = m_Controller.SaveBankAccountStateCache.m_SaveBranch;
        //    //    }

        //    //}
        //}

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            //m_Controller.Navigator.Navigate("BankingDetails");
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            //Metrics MI = base.GetClientMetrics();
            ////lblError.Text = "";

            //if (!Page.IsValid)
            //    return;

            //if (IsValid == true)
            //{
            //    m_Controller.SettlementBankDetailsGet(m_AccountKey, base.GetClientMetrics());        

            //    if (m_Controller.BankAccountDS._BankAccount.Rows.Count == 0)
            //    {
            //        //ADD the account
            //        // The validator has been call so there uniqueness of the account is determined
            //        BankAccount.BankAccountDataTable dt = m_Controller.BankAccountDS._BankAccount;
            //        BankAccount.BankAccountRow r = dt.NewBankAccountRow();
            //        r.ACBBranchCode = BranchCode.SelectedValue;
            //        r.ACBTypeNumber = int.Parse(AccountType.SelectedValue);
            //        //r.BankAccountKey;
            //        r.AccountName = AccountName.Text;
            //        r.AccountNumber = AccountNumber.Text;
            //        r.UserID = Authenticator.GetFullWindowsUserName();
            //        dt.AddBankAccountRow(r);

            //        try
            //        {                  
            //            m_Controller.AddBankAccount(m_Controller.BankAccountDS, MI);

            //            // update the AccountDebtSettlementTable with the BankAccountKey
            //            m_Controller.AccountDebtSettlementInsertBankAccountKey(int.Parse(DetailsGrid.Rows[0].Cells[0].Text), r.BankAccountKey, MI);

            //            m_Controller.Navigator.Navigate("SettlementBankingDetails");
            //        }
            //        catch (Exception ex)
            //        {
            //            lblError.Text = ex.Message;
            //        }
            //    }
            //    else
            //    {
            //        //Update the Account

            //       BankAccount.BankAccountDataTable dt = m_Controller.BankAccountDS._BankAccount;
            //       BankAccount.BankAccountRow r = dt.Rows[0] as BankAccount.BankAccountRow;
            //        r.ACBBranchCode = BranchCode.SelectedValue;
            //        r.ACBTypeNumber = int.Parse(AccountType.SelectedValue);
            //        //r.BankAccountKey;
            //        r.AccountName = AccountName.Text;
            //        r.AccountNumber = AccountNumber.Text;
            //        r.UserID = Authenticator.GetFullWindowsUserName();

            //        try
            //        {
            //            SAHL.Datasets.SettlementBankDetails.AccountDebtSettlementDataTable adsTable = new SettlementBankDetails.AccountDebtSettlementDataTable();
            //            m_Controller.AccountDebtSettlementGetByPropertyKey(adsTable,int.Parse(DetailsGrid.Rows[0].Cells[0].Text), base.GetClientMetrics());
            //            if (adsTable.Rows.Count > 0)
            //            {
            //                m_Controller.UpdateBankAccount(m_Controller.BankAccountDS._BankAccount,adsTable, MI);
            //            }
            //            m_Controller.Navigator.Navigate("SettlementBankingDetails");
            //        }
            //        catch (Exception ex)
            //        {
            //            lblError.Text = ex.Message;
            //        }
            //    }
            //}        
        }




        protected void ServerValidate(object source, ServerValidateEventArgs args)
        {
        //// Validate that the account number does not exist in the database.
        //m_Controller.SearchAccountNumber = AccountNumber.Text;
        //m_Controller.SearchAccountBranch = BranchCode.SelectedValue;



        //Metrics MI = base.GetClientMetrics();

        //m_Controller.BankAccountDS.Clear();

        //if (m_Controller.BankAccountDS._BankAccount.Rows.Count > 0)
        //{
        //    args.IsValid = false; // failed because it exists
        //    ValUniqueBankAccount.ErrorMessage = "A bank account record already exists for the details specified";
        //}

        //// now validate if the account number is valid
        //string ErrorMessage = m_Controller.ValidateAccountNumber(AccountNumber.Text, BranchCode.SelectedValue, AccountType.SelectedValue, MI);
        //if (ErrorMessage != "")
        //{
        //    args.IsValid = false; // failed because it exists
        //    ValUniqueBankAccount.ErrorMessage = ErrorMessage;
        //}
        }

        protected static void SearchButton_Click(object sender, EventArgs e)
        {
            //SaveBankDetailsState();

            //m_Controller.Navigator.Navigate("SettlementBankingDetailsUse");
        }

        //Peet van der Walt - Add functionality for ticket 3068
        //private void SaveBankDetailsState()
        //{
        //    //m_Controller.SaveBankAccountStateCache = new SaveBankAccountState();
        //    //m_Controller.SaveBankAccountStateCache.m_SaveBank = Bank.SelectedValue;
        //    //m_Controller.SaveBankAccountStateCache.m_SaveBranch = BranchCode.SelectedValue;
        //    //m_Controller.SaveBankAccountStateCache.m_SaveAccountType = AccountType.SelectedValue;
        //    //m_Controller.SaveBankAccountStateCache.m_SaveAccountNumber = AccountNumber.Text;
        //    //m_Controller.SaveBankAccountStateCache.m_SaveAccountName = AccountName.Text;
        //    //m_Controller.SaveBankAccountStateCache.m_SaveStatus = BankAccountStatus.SelectedValue;

        //    //m_Controller.SearchAccountNumber = AccountNumber.Text;
        //    //m_Controller.SearchAccountBranch = BranchCode.SelectedValue;      
        //}


        //protected void BankDetailsGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    string findValue;

        //    // lookup the bank
        //    findValue = e.Row.Cells[1].Text;
        //    if (findValue.Equals("&nbsp;"))
        //        findValue = "";
        //    Lookup.ACBBranchRow brRow = m_Controller.Lookups.ACBBranch.FindByACBBranchCode(findValue);
        //    if (brRow != null)
        //    {
        //        Lookup.ACBBankRow baRow = m_Controller.Lookups.ACBBank.FindByACBBankCode(brRow.ACBBankCode);
        //        if (baRow != null)
        //            e.Row.Cells[1].Text = baRow.ACBBankDescription;
        //    }

        //    // lookup the branch
        //    findValue = e.Row.Cells[2].Text;
        //    if (findValue.Equals("&nbsp;"))
        //        findValue = "";
        //    brRow = m_Controller.Lookups.ACBBranch.FindByACBBranchCode(findValue);
        //    if (brRow != null)
        //        e.Row.Cells[2].Text = brRow.ACBBranchCode + " - " + brRow.ACBBranchDescription;


        //    // lookup the Account type
        //    findValue = e.Row.Cells[3].Text;
        //    if (findValue.Equals("&nbsp;"))
        //        findValue = "0";
        //    Lookup.ACBTypeRow tRow = m_Controller.Lookups.ACBType.FindByACBTypeNumber(int.Parse(findValue));
        //    if (tRow != null)
        //        e.Row.Cells[3].Text = tRow.ACBTypeDescription;


        //    int bankAccountKey = int.Parse(e.Row.Cells[0].Text);
        //    for (int i = 0; i < m_Controller.BankAccountDS.LegalEntityBankAccount.Count; i++)
        //    {
        //        if (m_Controller.BankAccountDS.LegalEntityBankAccount[i].BankAccountKey == bankAccountKey)
        //        {
        //            e.Row.Cells[6].Text = m_Controller.Lookups.GeneralStatus.FindByGeneralStatusKey(m_Controller.BankAccountDS.LegalEntityBankAccount[i].GeneralStatusKey).Description;
        //            break;
        //        }
        //    }

        //}
        //}


        //private void BindGrid()
        //{
        //    //DetailsGrid.AddGridBoundColumn("PropertyKey", "Property Key", Unit.Percentage(0), HorizontalAlign.Left, false);
        //    //DetailsGrid.AddGridBoundColumn("UnitNumber", "Unit", Unit.Percentage(6), HorizontalAlign.Left, true);
        //    //DetailsGrid.AddGridBoundColumn("BuildingNumber", "Building", Unit.Percentage(20), HorizontalAlign.Left, true);
        //    //DetailsGrid.AddGridBoundColumn("BuildingName", "", Unit.Percentage(0), HorizontalAlign.Left, false);
        //    //DetailsGrid.AddGridBoundColumn("StreetNumber", "Street", Unit.Percentage(20), HorizontalAlign.Left, true);
        //    //DetailsGrid.AddGridBoundColumn("StreetName", "", Unit.Percentage(0), HorizontalAlign.Left, false);
        //    //DetailsGrid.AddGridBoundColumn("RRR_SuburbDescription", "Suburb", Unit.Percentage(18), HorizontalAlign.Left, true);
        //    //DetailsGrid.AddGridBoundColumn("RRR_CityDescription", "City", Unit.Percentage(18), HorizontalAlign.Left, true);
        //    //DetailsGrid.AddGridBoundColumn("RRR_ProvinceDescription", "Province", Unit.Percentage(18), HorizontalAlign.Left, true);

        //    //DetailsGrid.DataSource = m_Controller.SettlementBankDetailsDS.Address;
        //    //DetailsGrid.DataBind();
        //}

        //protected void DetailsGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    e.Row.Cells[1].Text = e.Row.Cells[1].Text + " " + e.Row.Cells[2].Text;
        //    e.Row.Cells[3].Text = e.Row.Cells[3].Text + " " + e.Row.Cells[4].Text;
        //}
        //}


        protected void DetailsGrid_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        //private void PopulateBankSettlementDetailsControls(int propKey)
        //{
        //    //if (propKey == -1)
        //    //    return;
        //    //SAHL.Datasets.BankAccount.ACBBranchRow branchRow = null;
        //    //BankAccount.BankAccountRow bankRow = null;
        //    //if (m_Controller.BankAccountDS.ACBBranch.Rows.Count > 0
        //    //    && m_Controller.BankAccountDS._BankAccount.Rows.Count > 0)
        //    //{
        //    //    branchRow = m_Controller.BankAccountDS.ACBBranch.Rows[0] as SAHL.Datasets.BankAccount.ACBBranchRow;
        //    //    bankRow = m_Controller.BankAccountDS._BankAccount.Rows[0] as BankAccount.BankAccountRow;
        //    //}

        //    //if (branchRow == null || bankRow == null)
        //    //{
        //    //    if (m_ViewMode == "display")
        //    //    {
        //    //        ListControls.Visible = false;
        //    //        lblError.Text = "A banking record does not exist for this account!";
        //    //        lblError.Visible = true;
        //    //    }
        //    //    else
        //    //    {
        //    //        ListControls.Visible = true;
        //    //    }
        //    //}
        //    //else
        //    //{
        //    //    ListControls.Visible = true;



        //    //    string bankName = m_Controller.Lookups.ACBBank.FindByACBBankCode(branchRow.ACBBankCode).ACBBankDescription;
        //    //    for (int x = 0; x < Bank.Items.Count; x++)
        //    //    {
        //    //        if (Bank.Items[x].Text == bankName)
        //    //        {
        //    //            bindControls(branchRow.ACBBankCode.ToString());
        //    //            Bank.SelectedIndex = x;
        //    //            break;
        //    //        }
        //    //    }

        //    //    for (int x = 0; x < BranchName.Items.Count; x++)
        //    //    {
        //    //        if (BranchName.Items[x].Text == branchRow.ACBBranchDescription)
        //    //        {
        //    //            BranchName.SelectedIndex = x;
        //    //            break;
        //    //        }
        //    //    }

        //    //    for (int x = 0; x < BranchCode.Items.Count; x++)
        //    //    {
        //    //        if (BranchCode.Items[x].Text == branchRow.ACBBranchCode.ToString())
        //    //        {
        //    //            BranchCode.SelectedIndex = x;
        //    //            break;
        //    //        }
        //    //    }

        //    //    string accType = m_Controller.Lookups.ACBType.FindByACBTypeNumber(bankRow.ACBTypeNumber).ACBTypeDescription;

        //    //    for (int x = 0; x < AccountType.Items.Count; x++)
        //    //    {
        //    //        if (AccountType.Items[x].Text == accType)
        //    //        {
        //    //            AccountType.SelectedIndex = x;
        //    //            break;
        //    //        }
        //    //    }

        //    //    AccountName.Text = bankRow.AccountName.ToString();
        //    //    AccountNumber.Text = bankRow.AccountNumber.ToString();
        //    //}  
        //}



        #region ISettlementBankingDetails Members

        public event EventHandler OnCancelButtonClicked;

        public event EventHandler OnSubmitButtonClicked;

        #endregion
    }
}