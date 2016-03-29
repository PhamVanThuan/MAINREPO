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
using System.Text;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;


namespace SAHL.Web.Views.Common
{

    public partial class HOCSummary : SAHLCommonBaseView, IHOCSummary
    {
        //private IHOC _hocAcct;

        /// <summary>
        /// Bind HOC Master Data Controls
        /// </summary>
        /// <param name="lstHOC"></param>
        public void BindMasterDataControls(IHOC lstHOC)
        {

            double dCurrentSumAssured = 0;
            double dHOCTotalPremium = 0;

            bool GetFirstHOC = true;

            IHOC _HOCAcct = lstHOC;

            dCurrentSumAssured = dCurrentSumAssured + Convert.ToDouble(_HOCAcct.HOCTotalSumInsured);
            dHOCTotalPremium = dHOCTotalPremium + Convert.ToDouble(_HOCAcct.HOCProrataPremium) + Convert.ToDouble(_HOCAcct.HOCMonthlyPremium);

            if (GetFirstHOC == true)
            {
                DateTime OpenDate = Convert.ToDateTime(_HOCAcct.CommencementDate);
                DateTime CloseDate = Convert.ToDateTime(_HOCAcct.CancellationDate);

                lblAccountNumber.Text = _HOCAcct.FinancialService.Account.Key.ToString();
                lblProduct.Text = _HOCAcct.FinancialService.FinancialServiceType.Description.ToString();
                lblStatus.Text = _HOCAcct.FinancialService.AccountStatus.Description.ToString();
                lblOpenDate.Text = _HOCAcct.CommencementDate == null ? "-" : OpenDate.ToString(SAHL.Common.Constants.DateFormat);
                lblCloseDate.Text = _HOCAcct.CancellationDate == null ? "-" : CloseDate.ToString(SAHL.Common.Constants.DateFormat);

                GetFirstHOC = false;
            }


            lblTotalHOCSumInsured.Text = dCurrentSumAssured.ToString(SAHL.Common.Constants.CurrencyFormat);
            lblTotalPremium.Text = dHOCTotalPremium.ToString(SAHL.Common.Constants.CurrencyFormat);

        }

        /// <summary>
        /// Bind HOC Grid Controls
        /// </summary>
        /// <param name="lstHOCGrid"></param>
        public void BindDetailDataGrid(List<IHOC> lstHOCGrid)
        {
            //_hocAcct = lstHOCGrid;

            HOCGrid.AddGridBoundColumn("", "HOC Insurer", Unit.Percentage(19), HorizontalAlign.Left, true);
            HOCGrid.AddGridBoundColumn("", "Roof Description", Unit.Percentage(14), HorizontalAlign.Left, true);
            HOCGrid.AddGridBoundColumn("", "Sum Insured", Unit.Percentage(15), HorizontalAlign.Right, true);
            HOCGrid.AddGridBoundColumn("", "Premium", Unit.Percentage(12), HorizontalAlign.Right, true);
            HOCGrid.AddGridBoundColumn("", "Policy Number", Unit.Percentage(15), HorizontalAlign.Left, true);
            HOCGrid.AddGridBoundColumn("", "Commencement", Unit.Percentage(10), HorizontalAlign.Center, true);
            HOCGrid.AddGridBoundColumn("", "Anniversary", Unit.Percentage(15), HorizontalAlign.Center, true);

            HOCGrid.DataSource = lstHOCGrid;
            HOCGrid.DataBind();

        }
        /// <summary>
        /// Format Grid data in rowdata bound event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void HOCGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            TableCellCollection cells = e.Row.Cells;

            DateTime CommencementDate;
            DateTime AnniversaryDate;
            Double dHOCMonthlyPremium;
            Double dHOCTotalSumInsured;

            IHOC hoc = e.Row.DataItem as IHOC;

            if (e.Row.DataItem != null)
            {
                cells[0].Text = hoc.HOCInsurer.Description.ToString();
                cells[1].Text = hoc.HOCRoof.Description.ToString();

                dHOCTotalSumInsured = Convert.ToDouble(hoc.HOCTotalSumInsured);
                cells[2].Text = dHOCTotalSumInsured.ToString(SAHL.Common.Constants.CurrencyFormat);

                dHOCMonthlyPremium = Convert.ToDouble(hoc.HOCMonthlyPremium);
                cells[3].Text = dHOCMonthlyPremium.ToString(SAHL.Common.Constants.CurrencyFormat);              
                cells[4].Text = hoc.HOCPolicyNumber == null ? "-" : hoc.HOCPolicyNumber.ToString();
                CommencementDate = Convert.ToDateTime(hoc.CommencementDate);
                AnniversaryDate = Convert.ToDateTime(hoc.AnniversaryDate);

                cells[5].Text = hoc.CommencementDate == null ? "-" : CommencementDate.ToString(SAHL.Common.Constants.DateFormat);
                cells[6].Text = hoc.AnniversaryDate == null ? "-" : AnniversaryDate.ToString(SAHL.Common.Constants.DateFormat);


            }
        }
    }


}