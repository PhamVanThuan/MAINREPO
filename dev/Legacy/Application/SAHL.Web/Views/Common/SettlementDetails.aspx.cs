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

using Microsoft.ApplicationBlocks.UIProcess;

using System.Text;


using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;

namespace SAHL.Web.Views.Common
{
    public partial class SettlementDetails : SAHLCommonBaseView,ISettlementDetails
    {
        //SettlementController m_Controller;
        //string m_ViewMode = "view";
        //int m_AccountKey = -1;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            //PopulateLookups();
            //m_Controller = base.Controller as SettlementController;

            //ViewSettings viewSettings = UIPConfiguration.Config.GetViewSettingsFromName(base.ViewName);
            //if (viewSettings.CustomAttributes.Count > 0)
            //{
            //    System.Xml.XmlNode PageStateNode = viewSettings.CustomAttributes.GetNamedItem("state");
            //    if (PageStateNode != null)
            //    {
            //        m_ViewMode = PageStateNode.Value;
            //    }
            //}
            //m_AccountKey = 1318286; //m_CBONavigator.SelectedItem.GenericKey;

            //m_Controller.SettlementBankDetailsGet(m_AccountKey, base.GetClientMetrics());        

            //BindGrid();
            //BindData();
            //BindControls();
        }


        protected override void OnPreRenderComplete(EventArgs e)
        {
            //base.OnPreRenderComplete(e);

            //txtDebtType.Attributes["onchange"] = "SetModifiedTrue();" + txtDebtType.Attributes["onchange"];
            //txtTotalOutstanding.Attributes["onkeypress"] = "SetModifiedTrue();" + txtTotalOutstanding.Attributes["onkeypress"];
            //txtMonthlyInstallment.Attributes["onkeypress"] = "SetModifiedTrue();" + txtMonthlyInstallment.Attributes["onkeypress"];            
            //txtReference.Attributes["onchange"] = "SetModifiedTrue();" + txtReference.Attributes["onchange"];

            ////txtAttorneyName.Attributes["onchange"] = "SetModifiedTrue();" + txtAttorneyName.Attributes["onchange"];
            ////txtTelephoneCode.Attributes["onchange"] = "SetModifiedTrue();" + txtTelephoneCode.Attributes["onchange"];
            ////txtContactNumber.Attributes["onchange"] = "SetModifiedTrue();" + txtContactNumber.Attributes["onchange"];
            ////txtContactPerson.Attributes["onchange"] = "SetModifiedTrue();" + txtContactPerson.Attributes["onchange"];

            //switch (m_ViewMode)
            //{
            //    case ("view"):
            //        {
            //            panelSettlementDetails.Visible = true;
            //            panelSettlement.Visible = false;
            //            btnCancel.Visible = false;
            //            btnUpdate.Visible = false;              
            //            txtBank.ReadOnly = true;
            //            txtBranch.ReadOnly = true;
            //            txtAccountType.ReadOnly = true;
            //            txtAccountNumber.ReadOnly = true;
            //            txtAccountName.ReadOnly = true;
            //            break;
            //        }

            //    case "add":
            //        {
            //            panelSettlement.Visible = true;
            //            panelSettlementDetails.Visible = true;
            //        //    panelGrid.Visible = false;
            //            panelBankDetails.Visible = false;
            //           // SAHLGridBanking.Visible = false;                                                                             
            //            btnUpdate.Text = "Add Banking Details";
            //            btnUpdate.ButtonSize = SAHL.Common.Web.UI.Controls.ButtonSizeTypes.Size6;                 
            //            txtBank.ReadOnly = false;
            //            txtBranch.ReadOnly = false;
            //            txtAccountType.ReadOnly = false;
            //            txtAccountNumber.ReadOnly = false;
            //            txtAccountName.ReadOnly = false;


            //            txtDebtType.Text = m_Controller.Lookups.ExpenseType.FindByExpenseTypeKey(8).Description;

            //            break;
            //        }
            //    case "update":
            //        {
            //            panelSettlement.Visible = true;
            //            panelSettlementDetails.Visible = true;
            //           // panelGrid.Visible = true;
            //            panelBankDetails.Width = Unit.Percentage(50);
            //            panelBankDetails.Visible = true;
            //           // SAHLGridBanking.Visible = true;
            //            btnUpdate.Text = "Update";               
            //            txtBank.ReadOnly = false;
            //            txtBranch.ReadOnly = false;
            //            txtAccountType.ReadOnly = false;
            //            txtAccountNumber.ReadOnly = false;
            //            txtAccountName.ReadOnly = false;                   
            //            break;
            //        }

            //    case "remove":
            //        {
            //            panelSettlement.Visible = true;
            //            panelSettlementDetails.Visible = false;
            //          //  panelGrid.Visible = true;
            //            panelBankDetails.Visible = true;
            //           // SAHLGridBanking.Visible = true;
            //            btnUpdate.Text = "Remove";              
            //            txtBank.ReadOnly = true;
            //            txtBranch.ReadOnly = true;
            //            txtAccountType.ReadOnly = true;
            //            txtAccountNumber.ReadOnly = true;
            //            txtAccountName.ReadOnly = true;
            //            break;
            //        }
            //}
        }
        //private void BindData()
        //{
            // get the data for this account and bind it to the grids and other controls
            //        SAHLGridBanking.Columns.Clear();
            //        SAHLGridBanking.AddGridBoundColumn("ExpenseKey", "", Unit.Percentage(0), HorizontalAlign.Left, false);
            //        SAHLGridBanking.AddGridBoundColumn("ExpenseAccountName", "Account Name", Unit.Percentage(20), HorizontalAlign.Left, true);
            //        SAHLGridBanking.AddGridBoundColumn("", "Debt Type", Unit.Percentage(20), HorizontalAlign.Left, true);
            //        SAHLGridBanking.AddGridBoundColumn("TotalOutstandingAmount", "Total Outstanding", Unit.Percentage(12), HorizontalAlign.Left, true);
            //        SAHLGridBanking.AddGridBoundColumn("MonthlyPayment", "Monthly", Unit.Percentage(8), HorizontalAlign.Left, true);
            //        SAHLGridBanking.AddGridBoundColumn("", "Amount To Settle", Unit.Percentage(15), HorizontalAlign.Left, true);
            //        SAHLGridBanking.AddGridBoundColumn("ToBeSettled", "Settle", Unit.Percentage(8), HorizontalAlign.Left, true);
            //        SAHLGridBanking.AddGridBoundColumn("", "Account Number", Unit.Percentage(15), HorizontalAlign.Left, true);
            //        SAHLGridBanking.AddGridBoundColumn("ExpenseReference", "Reference", Unit.Percentage(10), HorizontalAlign.Left, true);     
            //        SAHLGridBanking.DataSource = m_Controller.m_ExpenseDS.AccountExpense;

