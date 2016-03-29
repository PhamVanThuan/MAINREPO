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
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.Web.UI.Controls;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.BusinessModel;
using SAHL.Common.BusinessModel.DataTransferObjects;

namespace SAHL.Web.Views.Common
{
    public partial class GroupExposure : SAHLCommonBaseView , IGroupExposure
    {
        #region Event Handlers

        /// <summary>
        /// 
        /// </summary>
        public event KeyChangedEventHandler OnLegalEntityGridSelectedIndexChanged;

        /// <summary>
        /// 
        /// </summary>
        public event KeyChangedEventHandler OnGroupExposureGridSelectedIndexChanged;
        
        #endregion

        private enum LegalEntityGridColumns
        {
            LegalEntityName = 0,
            IDNumber,
            LegalEntityIncome,
            EmploymentType,
            SAHLAccounts,
            SAHLArrears,
            SAHLApplications,
            SAHLDeclinedApplications
        }

        private enum GroupExposureGridColumns
        {
            AccountKey = 0,
            OfferKey,
            Product,
            Status,
            OwnerOccupied,
            RoleDescription,
            CurrentBalance,
            ArrearBalance,
            LoanAgreementAmount,
            LatestValuationAmount,
            InstalmentAmount,
            HouseholdIncome,
            LTV,
            PTI
        }

        #region Protected Functions Section
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LegalEntityGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            ILookupRepository _lookupRepository = RepositoryFactory.GetRepository<ILookupRepository>();

