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
using SAHL.Common;
using SAHL.Common.Datasets;
using SAHL.Common.Authentication;
using SAHL.Web.Views.Common.Interfaces;
using System.Text;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;



public partial class Views_Life_History : SAHLCommonBaseView, IHistory
{
    protected void gridHistory_RowDataBound(object sender, GridViewRowEventArgs e)
    {
    }
    
    protected void gridPremiumHistory_RowDataBound(object sender, GridViewRowEventArgs e)
    {
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
       
    }

    #region IHistory Members

    public event EventHandler onCancelButtonClicked;

    public bool ShowHistoryGrid
    {
        set { throw new Exception("The method or operation is not implemented."); }
    }

    public bool ShowPremiumPanel
    {
        set { throw new Exception("The method or operation is not implemented."); }
    }

    public void BindHistoryGrid()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public void BindPremiumGrid()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    #endregion


    #region OldCode
    //int m_PolicyFSKey = -1;

    //protected override void OnInit(EventArgs e)
    //{
    //    base.OnInit(e);
    //    m_Controller = base.Controller as LifeController;
    //    m_Controller.m_UserSelectsKey = m_CBONavigator.SelectedItem.UserSelectsKey;

    //    PopulateLookups();

    //    // If the Policy Dataset is null then reload it
    //    if (m_Controller.PolicyDS == null)
    //    {
    //        m_Controller.LoadLifeData(Authenticator.GetFullWindowsUserName(), base.GetClientMetrics());
    //    }

    //    // Get the Fin Service Key from the CBO
    //    m_PolicyFSKey = m_Controller.GetCurrentPolicyFSKey(m_CBONavigator.SelectedItem.GenericTypeKey, m_CBONavigator.SelectedItem.GenericKey);
    //    m_Controller.GetPolicyFull(m_PolicyFSKey, base.GetClientMetrics());
    //    m_Controller.GetWorkflowState(m_CBONavigator);
  
    //    if (!ShouldRunPage())
    //        return;

    //    if (m_CBONavigator.SelectedItem.GenericTypeKey != 6) // Only show header in workflow mode
    //        WorkFlowHeader1.Visible = false;


    //    // Setup the Client Header
    //    try
    //    {
    //        Policy.vLifeLoanDetailsRow[] rows = m_Controller.PolicyDS.vLifeLoanDetails.Select("PolicyFSKey = " + m_PolicyFSKey.ToString()) as Policy.vLifeLoanDetailsRow[];
    //        WorkFlowHeader1.ClientName = m_CBONavigator.SelectedItem.LongDescription;
    //        WorkFlowHeader1.LoanNumber = rows[0].LoanAccountKey.ToString();
    //        WorkFlowHeader1.MortgageLoanPurpose = rows[0].MortgageLoanPurpose;
    //    }
    //    catch { }
    //}

    //protected void Page_Load(object sender, EventArgs e)
    //{
    //    if (!ShouldRunPage())
    //        return;
        
    //    gridHistory.AddGridBoundColumn("HistoryKey", "", new Unit(), HorizontalAlign.Left, false);
    //    gridHistory.AddGridBoundColumn("Date", "Date", new Unit(20, UnitType.Percentage), HorizontalAlign.Left, true);
    //    gridHistory.AddGridBoundColumn("UserName", "User", new Unit(15, UnitType.Percentage), HorizontalAlign.Left, true);
    //    gridHistory.AddGridBoundColumn("Action", "Action", new Unit(25, UnitType.Percentage), HorizontalAlign.Left, true);
    //    gridHistory.AddGridBoundColumn("", "Note", new Unit(50, UnitType.Percentage), HorizontalAlign.Left, true);