            //        SAHLGridBanking.DataBind();

            //        if (SAHLGridBanking.Rows.Count > 0)
            //        {
            //            PopulateBankSettlementDetailsControls(int.Parse(SAHLGridBanking.Rows[0].Cells[0].Text));
            //            if (m_viewMode != "add")
            //            {
            //                PopulateSettlementControls(int.Parse(SAHLGridBanking.Rows[0].Cells[0].Text));
            //            }
            ////            m_Controller.SelectedGridIndex = 0;
            //        }

            //m_Controller.GetExpensesForAccount(m_AccountKey, base.GetClientMetrics());

            // Bind to the grid
            //     SAHLGridBanking.DataSource = m_Controller.BankAccountDS.AccountExpense;





        //}


        //private void BindGrid()
        //{
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

        //private void BindControls()
        //{


        //    // get the selected grid aerow if one
        //    // set the control data based on the selected aerow
        //}

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            // turn the panels on or off based on viewmode

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //RegisterClientScripts();
            //AjaxPro.Utility.RegisterTypeForAjax(typeof(Views_SettlementDetails));
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {

            //switch (m_ViewMode)
            //{
            //    case "add":
            //        {

            //            break;
            //        }
            //    case "update":
            //        {

            //            break;
            //        }            
            //}
            //// now do the update on the dataset 


        }



        protected void SAHLGridBanking_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void SAHLGridBanking_SelectedIndexChanged(object sender, EventArgs e)
        {
            //        switch(m_ViewMode)
            //        {
            //            case "view":
            //                {
            ////                    PopulateBankSettlementDetailsControls(Convert.ToInt32(SAHLGridBanking.Rows[SAHLGridBanking.SelectedIndexInternal].Cells[0].Text));

            //                    break;
            //                }
            //            case "add":
            //                {                   
            //                    txtMonthlyInstallment.ReadOnly = false;
            //                    txtTotalOutstanding.ReadOnly = false;

            //                    txtReference.ReadOnly = false;                  
            //                    break;
            //                }
            //            case "update":
            //                {
            ////                    PopulateSettlementControls(Convert.ToInt32(SAHLGridBanking.Rows[SAHLGridBanking.SelectedIndexInternal].Cells[0].Text));
            //                    txtMonthlyInstallment.ReadOnly = false;
            //                    txtTotalOutstanding.ReadOnly = false;

            //                    txtReference.ReadOnly = false;                 
            //                    break;
            //                }
            //            case "remove":
            //                {
            //          //          PopulateSettlementControls(Convert.ToInt32(SAHLGridBanking.Rows[SAHLGridBanking.SelectedIndexInternal].Cells[0].Text));

            //                    txtMonthlyInstallment.ReadOnly = true;
            //                    txtTotalOutstanding.ReadOnly = true;

            //                    txtReference.ReadOnly = true;

            //                    break;
            //                }
            //    }
            //     //   m_Controller.SelectedGridIndex = SAHLGridBanking.SelectedIndex;
        }