            if (e.Row.DataItem != null)
            {
                DataRowView ro = e.Row.DataItem as DataRowView;
                if (ro.Row != null && Convert.ToInt32(ro.Row["EmploymentType"]) != -1)
                {
                    e.Row.Cells[(int)LegalEntityGridColumns.EmploymentType].Text = _lookupRepository.EmploymentTypes.ObjectDictionary[ro.Row["EmploymentType"].ToString()].Description;
                }
                else
                {
                    e.Row.Cells[(int)LegalEntityGridColumns.EmploymentType].Text = "Unknown";
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DebtCounsellingGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                IDetail detail = e.Row.DataItem as IDetail;
                e.Row.Cells[0].Text = detail.Account.Key.ToString();
                e.Row.Cells[1].Text = detail.DetailType.Description;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LegalEntityGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            KeyChangedEventArgs args = new KeyChangedEventArgs(LegalEntityGrid.SelectedIndex);
            if (OnLegalEntityGridSelectedIndexChanged != null)
            {
                GroupExposureGrid.SelectedIndex = 0;
                OnLegalEntityGridSelectedIndexChanged(sender, args);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GroupExposureGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            int accountKey = -1;
            TableRow selectedRow = GroupExposureGrid.Rows[GroupExposureGrid.SelectedIndex];

            accountKey = Convert.ToInt32(selectedRow.Cells[(int)GroupExposureGridColumns.AccountKey].Text);

            KeyChangedEventArgs args = new KeyChangedEventArgs(accountKey);
            OnGroupExposureGridSelectedIndexChanged(sender, args);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Implements <see cref="IGroupExposure.LegalEntityGridText"/>
        /// </summary>
        public string LegalEntityGridText
        {
            set { lblLegalEntity.Text = value; }
        }

        /// <summary>
        /// Implements <see cref="IGroupExposure.GroupExposureGridText"/>
        /// </summary>
        public string GroupExposureGridText
        {
            set { lblGroupExposure.Text = value; }
        }

        /// <summary>
        /// Implements <see cref="IGroupExposure.DebtCounsellingGridVisible"/>
        /// </summary>
        public bool DebtCounsellingGridVisible
        {
            set { 
                DebtCounsellingGrid.Visible  = value;
                lblDebtCounsellingHeading.Visible = value;
            }
        }

        /// <summary>
        /// Implements <see cref="IGroupExposure.LegalEntityGridSelectedIndex"/>
        /// </summary>
        public int LegalEntityGridSelectedIndex
        {
            get {
                return LegalEntityGrid.SelectedIndexInternal;
            }
        }

        #endregion

        #region IGroupExposure Members

        /// <summary>
        /// 
        /// </summary>
        /// <param name="legalEntityDT"></param>
        public void BindLegalEntityGrid(DataTable legalEntityDT)
        {
            LegalEntityGrid.Columns.Clear();
            LegalEntityGrid.ShowFooter = true;
            LegalEntityGrid.AddGridBoundColumn("LegalEntityName", "Legal Entity Name", Unit.Percentage(40), HorizontalAlign.Left, true);
            LegalEntityGrid.AddGridBoundColumn("IDNumber", "ID Number", Unit.Percentage(20), HorizontalAlign.Left, false);
            LegalEntityGrid.AddGridBoundColumn("LegalEntityIncome", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "Income", true, Unit.Percentage(20), HorizontalAlign.Right, true);
            LegalEntityGrid.AddGridBoundColumn("", "Employment Type", Unit.Percentage(20), HorizontalAlign.Left, true);
            LegalEntityGrid.AddGridBoundColumn("SAHLAccounts", "SAHL Accounts", Unit.Percentage(5), HorizontalAlign.Center, true);
            LegalEntityGrid.AddGridBoundColumn("SAHLArrears", "SAHL Arrears", Unit.Percentage(5), HorizontalAlign.Center, true);
            LegalEntityGrid.AddGridBoundColumn("SAHLApplications", "SAHL Apps", Unit.Percentage(5), HorizontalAlign.Center, true);
            LegalEntityGrid.AddGridBoundColumn("SAHLDeclinedApplications", "Declined Apps", Unit.Percentage(5), HorizontalAlign.Center, true);
            LegalEntityGrid.DataSource = legalEntityDT;
            LegalEntityGrid.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="legalEntityGroupExposure"></param>
        public void BindGroupExposureGrid(LegalEntityGroupExposure legalEntityGroupExposure)
        {
            GroupExposureGrid.Columns.Clear();
            GroupExposureGrid.ShowFooter = true;
            GroupExposureGrid.AddGridBoundColumn("AccountKey", "Account", Unit.Percentage(5), HorizontalAlign.Left, true);
            GroupExposureGrid.AddGridBoundColumn("OfferKey", "Offer", Unit.Percentage(5), HorizontalAlign.Left, true);
            GroupExposureGrid.AddGridBoundColumn("Product", "Product", Unit.Percentage(5), HorizontalAlign.Left, true);
            GroupExposureGrid.AddGridBoundColumn("Status", "Status", Unit.Percentage(5), HorizontalAlign.Center, true);
            GroupExposureGrid.AddGridBoundColumn("DisplayOwnerOccupied", "Owner Occ", Unit.Percentage(5), HorizontalAlign.Center, true);
            GroupExposureGrid.AddGridBoundColumn("RoleDescription", "Role", Unit.Percentage(5), HorizontalAlign.Left, true);
            GroupExposureGrid.AddGridBoundColumn("CurrentBalance", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "Current Balance", true, Unit.Percentage(10), HorizontalAlign.Right, true);
            GroupExposureGrid.AddGridBoundColumn("ArrearBalance", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "Arrear Balance (Current)", true, Unit.Percentage(10), HorizontalAlign.Right, true);
            GroupExposureGrid.AddGridBoundColumn("LoanAgreementAmount", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "Loan Agreement Amount", true, Unit.Percentage(10), HorizontalAlign.Right, true);
            GroupExposureGrid.AddGridBoundColumn("LatestValuationAmount", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "Latest Valuation", true, Unit.Percentage(10), HorizontalAlign.Right, true);
            GroupExposureGrid.AddGridBoundColumn("InstalmentAmount", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "Loan Instalment", true, Unit.Percentage(10), HorizontalAlign.Right, true);
            GroupExposureGrid.AddGridBoundColumn("HouseholdIncome", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "Household Income", false, Unit.Percentage(10), HorizontalAlign.Right, true);
            GroupExposureGrid.AddGridBoundColumn("LTV", SAHL.Common.Constants.RateFormat, GridFormatType.GridNumber, "LTV", false, Unit.Percentage(5), HorizontalAlign.Right, true);
            GroupExposureGrid.AddGridBoundColumn("PTI", SAHL.Common.Constants.RateFormat, GridFormatType.GridNumber, "PTI", false, Unit.Percentage(5), HorizontalAlign.Right, true);
            GroupExposureGrid.DataSource = legalEntityGroupExposure.GroupExposureItems;
            GroupExposureGrid.DataBind();

            if (legalEntityGroupExposure.GroupExposureItems.Count > 0)
            {
                // add the totals to the grid
                // CurrentBalance
                GroupExposureGrid.FooterRow.Cells[(int)GroupExposureGridColumns.CurrentBalance].Text = legalEntityGroupExposure.TotalCurrentBalance.ToString(SAHL.Common.Constants.CurrencyFormat);
                GroupExposureGrid.FooterRow.Cells[(int)GroupExposureGridColumns.CurrentBalance].HorizontalAlign = HorizontalAlign.Right;
                // ArrearBalance
                GroupExposureGrid.FooterRow.Cells[(int)GroupExposureGridColumns.ArrearBalance].Text = legalEntityGroupExposure.TotalArrearBalance.ToString(SAHL.Common.Constants.CurrencyFormat);
                GroupExposureGrid.FooterRow.Cells[(int)GroupExposureGridColumns.ArrearBalance].HorizontalAlign = HorizontalAlign.Right;
                // LoanAgreementAmount
                GroupExposureGrid.FooterRow.Cells[(int)GroupExposureGridColumns.LoanAgreementAmount].Text = legalEntityGroupExposure.TotalLoanAgreementAmount.ToString(SAHL.Common.Constants.CurrencyFormat);
                GroupExposureGrid.FooterRow.Cells[(int)GroupExposureGridColumns.LoanAgreementAmount].HorizontalAlign = HorizontalAlign.Right;
                // LatestValuationAmount
                GroupExposureGrid.FooterRow.Cells[(int)GroupExposureGridColumns.LatestValuationAmount].Text = legalEntityGroupExposure.TotalLatestValuation.ToString(SAHL.Common.Constants.CurrencyFormat);
                GroupExposureGrid.FooterRow.Cells[(int)GroupExposureGridColumns.LatestValuationAmount].HorizontalAlign = HorizontalAlign.Right;
                // InstalmentAmount
                GroupExposureGrid.FooterRow.Cells[(int)GroupExposureGridColumns.InstalmentAmount].Text = legalEntityGroupExposure.TotalLoanInstalment.ToString(SAHL.Common.Constants.CurrencyFormat);
                GroupExposureGrid.FooterRow.Cells[(int)GroupExposureGridColumns.InstalmentAmount].HorizontalAlign = HorizontalAlign.Right;
            }
        }
        

        /// <summary>
        /// Implements <see cref="IGroupExposure.BindDebtCounsellingGrid"/>
        /// </summary>
        /// <param name="combinedDetailList"></param>
        public void BindDebtCounsellingGrid(List<IDetail> combinedDetailList)
        {
            DebtCounsellingGrid.Columns.Clear();
            DebtCounsellingGrid.AddGridBoundColumn("", "Account", Unit.Percentage(5), HorizontalAlign.Left, true);
            DebtCounsellingGrid.AddGridBoundColumn("", "Messages", Unit.Percentage(95), HorizontalAlign.Left, true);
            DebtCounsellingGrid.DataSource = combinedDetailList;
            DebtCounsellingGrid.DataBind();
        }

        #endregion
    }
}




