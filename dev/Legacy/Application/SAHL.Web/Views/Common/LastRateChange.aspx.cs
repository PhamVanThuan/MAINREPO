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
using SAHL.Common.Authentication;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using SAHL.Common.Web.UI.Controls;

namespace SAHL.Web.Views.Common
{
    public partial class LastRateChange : SAHLCommonBaseView,ILastRateChange
    {
        #region private variables

        //private LoanCalculatorController m_MyController = null; // this needs to change to the right controller
        //private string m_PageState = "display";
        //private Metrics p_MI;

        ////Account detail
        //eCalculator.AccountProductsDataTable m_AccProdDT;
        //eCalculator.LoanTransactionDataTable m_DT;

        //private int m_AccountKey = 0;

        #endregion

        #region Screen Methods

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!ShouldRunPage())
            //    return; // Navigate to another screen 

            //m_AccountKey = int.Parse(m_CBONavigator.SelectedItem.GenericKey);
            //BindData();
            //BindGrid();

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            //m_MyController = base.Controller as LoanCalculatorController;

            //if (!ShouldRunPage())
            //    return; // Navigate to another screen 

            //ViewSettings viewSettings = UIPConfiguration.Config.GetViewSettingsFromName(base.ViewName);

            //if (viewSettings.CustomAttributes.Count > 0)
            //{
            //    System.Xml.XmlNode PageStateNode = null;

            //    PageStateNode = viewSettings.CustomAttributes.GetNamedItem("state");
            //    if (PageStateNode != null)
            //    {
            //        m_PageState = PageStateNode.Value;
            //    }
            //}
        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
            Navigator.Navigate("LoanCalculator");
        }

        #endregion

        #region General methods

        //private void BindData()
        //{

        //    //if (p_MI == null)
        //    //    p_MI = new Metrics();

        //    //int p_CA = m_CBONavigator.DataSource.CompanyAccess[0].OriginationSourceKey;
        //    //eCalculator.LoanTransactionRow Row;
        //    //int RemTerm = 0;

        //    //m_AccountKey = int.Parse(m_CBONavigator.SelectedItem.GenericKey);

        //    //m_DT = new eCalculator.LoanTransactionDataTable();
        //    //m_AccProdDT = new eCalculator.AccountProductsDataTable();

        //    //// Load the products linked to the Account
        //    //m_MyController.GetAccountProducts(m_AccountKey, p_MI);
        //    //m_AccProdDT = m_MyController.AccountProductsDT;

        //    //foreach (eCalculator.AccountProductsRow P_Row in m_AccProdDT)
        //    //{
        //    //    Row = m_MyController.GetLoanTransaction(P_Row.FinancialServiceKey, m_AccountKey, p_MI);

        //    //    if (Row != null)
        //    //    {
        //    //        RemTerm = Row.RemainingTerm + P_Row.RemainingTerm;
        //    //        m_DT.AddLoanTransactionRow(P_Row.Service, Row.LoanNumber, Row.LoanAmount, RemTerm, Row.InterestRate, Row.Instalment, Row.LoanTransactionEffectiveDate);
        //    //    }
        //    //}
        //}

        //private void BindGrid()
        //{

            //grdLastRateChange.AddGridBoundColumn("FinServiceType", "Service", 150, HorizontalAlign.Left, true);
            //grdLastRateChange.AddGridBoundColumn("LoanAmount", m_MyController.CurrencyFormat, GridFormatType.GridNumber, "Loan Amount", true, Unit.Percentage(20), HorizontalAlign.Right, true);
            //grdLastRateChange.AddGridBoundColumn("RemainingTerm", m_MyController.MonthFormat, GridFormatType.GridNumber, "Remaining Term", false, Unit.Percentage(15), HorizontalAlign.Right, true);
            //grdLastRateChange.AddGridBoundColumn("InterestRate", m_MyController.PercentageFormat, GridFormatType.GridNumber, "Interest Rate", false, Unit.Percentage(20), HorizontalAlign.Right, true);
            //grdLastRateChange.AddGridBoundColumn("Instalment", m_MyController.CurrencyFormat, GridFormatType.GridNumber, "Instalment", true, Unit.Percentage(20), HorizontalAlign.Right, true);

            //grdLastRateChange.DataSource = m_DT;
            //grdLastRateChange.DataBind();

        //}

        #endregion


        #region ILastRateChange Members

        /// <summary>
        /// 
        /// </summary>
        public void BindGrid(IList<IMortgageLoan> lstMortgageLoans)
        {
            grdLastRateChange.AddGridBoundColumn("FinancialServiceType", "Service", 150, HorizontalAlign.Left, true);
            grdLastRateChange.AddGridBoundColumn("CurrentBalance", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "Loan Amount", true, Unit.Percentage(20), HorizontalAlign.Right, true);
            grdLastRateChange.AddGridBoundColumn("RemainingInstallments", "Remaining Term", Unit.Percentage(15), HorizontalAlign.Right, true);
            grdLastRateChange.AddGridBoundColumn("InterestRate", SAHL.Common.Constants.RateFormat, GridFormatType.GridNumber, "Interest Rate", false, Unit.Percentage(20), HorizontalAlign.Right, true);
            grdLastRateChange.AddGridBoundColumn("Payment", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "Instalment", true, Unit.Percentage(20), HorizontalAlign.Right, true);
            grdLastRateChange.DataSource = lstMortgageLoans;
            grdLastRateChange.DataBind();
        }

        #endregion

        protected void grdLastRateChange_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                IMortgageLoan mortgageLoan = e.Row.DataItem as IMortgageLoan;
                TableCellCollection cells = e.Row.Cells;
                cells[0].Text = mortgageLoan.FinancialServiceType.Description;
            }
        }
    }
}