        //private void PopulateBankSettlementDetailsControls(int ExpenseKey)
        //{
            //if (ExpenseKey == -1)
            //    return;




            ////  Policy.LegalEntityRow LR = null;

            ////       if (SAHLGridBanking.Rows.Count > 0 && SAHLGridBanking.SelectedRow != null)
            ////       {
            ////Expense.AccountExpenseRow aeRow = m_Controller.ExpenseAccountDS.AccountExpense.FindByExpenseKey(int.Parse(SAHLGridBanking.SelectedRow.Cells[0].Text));
            ////if (aeRow != null)
            ////{

            ////    Expense.AccountDebtSettlementRow[] adsRow = aeRow.GetAccountDebtSettlementRows();
            ////    Expense.BankAccountRow mBankRow = adsRow[0].BankAccountRow;
            ////    SAHL.Datasets.Lookup.ACBBranchRow mBranchRow = m_Controller.Lookups.ACBBranch.FindByACBBranchCode(mBankRow.ACBBranchCode);
            ////    if (mBranchRow != null)
            ////    {
            ////        SAHL.Datasets.Lookup.ACBBankRow mBankNameRow = m_Controller.Lookups.ACBBank.FindByACBBankCode(mBranchRow.ACBBankCode);
            ////        if (mBankNameRow != null)
            ////        {
            ////            txtBank.Text = mBankNameRow.ACBBankDescription;
            ////        }
            ////    }
            ////    txtBranch.Text = m_Controller.Lookups.ACBBranch.FindByACBBranchCode(mBankRow.ACBBranchCode).ACBBranchDescription;
            ////    SAHL.Datasets.Lookup.ACBTypeRow  mTypeRow = m_Controller.Lookups.ACBType.FindByACBTypeNumber(mBankRow.ACBTypeNumber);
            ////    if (mTypeRow != null)
            ////    {                    
            ////        txtAccountType.Text = mTypeRow.ACBTypeDescription;
            ////    }                
            ////    txtAccountNumber.Text = mBankRow.AccountNumber;
            ////    txtAccountName.Text = mBankRow.AccountName;
            ////   }

            ////}
        //}

        //private void PopulateSettlementControls(int expenseKey)
        //{

        //}

        //private void RegisterClientScripts()
        //{
            //StringBuilder mBuilder = new StringBuilder();


            //mBuilder.AppendLine("var isModified = false;");

            //mBuilder.AppendLine("function SetModifiedTrue()");
            //mBuilder.AppendLine("{");        
            //mBuilder.AppendLine("isModified = true;");
            //mBuilder.AppendLine("}");


            //mBuilder.AppendLine("function SetModifiedFalse()");
            //mBuilder.AppendLine("{");
            //mBuilder.AppendLine("isModified = false;");
            //mBuilder.AppendLine("}");

            //mBuilder.AppendLine("function GetModifiedStatus()");
            //mBuilder.AppendLine("{");       
            //mBuilder.AppendLine("if(isModified == false)");
            //mBuilder.AppendLine("{");
            //mBuilder.AppendLine("Views_SettlementDetails.SaveChanges()");          
            //mBuilder.AppendLine("}");
            //mBuilder.AppendLine("else");
            //mBuilder.AppendLine("{");             
            //mBuilder.AppendLine("alert('Please save changes before updating the banking details!');");
            //mBuilder.AppendLine("}");
            //mBuilder.AppendLine("}");

            //mBuilder.AppendLine("function ServerSideCallBack(response)");
            //mBuilder.AppendLine("{");       
            //mBuilder.AppendLine("}");


            //if (!Page.ClientScript.IsClientScriptBlockRegistered(this.GetType(), "lstScripts"))
            //{
            //    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "lstScripts", mBuilder.ToString(), true);
            //}
        //}

        //[AjaxPro.AjaxMethod()]

        //public void SaveChanges()
        //{

        //}

        protected void btnUpdate_Click1(object sender, EventArgs e)
        {

        }

        protected void btnUpdateBankingDetails_Click(object sender, EventArgs e)
        {

        }


        protected void DetailsGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    e.Row.Cells[1].Text = e.Row.Cells[1].Text + " " + e.Row.Cells[2].Text;
            //    e.Row.Cells[3].Text = e.Row.Cells[3].Text + " " + e.Row.Cells[4].Text;
            //}
        }
        protected void DetailsGrid_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        #region ISettlementDetails Members

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnCancelButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnSubmitButtonClicked;

        #endregion
    }
}