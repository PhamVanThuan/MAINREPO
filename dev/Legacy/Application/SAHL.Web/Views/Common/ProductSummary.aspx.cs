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
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI.Controls;
using System.Collections.Generic;
using SAHL.Common.Globals;

namespace SAHL.Web.Views.Common
{
    public partial class ProductSummary : SAHLCommonBaseView, IProductSummary
    {
        private enum SummaryGridColumnPositions
        {
            AccountKey=0,
            Product=1,
            AccountStatus=2
        }

        private enum FinancialAdjustmentGridColumnPositions
        {
            AccountKey = 0,
            FinancialAdjustmentSource = 1,
            FinancialAdjustmentType = 2,
            FromDate = 3,
            Term = 4,
            Value = 5,
            Status = 6

        }

        #region IProductSummary Members

        public void BindSummaryGrid(IList<SAHL.Common.BusinessModel.Interfaces.IAccount> accounts)
        {
            SummaryGrid.AddGridBoundColumn("", "Account Number", Unit.Percentage(20), HorizontalAlign.Left, true);
            SummaryGrid.AddGridBoundColumn("", "Product", Unit.Percentage(50), HorizontalAlign.Left, true);
            SummaryGrid.AddGridBoundColumn("", "Status", Unit.Percentage(30), HorizontalAlign.Left, true);

            SummaryGrid.DataSource = accounts;
            SummaryGrid.DataBind();
        }

        public void BindFinancialAdjustmentGrid(IList<IFinancialAdjustment> financialAdjustments)
        {
            FinancialAdjustmentGrid.AddGridBoundColumn("", "Account", Unit.Percentage(8), HorizontalAlign.Left, true);
            FinancialAdjustmentGrid.AddGridBoundColumn("", "Financial Adjustment", Unit.Percentage(20), HorizontalAlign.Left, true);
            FinancialAdjustmentGrid.AddGridBoundColumn("", "Financial Adjustment Type", Unit.Percentage(20), HorizontalAlign.Left, true);
            FinancialAdjustmentGrid.AddGridBoundColumn("", SAHL.Common.Constants.DateFormat, GridFormatType.GridDate, "Effective Date", false, Unit.Percentage(12), HorizontalAlign.Center, true);
            FinancialAdjustmentGrid.AddGridBoundColumn("", "Term", Unit.Percentage(5), HorizontalAlign.Left, true);
            FinancialAdjustmentGrid.AddGridBoundColumn("", "Value", Unit.Percentage(10), HorizontalAlign.Left, true);
            FinancialAdjustmentGrid.AddGridBoundColumn("", "Status", Unit.Percentage(11), HorizontalAlign.Left, true);
            FinancialAdjustmentGrid.DataSource = financialAdjustments;
            FinancialAdjustmentGrid.DataBind();
        }

        #endregion

        protected void SummaryGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                // Get the LifePremiumHistory Row
                IAccount account = e.Row.DataItem as IAccount;

                e.Row.Cells[(int)SummaryGridColumnPositions.AccountKey].Text = account.Key.ToString();
                e.Row.Cells[(int)SummaryGridColumnPositions.Product].Text = account.Product.Description;
                e.Row.Cells[(int)SummaryGridColumnPositions.AccountStatus].Text = account.AccountStatus.Description;
            }
        }

        protected void FinancialAdjustmentGrid_RowDataBound(object sender, GridViewRowEventArgs e) 
        {
            if (e.Row.DataItem != null)
            {
                // Get the FinancialAdjustment Row
                IFinancialAdjustment financialAdjustment = e.Row.DataItem as IFinancialAdjustment;

                e.Row.Cells[(int)FinancialAdjustmentGridColumnPositions.AccountKey].Text = financialAdjustment.FinancialService.Account.Key.ToString();
                e.Row.Cells[(int)FinancialAdjustmentGridColumnPositions.FinancialAdjustmentSource].Text = financialAdjustment.FinancialAdjustmentSource.Description ;
                e.Row.Cells[(int)FinancialAdjustmentGridColumnPositions.FinancialAdjustmentType].Text = financialAdjustment.FinancialAdjustmentType.Description ;
                e.Row.Cells[(int)FinancialAdjustmentGridColumnPositions.FromDate].Text = financialAdjustment.FromDate.HasValue ? financialAdjustment.FromDate.Value.ToString(SAHL.Common.Constants.DateFormat) : "";
                e.Row.Cells[(int)FinancialAdjustmentGridColumnPositions.Term].Text = financialAdjustment.Term.ToString();
                e.Row.Cells[(int)FinancialAdjustmentGridColumnPositions.Value].Text = financialAdjustment.ToString();
                e.Row.Cells[(int)FinancialAdjustmentGridColumnPositions.Status].Text =  financialAdjustment.FinancialAdjustmentStatus.Description ;

                switch (financialAdjustment.FinancialAdjustmentStatus.Key)
                {
                    case (int)FinancialAdjustmentStatuses.Active:
                        e.Row.Attributes.Add("style", "color: #FFFFFF; background-color:green;");
                        break;
                    case (int)FinancialAdjustmentStatuses.Inactive:
                        e.Row.Attributes.Add("style", "color: #FFFFFF; background-color:#e66426;");
                        break;
                    case (int)FinancialAdjustmentStatuses.Canceled:
                        e.Row.Attributes.Add("style", "color: #FFFFFF; background-color:red;");
                        break;
                    default:
                        break;
                }

            }
        }
    }
}