    //    gridPremiumHistory.AddGridBoundColumn("LifePremiumHistoryKey", "", new Unit(), HorizontalAlign.Left, false);
    //    gridPremiumHistory.AddGridBoundColumn("ChangeDate", "Date", new Unit(18, UnitType.Percentage), HorizontalAlign.Left, true);
    //    gridPremiumHistory.AddGridBoundColumn("", "User", new Unit(10, UnitType.Percentage), HorizontalAlign.Left, false);
    //    gridPremiumHistory.AddGridBoundColumn("DeathPremium", SAHL.Common.Constants.CurrencyFormat, SAHL.Common.Web.UI.Controls.GridFormatType.GridNumber, "Death Prem", false, new Unit(10, UnitType.Percentage), HorizontalAlign.Right, true);
    //    gridPremiumHistory.AddGridBoundColumn("IPBPremium", SAHL.Common.Constants.CurrencyFormat, SAHL.Common.Web.UI.Controls.GridFormatType.GridNumber, "IPB Prem", false, new Unit(10, UnitType.Percentage), HorizontalAlign.Right, true);
    //    gridPremiumHistory.AddGridBoundColumn("SumAssured", SAHL.Common.Constants.CurrencyFormat, SAHL.Common.Web.UI.Controls.GridFormatType.GridNumber, "Sum Assured", false, new Unit(10, UnitType.Percentage), HorizontalAlign.Right, true);
    //    gridPremiumHistory.AddGridBoundColumn("YearlyPremium", SAHL.Common.Constants.CurrencyFormat, SAHL.Common.Web.UI.Controls.GridFormatType.GridNumber, "Yearly Prem", false, new Unit(10, UnitType.Percentage), HorizontalAlign.Right, true);
    //    gridPremiumHistory.AddGridBoundColumn("PolicyFactor", "Policy Factor", new Unit(12, UnitType.Percentage), HorizontalAlign.Center, true);
    //    gridPremiumHistory.AddGridBoundColumn("DiscountFactor", "Discount Factor", new Unit(14, UnitType.Percentage), HorizontalAlign.Center, true);

    //    string select = "FinancialServiceKey = " + m_PolicyFSKey;

    //    DataRow[] hRows = m_Controller.PolicyDS.History.Select(select);
    //    DataRow[] phRows = m_Controller.PolicyDS.LifePremiumHistory.Select(select);

    //    gridHistory.DataSource = hRows;
    //    gridHistory.DataBind();

    //    gridPremiumHistory.DataSource = phRows;
    //    gridPremiumHistory.DataBind();
    //}

    //protected override void OnRemoveCBONode()
    //{
    //    m_Controller.RemovePolicy(m_RemovedGenericKey);
    //}

    //protected void gridHistory_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        TableCellCollection cells = e.Row.Cells;

    //        // Get the History Row
    //        DataRow[] drHistory = m_Controller.PolicyDS.History.Select("HistoryKey = " + cells[0].Text);
    //        Policy.HistoryRow hr = drHistory[0] as Policy.HistoryRow;

    //        //Date
    //        cells[1].Text = hr.Date.ToString(SAHL.Common.Constants.DateFormat + " HH:mm:ss");

    //        //note
    //        cells[4].Text = hr.IsNoteNull() ? "" : hr.Note;
    //    }
    //}
    //protected void gridPremiumHistory_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        TableCellCollection cells = e.Row.Cells;

    //        // Get the LifePremiumHistory Row
    //        DataRow[] drLifePremiumHistory = m_Controller.PolicyDS.LifePremiumHistory.Select("LifePremiumHistoryKey = " + cells[0].Text);
    //        Policy.LifePremiumHistoryRow hr = drLifePremiumHistory[0] as Policy.LifePremiumHistoryRow;

    //        //Date
    //        cells[1].Text = hr.ChangeDate.ToString(SAHL.Common.Constants.DateFormat + " HH:mm:ss");

    //        e.Row.Cells[2].Text = hr.IsUserNameNull() ? "" : hr.UserName;
    //    }
    //}
    //protected void btnCancel_Click(object sender, EventArgs e)
    //{
    //    m_Controller.LifeNavigate();
    //}
    #endregion

}
