using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Controls;
using SAHL.Web.Views.FurtherLending.Interfaces;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace SAHL.Web.Views.FurtherLending
{
    public partial class PreCheck : SAHLCommonBaseView, ICalculatorPreCheck
    {
        private IAccountRepository _accRepo;

        private IAccountRepository AccRepo
        {
            get
            {
                if (_accRepo == null)
                    _accRepo = RepositoryFactory.GetRepository<IAccountRepository>();

                return _accRepo;
            }
        }

        #region Interface

        private bool _nextButtonVisible;

        public event EventHandler OnNextButtonClicked;

        public event EventHandler OnCancelButtonClicked;

        public void BindDisplay(IAccount Account, IList<IFinancialAdjustment> FinancialAdjustments)
        {
            #region GetPaymentType

            foreach (IFinancialService fs in Account.FinancialServices)
            {
                switch (fs.Account.AccountStatus.Key)
                {
                    case (int)SAHL.Common.Globals.AccountStatuses.Open:
                        foreach (IFinancialServiceBankAccount fa in fs.FinancialServiceBankAccounts)
                        {
                            if (fa.GeneralStatus.Key == (int)SAHL.Common.Globals.GeneralStatuses.Active)
                            {
                                lblPaymentMethod.Text = fa.FinancialServicePaymentType.Description;
                            }
                        }
                        break;

                    case (int)SAHL.Common.Globals.AccountStatuses.Locked:
                    case (int)SAHL.Common.Globals.AccountStatuses.Dormant:
                    default:
                        break;
                }
            }

            #endregion GetPaymentType

            BindConditionsAndDetail(Account);
            BindFinancialAdjustmentGrid(FinancialAdjustments);
            lblProduct.Text = Account.Product.Description;
        }

        public bool NextButtonVisible
        {
            set { _nextButtonVisible = value; }
        }

        #endregion Interface

        #region Page

        protected void Page_Load(object sender, EventArgs e)
        {
            btnNext.Enabled = _nextButtonVisible;
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            if (OnNextButtonClicked != null)
                OnNextButtonClicked(null, null);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (OnCancelButtonClicked != null)
                OnCancelButtonClicked(null, null);
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

        protected void BindConditionsAndDetail(IAccount Account)
        {
            //Detail
            IReadOnlyEventList<IDetail> listDetail = AccRepo.GetAccountDetailForFurtherLending(Account.Key);
            List<string> detailList = new List<string>();

            if (listDetail != null && listDetail.Count > 0)
            {
                foreach (IDetail det in listDetail)
                {
                    detailList.Add(det.DetailType.Description + (String.IsNullOrEmpty(det.Description) ? "" : " - " + det.Description));
                }
            }

            gvDetail.Columns.Clear();
            gvDetail.AddGridBoundColumn("!", "Details", Unit.Percentage(100), HorizontalAlign.Left, true);
            gvDetail.DataSource = detailList;
            gvDetail.DataBind();

            //Conditions
            IConditionsRepository conditionsRepo = RepositoryFactory.GetRepository<IConditionsRepository>();
            List<string> conditionsList = conditionsRepo.GetLastDisbursedApplicationConditions(Account.Key);
            gvConditions.Columns.Clear();
            gvConditions.AddGridBoundColumn("!", "Conditions", Unit.Percentage(100), HorizontalAlign.Left, true);
            gvConditions.DataSource = conditionsList;
            gvConditions.DataBind();
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

        protected void FinancialAdjustmentGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                // Get the FinancialAdjustment Row
                IFinancialAdjustment financialAdjustment = e.Row.DataItem as IFinancialAdjustment;

                e.Row.Cells[(int)FinancialAdjustmentGridColumnPositions.AccountKey].Text = financialAdjustment.FinancialService.Account.Key.ToString();
                e.Row.Cells[(int)FinancialAdjustmentGridColumnPositions.FinancialAdjustmentSource].Text = financialAdjustment.FinancialAdjustmentSource.Description;
                e.Row.Cells[(int)FinancialAdjustmentGridColumnPositions.FinancialAdjustmentType].Text = financialAdjustment.FinancialAdjustmentType.Description;
                e.Row.Cells[(int)FinancialAdjustmentGridColumnPositions.FromDate].Text = financialAdjustment.FromDate.HasValue ? financialAdjustment.FromDate.Value.ToString(SAHL.Common.Constants.DateFormat) : "";
                e.Row.Cells[(int)FinancialAdjustmentGridColumnPositions.Term].Text = financialAdjustment.Term.ToString();
                e.Row.Cells[(int)FinancialAdjustmentGridColumnPositions.Value].Text = financialAdjustment.ToString();
                e.Row.Cells[(int)FinancialAdjustmentGridColumnPositions.Status].Text = financialAdjustment.FinancialAdjustmentStatus.Description;

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

        #endregion Page
}