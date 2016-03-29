using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Text;

public partial class BarclayCard_GoldCardSearch : System.Web.UI.Page
{
    private SqlConnection oConn = new SqlConnection();
    string sConnectionString = "";
    private bool bJointApplication = false;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        sConnectionString = DBConnection.ConnectionString();
        //sConnectionString = "Server=SAHLS303;Trusted_Connection=False;Database=SAHLDB;User=CraigF;Password=";

        osqlds_GetLoanFindDataByNumber.ConnectionString = sConnectionString;
        osqlds_GetLoanFindDataByNames.ConnectionString = sConnectionString;
        osqlds_GetLoanFindDataBySurname.ConnectionString = sConnectionString;
        osqlds_GetLoanFindDataByFirstName.ConnectionString = sConnectionString; 
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            btnGoldCardApp.Visible = false;
            gvSearch.Visible = false;
        }

        if (Context.User.Identity.Name == @"SAHL\CraigF")
        {
            Label1.Text = "Version 3";
            Label1.Visible = true;
        }

        RegisterClientJavascript();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        LoadData();
    }

    protected void LoadData()
    {
        try
        {
            btnGoldCardApp.Visible = false;

            if (txtLoanNumber.Text.Trim().Length > 0)
                this.gvSearch.DataSourceID = osqlds_GetLoanFindDataByNumber.ID;
            else if (txtSurname.Text.Trim().Length > 0 && txtFirstNames.Text.Trim().Length > 0)
                this.gvSearch.DataSourceID = osqlds_GetLoanFindDataByNames.ID;
            else if (txtSurname.Text.Trim().Length > 0)
                this.gvSearch.DataSourceID = osqlds_GetLoanFindDataBySurname.ID;
            else if (txtFirstNames.Text.Trim().Length > 0)
                this.gvSearch.DataSourceID = osqlds_GetLoanFindDataByFirstName.ID;

            this.gvSearch.DataBind();

            if (gvSearch.Rows.Count > 0)
            {
                btnGoldCardApp.Visible = true;
                gvSearch.Visible = true;
            }
        }
        catch (Exception ex)
        {
            ValidateSearchCtrl.IsValid  = false;
            ValidateSearchCtrl.ErrorMessage = ex.Message;
        }
    }

    protected void btnGoldCardApp_Click(object sender, EventArgs e)
    {
        ValidateApplication();

        if (!Page.IsValid)
            return;

        string sAccountName = "";
        int iPurpose = 10, iClientNumber = 0;
        int iSelectedLoanNumber = -1;
        string sASPX_Page = "";
        bool bGoldCardRecordAdded = false;

        iSelectedLoanNumber = Convert.ToInt32(gvSearch.SelectedRow.Cells[1].Text);
        sAccountName = gvSearch.SelectedRow.Cells[2].Text;
        iClientNumber = Convert.ToInt32(gvSearch.SelectedRow.Cells[8].Text);

        // Call Stored Procedure to create/update Barclay Card Records
        switch (bJointApplication) // bJointApplication - this gets set in the ValidateApplication method - more than 1 Role = Joint
        {
            case false:
                sASPX_Page = "BarclayCardApp.aspx";
                //bGoldCardRecordAdded = AddUpdateBarclayCardRecord(iClientNumber, "Client", iPurpose, enums.enBondHolderType.Principal);
                break;
            case true:
                 sASPX_Page = "BarclayCardJointApp.aspx";
                 //bGoldCardRecordAdded = AddUpdateBarclayCardRecord(iClientNumber, "Client", iPurpose, enums.enBondHolderType.Principal);
                 //bGoldCardRecordAdded = AddUpdateBarclayCardRecord(iClientNumber, "Client", iPurpose, enums.enBondHolderType.Secondary);
                break;
        }

        bGoldCardRecordAdded = AddUpdateBarclayCardRecord(iClientNumber, "Client" , bJointApplication);

        if (bGoldCardRecordAdded == false)
        {
            ValidateApplicationCtrl.IsValid = false;
            ValidateApplicationCtrl.ErrorMessage = "Application aborted : Error creating Gold Card record in the database";
        }
        else
        {
            string sUrl = sASPX_Page + "?Mid=101";
            sUrl += "&param0=0&param1=" + iSelectedLoanNumber.ToString().Trim() + "&param2=" + sAccountName;
            Response.Redirect(sUrl);
        }
    }

    protected void ValidateSearch(object source, ServerValidateEventArgs args)
    {
        if ( txtLoanNumber.Text.Trim().Length > 0 ||
             txtSurname.Text.Trim().Length > 0 ||
             txtFirstNames.Text.Trim().Length > 0)
        {
            return;
        }
        args.IsValid = false;
    }

    protected void ValidateApplication()
    {
        oConn.ConnectionString = sConnectionString;

        try
        {
            if (gvSearch.SelectedIndex < 0)
            {
                ValidateApplicationCtrl.ErrorMessage = "Gold Card cannot be processed : Must select a loan record";
                ValidateApplicationCtrl.IsValid = false;
                return;
            }

            DataTable dtLegalEntity = new DataTable();
            oConn.Open();

            // Get the Legal Entity / Role Data
            string sSQL = "select le.LegalEntityTypeKey,isnull(let.Description,'') as LegalEntityType,isnull(le.CitizenTypeKey,0) as CitizenTypeKey,isnull(ct.Description,'Unknown') as CitizenShipType,isnull(le.FirstNames,'') + ' ' + isnull(le.Surname,'') as LegalEntityName "
                        + "from [2am]..Role r (nolock) "
                        + "join [2am]..LegalEntity le (nolock) on le.LegalEntityKey = r.LegalEntityKey "
                        + "join [2am]..LegalEntityType let (nolock) on let.LegalEntityTypeKey = le.LegalEntityTypeKey "
                        + "left join [2am]..CitizenType ct (nolock) on ct.CitizenTypeKey = le.CitizenTypeKey "
                        + "where AccountKey = " + Convert.ToInt32(gvSearch.SelectedRow.Cells[1].Text)
                        + " and r.RoleTypeKey = 2"; // Main Applicants only

            SqlDataAdapter da = new SqlDataAdapter(sSQL, oConn);
            da.Fill(dtLegalEntity);

            bJointApplication = false;

            if (dtLegalEntity.Rows.Count > 0)
            {
                if (dtLegalEntity.Rows.Count > 1)
                    bJointApplication = true;

                foreach (DataRow dr in dtLegalEntity.Rows)
                {
                    switch (Convert.ToInt32(dr["LegalEntityTypeKey"]))
	                {
                        case 1: // Unknown
                        case 3: // Company
                        case 4: // Close Corporation
                        case 5: // Trust
                            ValidateApplicationCtrl.ErrorMessage = "Gold Card cannot be processed : Loan has an applicant type of [" + dr["LegalEntityType"].ToString() + "]";
                            ValidateApplicationCtrl.IsValid = false;
                            break;
		                default:
                            break;
	                }

                    if (Convert.ToInt32(dr["CitizenTypeKey"])==3)
                    {
                        ValidateApplicationCtrl.ErrorMessage = "Gold Card cannot be processed : Loan has an applicant with a Citizenship of [" + dr["CitizenShipType"].ToString() + "]";
                        ValidateApplicationCtrl.IsValid = false;
                        break;

                    }
                }
            }
        }
        catch (Exception ex)
        {
            ValidateApplicationCtrl.ErrorMessage = ex.ToString();
            oConn.Close();
        }

        oConn.Close();
    }

    private bool AddUpdateBarclayCardRecord(int iClientNumber, string sStage, bool bJointApplication)
    {
        string sStoredProc = "[2am]..pGoldCardApplication";

        try
        {
            //if (enBondHolder == enums.enBondHolderType.Principal)
            //    sStoredProc = "bc_AddUpdateBarclayCardRecord";
            //else
            //    sStoredProc = "bc_AddUpdateBarclayCardRecordSecondary";

            oConn.ConnectionString = sConnectionString; 
            oConn.Open();


            string sSQL = "exec " + sStoredProc + " " + iClientNumber + ",'" + sStage + "'," + bJointApplication;
            SqlCommand com = new SqlCommand(sSQL,oConn);
            com.ExecuteNonQuery();

            oConn.Close();

            return true;

        }
        catch (Exception ex)
        {
            ValidateApplicationCtrl.IsValid = false;
            ValidateApplicationCtrl.ErrorMessage = ex.Message;

            oConn.Close();

            return false;
        }
    } 
    
    protected void gvSearch_DataBound(object sender, EventArgs e)
    {
        try
        {
            // Retrieve the pager row.
            GridViewRow pagerRow = gvSearch.BottomPagerRow;

            // Retrieve the DropDownList and Label controls from the row.
            DropDownList pageList = (DropDownList)pagerRow.Cells[0].FindControl("PageDropDownList");
            Label pageLabel = (Label)pagerRow.Cells[0].FindControl("CurrentPageLabel");

            if (pageList != null)
            {

                // Create the values for the DropDownList control based on 
                // the  total number of pages required to display the data
                // source.
                for (int i = 0; i < gvSearch.PageCount; i++)
                {

                    // Create a ListItem object to represent a page.
                    int pageNumber = i + 1;
                    ListItem item = new ListItem(pageNumber.ToString());

                    // If the ListItem object matches the currently selected
                    // page, flag the ListItem object as being selected. Because
                    // the DropDownList control is recreated each time the pager
                    // row gets created, this will persist the selected item in
                    // the DropDownList control.   
                    if (i == gvSearch.PageIndex)
                    {
                        item.Selected = true;
                    }

                    // Add the ListItem object to the Items collection of the 
                    // DropDownList.
                    pageList.Items.Add(item);

                }

            }

            if (pageLabel != null)
            {

                // Calculate the current page number.
                int currentPage = gvSearch.PageIndex + 1;

                // Update the Label control with the current page information.
                pageLabel.Text = "Page " + currentPage.ToString() +
                  " of " + gvSearch.PageCount.ToString();

            }
        }
        catch (Exception ex)
        {
            ValidateSearchCtrl.ErrorMessage = ex.ToString();
        }

    }

    protected void PageDropDownList_SelectedIndexChanged(Object sender, EventArgs e)
    {
        // Retrieve the pager row.
        GridViewRow pagerRow = gvSearch.BottomPagerRow;

        // Retrieve the PageDropDownList DropDownList from the bottom pager row.
        DropDownList pageList = (DropDownList)pagerRow.Cells[0].FindControl("PageDropDownList");

        // Set the PageIndex property to display that page selected by the user.
        gvSearch.PageIndex = pageList.SelectedIndex;
    }

    private void RegisterClientJavascript()
    {
        string sFormName = "window." + this.Form.Name;

        StringBuilder sbJavascript = new StringBuilder();

        #region ClearScreen
        sbJavascript = new StringBuilder();
        sbJavascript.AppendLine("function ResetScreen ()");
        sbJavascript.AppendLine("{");
        sbJavascript.AppendLine(sFormName + ".document.all('" + this.txtLoanNumber.ClientID + "').disabled = false;");
        sbJavascript.AppendLine(sFormName + ".document.all('" + this.txtLoanNumber.ClientID + "').value = '';");
        sbJavascript.AppendLine(sFormName + ".document.all('" + this.txtSurname.ClientID + "').value = '';");
        sbJavascript.AppendLine(sFormName + ".document.all('" + this.txtFirstNames.ClientID + "').value = '';");
        sbJavascript.AppendLine("}");
        if (!Page.ClientScript.IsClientScriptBlockRegistered("ResetScreen"))
            Page.ClientScript.RegisterStartupScript(this.GetType(), "ResetScreen", sbJavascript.ToString(), true);
        #endregion
    }

   
}
