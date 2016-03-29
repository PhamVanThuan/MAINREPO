using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web.Services;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxGridView;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DomainMessages;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Utils;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Web.UI.Events;
using SAHL.Web.Views.DebtCounselling.Interfaces;

namespace SAHL.Web.Views.DebtCounselling
{
    public partial class ProposalDetails : SAHLCommonBaseView, IProposalDetails
    {
        private bool _readOnlyMode;
        private IDebtCounsellingRepository _debtCounsellingRepo;
        private ILookupRepository _lookupRepo;
        private double _interestRate;
        private IProposal _proposal;

        #region Properties

        /// <summary>
        /// Controls visibility of Cancel Button
        /// </summary>
        public bool ShowCancelButton
        {
            set
            {
                btnCancel.Visible = value;
            }
        }

        /// <summary>
        /// Controls visibility of Add Button
        /// </summary>
        public bool ShowAddButton
        {
            set
            {
                btnAdd.Visible = value;
            }
        }

        /// <summary>
        /// Controls visibility of Remove Button
        /// </summary>
        public bool ShowRemoveButton
        {
            set
            {
                btnRemove.Visible = value;
            }
        }

        /// <summary>
        /// Controls visibility of Save Button
        /// </summary>
        public bool ShowSaveButton
        {
            set
            {
                if (value)
                    btnSave.Attributes.Add("style", "display: inline");
                else
                    btnSave.Attributes.Add("style", "display: none");
            }
        }

        /// <summary>
        ///
        /// </summary>
        public bool ReadOnlyMode
        {
            set
            {
                txtReadOnly.Text = value.ToString();
                _readOnlyMode = value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public string SelectedMarketRate
        {
            get
            {
                return ddlMarketRate.SelectedItem.Value;
            }
        }

        /// <summary>
        /// Proposal Reason
        /// </summary>
        public string CounterProposalNote
        {
            get { return txtCounterReason.Text.Trim(); }
            set { txtCounterReason.Text = value != null ? value.Trim() : value; }
        }

        private ProposalTypeDisplays _showProposals;

        public ProposalTypeDisplays ShowProposals
        {
            get
            {
                if (_showProposals == null)
                    return ProposalTypeDisplays.All;

                return _showProposals;
            }
            set
            {
                _showProposals = value;
            }
        }

        public DateTime? GetReviewDate
        {
            get { return dteReviewDate.Date; }
            set { dteReviewDate.Date = value; }
        }

        #endregion Properties

        public event EventHandler OnAddButtonClicked;

        public event KeyChangedEventHandler OnRemoveButtonClicked;

        public event EventHandler OnCancelButtonClicked;

        public event EventHandler OnDeleteButtonClicked;

        public event EventHandler OnSaveButtonClicked;

        public event EventHandler OnLifeInclSelectedIndexChanged;

        public event EventHandler OnHOCInclSelectedIndexChanged;

        protected override void OnInit(EventArgs e)
        {
            if (_debtCounsellingRepo == null)
                _debtCounsellingRepo = RepositoryFactory.GetRepository<IDebtCounsellingRepository>();
            if (_lookupRepo == null)
                _lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();

            base.OnInit(e);

            if (!this.ShouldRunPage)
                return;

            DevExpress.Web.ASPxClasses.ASPxWebControl.RegisterBaseScript(this);
            this.proposalGraph.ClientInstanceName = "proposalChart";
            this.secondaryProposalGraph.ClientInstanceName = "secondaryProposalChart";
            ProposalType.Value = ShowProposals.ToString();

            //set default values
            txtAdditionalAmount.Amount = 0;
            txtAmount.Amount = 0;
            txtInstalPercentDisplay.Amount = 0;
            txtInstalmentPercentage.Text = "0";
            txtAnnualEscalation.Amount = 0;
        }

        protected override void OnLoad(EventArgs e)
        {
            gvProposalItems.HtmlDataCellPrepared += new ASPxGridViewTableDataCellEventHandler(gvProposalItems_HtmlDataCellPrepared);
            base.OnLoad(e);

            txtInstalPercentDisplay.Attributes["onkeyup"] = "setPercentAndCalc();" + txtInstalPercentDisplay.Attributes["onkeyup"];
            txtInterestRate.Attributes["onkeyup"] = "CalculateInstalment('percent');" + txtInterestRate.Attributes["onkeyup"];
            txtInstalment.Attributes["onkeyup"] = "CalculateInstalment('amount');" + txtInstalment.Attributes["onkeyup"];

            //Set the View Display for the Labels for the Graphs
            //Set the style properties on the gridDiv
            //this is a workaround for IE8 & Dev XPress Grid
            //the show horizontal scrollbar has funny side effects with IE8 & DevXPress
            if (ShowProposals == ProposalTypeDisplays.All || ShowProposals == ProposalTypeDisplays.Proposal)
            {
                gridDiv.Attributes.Add("style", "overflow: scroll; height: 220px");
                lblProposalGraph.Text = "Proposal";
                lblSecondaryProposalGraph.Text = "Counter Proposal";
            }
            else
            {
                gridDiv.Attributes.Add("style", "height: 120px");
                lblProposalGraph.Text = "Counter Proposal";
                lblSecondaryProposalGraph.Text = "Proposal";
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!ShouldRunPage)
                return;

            tblProposalDetails.Visible = !_readOnlyMode;
            ddlHOCIncl.Visible = !_readOnlyMode;
            ddlLifeIncl.Visible = !_readOnlyMode;
            chkServiceFee.Enabled = !_readOnlyMode;

            lblHOCIncl.Visible = _readOnlyMode;
            lblLifeIncl.Visible = _readOnlyMode;

            txtCounterReason.ReadOnly = _readOnlyMode;

            btnRemove.Attributes.Add("onclick", "return confirm('Are you sure you want to remove the selected proposal item ?');");

            //hide the additional counter proposal specific stuff
            switch (ShowProposals)
            {
                case ProposalTypeDisplays.CounterProposal:
                    // Hide Proposal Specific Fields
                    AmountRow.Visible = false;
                    AdditionalAmountRow.Visible = false;
                    MarketRateRow.Visible = false;
                    trCounterProposal.Visible = true;
                    //txtInstalment.ReadOnly = true;
                    break;

                default:

                    //secondaryProposalChartContainer.Visible = false;

                    // Hide counter proposal fields
                    InstalmentPercentageRow.Visible = false;
                    InstalmentRow.Visible = false;
                    AnnualEscalationRow.Visible = false;
                    apAccountSummary.Visible = false;
                    apReason.Visible = false;
                    apDetailGrid.HeaderContainer.Visible = false;
                    //remove the style="display:none" attribute,
                    //otherwise the content is not visible
                    apDetailGrid.ContentContainer.Attributes.Remove("style");
                    break;
            }
        }

        /// <summary>
        /// Render ProposalGraph
        /// </summary>
        /// <param name="proposalKey"></param>
        public void RenderProposalGraph(int proposalKey)
        {
            try
            {
                proposalGraph.Render(proposalKey);
            }
            catch (SAHL.Common.Exceptions.DomainValidationException ex)
            {
                this.Messages.Add(new Error(ex.Message, ex.Message));
            }
        }

        public void RenderAmortisationSchedule(int proposalKey)
        {
            lblClosingBalance.Text = "-";
            pnlAmortisationSchedule.Render(proposalKey, true);
            if (gvProposalItems.DataSource != null && pnlAmortisationSchedule != null &&
                pnlAmortisationSchedule.AmortisationScheduleDataTable != null &&
                pnlAmortisationSchedule.AmortisationScheduleDataTable.Rows.Count > 0)
            {
                var lastAmortisationScheduleRow = pnlAmortisationSchedule.AmortisationScheduleDataTable.Last();
                if (lastAmortisationScheduleRow != null)
                {
                    lblClosingBalance.Text = lastAmortisationScheduleRow.Closing.ToString(SAHL.Common.Constants.CurrencyFormat);
                }
            }
        }

        /// <summary>
        /// Render Counter ProposalGraph
        /// </summary>
        /// <param name="proposalKey"></param>
        public void RenderCounterProposalGraph(int proposalKey)
        {
            //Dont even show the graph
            secondaryProposalChartContainer.Visible = true;
            try
            {
                secondaryProposalGraph.Render(proposalKey);
            }
            catch (SAHL.Common.Exceptions.DomainValidationException ex)
            {
                this.Messages.Add(new Error(ex.Message, ex.Message));
            }
        }

        protected void gvProposalItems_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e != null)
            {
                var proposalItem = _debtCounsellingRepo.GetProposalItemByKey(Convert.ToInt32(e.GetValue("Key")));
                switch (e.DataColumn.Caption
)
                {
                    case "Market Rate":
                        e.Cell.Text = proposalItem.MarketRate != null ? proposalItem.MarketRate.Description : "Fixed";
                        break;

                    case "User":
                        e.Cell.Text = proposalItem.ADUser.ADUserName.Replace("SAHL\\", string.Empty);
                        break;

                    case "Rate":
                    case "Interest Rate":
                        e.Cell.Text = (proposalItem.InterestRate * 100).ToString() + " %";
                        break;

                    case "Instalment %":
                        e.Cell.Text = proposalItem.InstalmentPercent.Value.ToString("0.00 %");
                        break;

                    case "Escalation":
                        e.Cell.Text = (proposalItem.AnnualEscalation * 100).ToString() + " %";
                        break;
                }
            }
        }

        public void BindMarketRates(IDictionary<int, string> marketRates)
        {
            ddlMarketRate.DataSource = marketRates;
            ddlMarketRate.DataBind();

            if (marketRates.Count == 1)
                ddlMarketRate.SelectedIndex = 1;
        }

        public void BindHOCAndLife(IDictionary<int, string> inclusiveExclusive)
        {
            ddlHOCIncl.DataSource = inclusiveExclusive;
            ddlHOCIncl.DataBind();

            ddlLifeIncl.DataSource = inclusiveExclusive;
            ddlLifeIncl.DataBind();
        }

        public void BindProposalHeader(IProposal proposal)
        {
            _proposal = proposal;

            double openingBalance = 0;
            if (_proposal.DebtCounselling != null &&
                _proposal.DebtCounselling.Account != null &&
                _proposal.ProposalItems != null &&
                _proposal.ProposalItems.Count > 0)
            {
                IMortgageLoanAccount mortgageLoanAccount = proposal.DebtCounselling.Account as IMortgageLoanAccount;
                openingBalance = mortgageLoanAccount.GetAccountBalanceByDate(_proposal.ProposalItems[0].StartDate);
            }
            lblOpeningBalance.Text = openingBalance.ToString(SAHL.Common.Constants.CurrencyFormat);

            lblProposalStatus.Text = proposal.ProposalStatus != null ? proposal.ProposalStatus.Description : "";
            //lblProposalRemainingTerm.Text = CalculateProposalRemainingTerm(proposal).ToString();

            chkServiceFee.Checked = proposal.MonthlyServiceFeeInclusive;
            if (_readOnlyMode)
            {
                lblHOCIncl.Text = proposal.HOCInclusive.Value == true ? SAHL.Common.Constants.Proposals.HOCLifeInclusiveDesc : SAHL.Common.Constants.Proposals.HOCLifeExclusiveDesc;
                lblLifeIncl.Text = proposal.LifeInclusive.Value == true ? SAHL.Common.Constants.Proposals.HOCLifeInclusiveDesc : SAHL.Common.Constants.Proposals.HOCLifeExclusiveDesc;
                dteReviewDate.Text = proposal.ReviewDate.HasValue == true ? proposal.ReviewDate.Value.ToString(SAHL.Common.Constants.DateFormat) : "-";
                dteReviewDate.Enabled = !_readOnlyMode;
            }
            else
            {
                if (proposal.HOCInclusive.HasValue)
                    ddlHOCIncl.SelectedValue = proposal.HOCInclusive.Value == true ? SAHL.Common.Constants.Proposals.HOCLifeInclusiveKey.ToString() : SAHL.Common.Constants.Proposals.HOCLifeExclusiveKey.ToString();

                if (proposal.LifeInclusive.HasValue)
                    ddlLifeIncl.SelectedValue = proposal.LifeInclusive.Value == true ? SAHL.Common.Constants.Proposals.HOCLifeInclusiveKey.ToString() : SAHL.Common.Constants.Proposals.HOCLifeExclusiveKey.ToString();

                if (proposal.ReviewDate.HasValue)
                    dteReviewDate.Date = proposal.ReviewDate.Value;
            }
            DateTime dtISD = DateTime.Now.AddYears(-100);
            int stIPeriod = 0;
            if (proposal.ProposalItems.Count > 0)
            {
                foreach (IProposalItem pI in proposal.ProposalItems)
                {
                    if (pI.EndDate > dtISD)
                    {
                        dtISD = pI.EndDate.AddDays(1);
                        stIPeriod = pI.EndPeriod;
                    }
                }
                //set the Proposal start date for the view
                dteStartDate.Date = dtISD;
            }
            //begin at the next period
            txtStartPeriod.Text = (stIPeriod + 1).ToString();
            if (stIPeriod != 0)
                dteStartDate.Enabled = false;
            else
            {
                //dteStartDate.Date = null;
                dteStartDate.Enabled = true;
            }
        }

        public void PopulateProposalFromScreen(IProposal proposal)
        {
            proposal.MonthlyServiceFeeInclusive = chkServiceFee.Checked;

            if (ddlHOCIncl.SelectedItem.Value != SAHL.Common.Constants.DefaultDropDownItem)
                proposal.HOCInclusive = ddlHOCIncl.SelectedItem.Value == SAHL.Common.Constants.Proposals.HOCLifeInclusiveKey.ToString() ? true : false;
            else
                proposal.HOCInclusive = null;

            if (ddlHOCIncl.SelectedItem.Value != SAHL.Common.Constants.DefaultDropDownItem)
                proposal.HOCInclusive = ddlHOCIncl.SelectedItem.Value == SAHL.Common.Constants.Proposals.HOCLifeInclusiveKey.ToString() ? true : false;
            else
                proposal.HOCInclusive = null;

            if (ddlLifeIncl.SelectedItem.Value != SAHL.Common.Constants.DefaultDropDownItem)
                proposal.LifeInclusive = ddlLifeIncl.SelectedItem.Value == SAHL.Common.Constants.Proposals.HOCLifeInclusiveKey.ToString() ? true : false;
            else
                proposal.LifeInclusive = null;

            switch (ShowProposals)
            {
                case ProposalTypeDisplays.CounterProposal:
                    {
                        if (ShowProposals == ProposalTypeDisplays.CounterProposal && dteReviewDate.Date.HasValue)
                            proposal.ReviewDate = dteReviewDate.Date.Value;

                        break;
                    }
                default:
                    break;
            }
        }

        public void PopulateProposalItemFromScreen(IProposalItem proposalItem, bool update)
        {
            //limited values are allowed to be update
            if (!update)
            {
                if (dteStartDate.Date.HasValue)
                    proposalItem.StartDate = dteStartDate.Date.Value;

                if (dteEndDate.Date.HasValue)
                    proposalItem.EndDate = dteEndDate.Date.Value;

                if (!String.IsNullOrEmpty(txtStartPeriod.Text))
                    proposalItem.StartPeriod = Convert.ToInt16(txtStartPeriod.Text);
                if (!String.IsNullOrEmpty(txtEndPeriod.Text))
                    proposalItem.EndPeriod = Convert.ToInt16(txtEndPeriod.Text);
            }

            if (txtInterestRate.Amount.HasValue)
            {
                proposalItem.InterestRate = txtInterestRate.Amount.Value / 100;
            }

            switch (ShowProposals)
            {
                case ProposalTypeDisplays.CounterProposal:
                    {
                        if (txtInstalment.Amount.HasValue)
                            proposalItem.Amount = txtInstalment.Amount.Value;

                        if (!String.IsNullOrEmpty(txtInstalmentPercentage.Text))
                            proposalItem.InstalmentPercent = Convert.ToDouble(txtInstalmentPercentage.Text) / 100;

                        if (txtAnnualEscalation.Amount.HasValue)
                            proposalItem.AnnualEscalation = txtAnnualEscalation.Amount.Value / 100;

                        break;
                    }
                default:
                    {
                        if (txtAmount.Amount.HasValue)
                            proposalItem.Amount = txtAmount.Amount.Value;

                        if (!update && ddlMarketRate.SelectedItem.Value != SAHL.Common.Constants.DefaultDropDownItem && ddlMarketRate.SelectedItem.Value != SAHL.Common.Constants.Proposals.FixedMarketRateKey.ToString())
                            proposalItem.MarketRate = _lookupRepo.MarketRates.ObjectDictionary[ddlMarketRate.SelectedValue];

                        if (txtAdditionalAmount.Amount.HasValue)
                            proposalItem.AdditionalAmount = txtAdditionalAmount.Amount.Value;

                        break;
                    }
            }
        }

        public void ProposalGridUnselectAll()
        {
            gvProposalItems.Selection.UnselectAll();
        }

        public void BindAccountSummary(IAccount acc, double hocMonthlyPremium, double lifePolicyMonthlyPremium, double lifeBalance)
        {
            IAccountInstallmentSummary ais = acc.InstallmentSummary;
            IMortgageLoan ml = acc.FinancialServices.FirstOrDefault(x => x.FinancialServiceType.Key == (int)FinancialServiceTypes.VariableLoan) as IMortgageLoan;

            double LTV = 0d;

            IMortgageLoanAccount mortgageLoanAccount = acc as IMortgageLoanAccount;
            hdnAdminFee.Value = ais.MonthlyServiceFee.ToString();
            lblProposalFees.Text = ais.MonthlyServiceFee.ToString(SAHL.Common.Constants.CurrencyFormat);
            lblProposalHOC.Text = hocMonthlyPremium.ToString(SAHL.Common.Constants.CurrencyFormat);
            lblProposalLife.Text = lifePolicyMonthlyPremium.ToString(SAHL.Common.Constants.CurrencyFormat);

            LTV = Convert.ToDouble(ml.CurrentBalance) / Convert.ToDouble(ml.Property.LatestCompleteValuation.ValuationAmount);
            lblSPV.Text = ml.Account.SPV.Description;
            lblCurrentBalance.Text = ais.CurrentBalance.ToString(SAHL.Common.Constants.CurrencyFormat);
            lblInstalmentAmount.Text = ais.TotalLoanInstallment.ToString(SAHL.Common.Constants.CurrencyFormat);
            lblArrearBalance.Text = ais.TotalArrearsBalance.ToString(SAHL.Common.Constants.CurrencyFormat);

            lblMonthsInArrear.Text = ais.MonthsInArrears.ToString();
            lblDODay.Text = ml.FinancialServiceBankAccounts[0].DebitOrderDay.ToString();
            lblLTV.Text = LTV.ToString(SAHL.Common.Constants.RateFormat);
            lblLAA.Text = ml.SumBondLoanAgreementAmounts().ToString(SAHL.Common.Constants.CurrencyFormat);

            lblTotalBondsRegistered.Text = ml.SumBondRegistrationAmounts().ToString(SAHL.Common.Constants.CurrencyFormat);
            lblFixedDOAmount.Text = acc.FixedPayment.ToString(SAHL.Common.Constants.CurrencyFormat);
            lblAccountOpenDate.Text = acc.OpenDate.Value.ToString(SAHL.Common.Constants.DateFormat);

            _interestRate = ml.InterestRate * 100;
            if (acc.Product.Key == (int)Products.VariFixLoan)
            {
                //Fixed leg interest rate
                double interestRate = mortgageLoanAccount.SecuredMortgageLoan.InterestRate;

                //Variable leg interest rate
                IAccountVariFixLoan varifixLoanAccount = acc as IAccountVariFixLoan;

                //If the Variable Leg Interest rate is higher than the Fixed leg interest rate, use the Variable Interest Rate
                if (varifixLoanAccount.FixedSecuredMortgageLoan.InterestRate > interestRate)
                {
                    _interestRate = varifixLoanAccount.FixedSecuredMortgageLoan.InterestRate;
                }
                _interestRate = interestRate * 100;
                hdnCurrentBalance.Value = (varifixLoanAccount.FixedSecuredMortgageLoan.CurrentBalance + (ml.CurrentBalance - lifeBalance)).ToString();
            }
            else
            {
                hdnCurrentBalance.Value += (ml.CurrentBalance - lifeBalance).ToString();
            }

            if (txtInterestRate.Amount == null)
            {
                if (_proposal.ProposalItems.Count > 0)
                    txtInterestRate.Amount = _proposal.ProposalItems[0].InterestRate * 100;
                else
                    txtInterestRate.Amount = _interestRate;
            }

            hdnRemainingTerm.Value = ml.RemainingInstallments.ToString();
            lblCurrentRemainingTerm.Text = ml.RemainingInstallments.ToString();
            hdnHOCIncl.Value = hocMonthlyPremium.ToString();
            hdnLifeIncl.Value = lifePolicyMonthlyPremium.ToString();
            lblTotalInstalment.Text = (ais.TotalAmountDue).ToString(SAHL.Common.Constants.CurrencyFormat);
        }

        public void SetProposalRemainingTerm(IProposal proposal)
        {
            IEventList<IProposalItem> propItems = _debtCounsellingRepo.SortProposalItems(proposal);

            if (propItems.Count > 0)
            {
                DateTime proposalStartDT = propItems[0].StartDate;
                DateTime proposalEndDT = propItems[propItems.Count - 1].EndDate;

                //int monthsElapsed = proposalStartDT.MonthDifference(proposal.DebtCounselling.Account.OpenDate.Value, 1);
                int proposalPeriod = proposalEndDT.MonthDifference(proposalStartDT, 1);

                lblProposalRemainingTerm.Text = proposalPeriod.ToString();
            }
            else
                lblProposalRemainingTerm.Text = "-";
        }

        public void BindProposalItemsGrid(IEventList<IProposalItem> proposalItems)
        {
            //use a boolean switch to show/hide column data based on the type of proposal item detail being displayed
            bool isCounterProp = (ShowProposals == ProposalTypeDisplays.CounterProposal);

            SetupGrid(isCounterProp);
            gvProposalItems.SettingsPager.PageSize = proposalItems.Count;
            gvProposalItems.DataSource = proposalItems;
            gvProposalItems.DataBind();
        }

        private void SetupGrid(bool isCounterProp)
        {
            //clear columns
            gvProposalItems.Columns.Clear();
            gvProposalItems.KeyFieldName = "Key";
            gvProposalItems.SettingsText.Title = (isCounterProp ? "Counter " : "") + "Proposal Details";
            gvProposalItems.SettingsText.EmptyDataRow = "No Details";

            // key column
            AddGridColumn("Key", "", Unit.Pixel(5), GridFormatType.GridNumber, "", HorizontalAlign.Left, GridViewColumnFixedStyle.None, false);

            //Because of the additional detail displayed for Counter Proposals, and less Items anticipated, the grid can be smaller
            if (isCounterProp)
            {
                gvProposalItems.Settings.ShowVerticalScrollBar = true;
                gvProposalItems.Settings.VerticalScrollableHeight = 0;
                AddGridColumn("StartDate", "Start Date", Unit.Pixel(85), GridFormatType.GridDateTime, "dd/MM/yyyy", HorizontalAlign.Left, GridViewColumnFixedStyle.None, true);
                AddGridColumn("EndDate", "End Date", Unit.Pixel(85), GridFormatType.GridDateTime, "dd/MM/yyyy", HorizontalAlign.Left, GridViewColumnFixedStyle.None, true);
                AddGridColumn("StartPeriod", "Start", Unit.Pixel(30), GridFormatType.GridString, null, HorizontalAlign.Right, GridViewColumnFixedStyle.None, true);
                AddGridColumn("EndPeriod", "End", Unit.Pixel(30), GridFormatType.GridString, null, HorizontalAlign.Right, GridViewColumnFixedStyle.None, true);
                AddGridColumn("", "Rate", Unit.Pixel(85), GridFormatType.GridNumber, SAHL.Common.Constants.RateFormat, HorizontalAlign.Right, GridViewColumnFixedStyle.None, true);
                AddGridColumn("", "Instalment %", Unit.Pixel(85), GridFormatType.GridNumber, SAHL.Common.Constants.RateFormat, HorizontalAlign.Right, GridViewColumnFixedStyle.None, true);
                AddGridColumn("Amount", "Instalment", Unit.Pixel(90), GridFormatType.GridCurrency, null, HorizontalAlign.Right, GridViewColumnFixedStyle.None, true);
                AddGridColumn("", "Escalation", Unit.Pixel(100), GridFormatType.GridNumber, SAHL.Common.Constants.RateFormat, HorizontalAlign.Right, GridViewColumnFixedStyle.None, true);
                AddGridColumn("CreateDate", "Date Captured", Unit.Pixel(85), GridFormatType.GridDateTime, "dd/MM/yyyy", HorizontalAlign.Left, GridViewColumnFixedStyle.None, true);
                AddGridColumn("", "User", Unit.Pixel(90), GridFormatType.GridString, "", HorizontalAlign.Left, GridViewColumnFixedStyle.None, true);
            }
            else
            {
                gvProposalItems.Settings.ShowVerticalScrollBar = false;
                AddGridColumn("StartDate", "Start Date", Unit.Pixel(85), GridFormatType.GridDateTime, "dd/MM/yyyy", HorizontalAlign.Left, GridViewColumnFixedStyle.None, true);
                AddGridColumn("EndDate", "End Date", Unit.Pixel(85), GridFormatType.GridDateTime, "dd/MM/yyyy", HorizontalAlign.Left, GridViewColumnFixedStyle.None, true);
                AddGridColumn("StartPeriod", "Start", Unit.Pixel(30), GridFormatType.GridString, null, HorizontalAlign.Right, GridViewColumnFixedStyle.None, true);
                AddGridColumn("EndPeriod", "End", Unit.Pixel(30), GridFormatType.GridString, null, HorizontalAlign.Right, GridViewColumnFixedStyle.None, true);
                AddGridColumn("", "Market Rate", Unit.Pixel(85), GridFormatType.GridString, null, HorizontalAlign.Left, GridViewColumnFixedStyle.None, true);
                AddGridColumn("", "Interest Rate", Unit.Pixel(85), GridFormatType.GridNumber, SAHL.Common.Constants.RateFormat, HorizontalAlign.Right, GridViewColumnFixedStyle.None, true);
                AddGridColumn("Amount", "Amount", Unit.Pixel(90), GridFormatType.GridCurrency, null, HorizontalAlign.Right, GridViewColumnFixedStyle.None, true);
                AddGridColumn("AdditionalAmount", "Additional Amount", Unit.Pixel(90), GridFormatType.GridCurrency, null, HorizontalAlign.Right, GridViewColumnFixedStyle.None, true);
                AddGridColumn("", "Instalment %", Unit.Pixel(85), GridFormatType.GridNumber, SAHL.Common.Constants.RateFormat, HorizontalAlign.Right, GridViewColumnFixedStyle.None, true);
                AddGridColumn("CreateDate", "Date Captured", Unit.Pixel(85), GridFormatType.GridDateTime, "dd/MM/yyyy", HorizontalAlign.Left, GridViewColumnFixedStyle.None, true);
                AddGridColumn("", "User", Unit.Pixel(90), GridFormatType.GridString, "", HorizontalAlign.Left, GridViewColumnFixedStyle.None, true);
            }

            gvProposalItems.SortBy(gvProposalItems.Columns["StartDate"], DevExpress.Data.ColumnSortOrder.Ascending);
        }

        private void AddGridColumn(string fieldName, string caption, Unit width, GridFormatType formatType, string format, HorizontalAlign align, GridViewColumnFixedStyle fixedStyle, bool visible)
        {
            DXGridViewFormattedTextColumn col = new DXGridViewFormattedTextColumn();
            col.FieldName = fieldName;
            col.Caption = caption;

            col.Width = width;

            col.FixedStyle = fixedStyle;
            col.Format = formatType;
            if (!String.IsNullOrEmpty(format))
                col.FormatString = format;
            col.Visible = visible;
            col.CellStyle.HorizontalAlign = align;
            gvProposalItems.Columns.Add(col);
        }

        public void ResetInputFields()
        {
            //dteStartDate.Date = null;
            dteEndDate.Date = null;
            //txtStartPeriod.Text = String.Empty;
            txtEndPeriod.Text = String.Empty;
            if (ddlMarketRate.Items.Count > 2)
                ddlMarketRate.SelectedValue = SAHL.Common.Constants.DefaultDropDownItem;

            if (_proposal.ProposalItems.Count > 0)
                txtInterestRate.Amount = _proposal.ProposalItems[0].InterestRate * 100;
            else
                txtInterestRate.Amount = _interestRate;

            txtAmount.Amount = 0;
            txtAdditionalAmount.Amount = 0;
            txtInstalPercentDisplay.Amount = 0;
            txtInstalmentPercentage.Text = "0";
            txtInstalment.Text = null;
            txtAnnualEscalation.Amount = 0;
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (OnAddButtonClicked != null)
                OnAddButtonClicked(sender, e);
        }

        protected void btnRemove_Click(object sender, EventArgs e)
        {
            // get the selected ProposalItemKey
            KeyChangedEventArgs keyChangedEventArgs = new KeyChangedEventArgs(gvProposalItems.SelectedKeyValue);

            if (OnRemoveButtonClicked != null)
                OnRemoveButtonClicked(sender, keyChangedEventArgs);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (OnCancelButtonClicked != null)
                OnCancelButtonClicked(sender, e);
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (OnDeleteButtonClicked != null)
                OnDeleteButtonClicked(sender, e);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (OnSaveButtonClicked != null)
                OnSaveButtonClicked(sender, e);
        }

        protected void ddlLifeIncl_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (ddlHOCIncl.SelectedValue == SAHLDropDownList.PleaseSelectValue)
            //{
            //    Messages.Add(new Error("Please Select whether Life is Inclusive or Exclusive", "Please Select whether Life is Inclusive or Exclusive"));
            //    return;
            //}
            //if (OnLifeInclSelectedIndexChanged != null)
            //    OnLifeInclSelectedIndexChanged(sender, e);
        }

        protected void ddlHOCIncl_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (ddlHOCIncl.SelectedValue == SAHLDropDownList.PleaseSelectValue)
            //{
            //    Messages.Add(new Error("Please Select whether HOC is Inclusive or Exclusive", "Please Select whether HOC is Inclusive or Exclusive"));
            //    return;
            //}
            //if (OnHOCInclSelectedIndexChanged != null)
            //    OnHOCInclSelectedIndexChanged(sender, e);
        }

        /// <summary>
        /// Calculate the Monthly Instalment
        /// </summary>
        /// <param name="loanAmount"></param>
        /// <param name="interestRate"></param>
        /// <param name="term"></param>
        [WebMethod]
        public static double CalculateMonthlyInstalment(double loanAmount, double interestRate, int term)
        {
            return SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculateInstallment(loanAmount, interestRate, term, false);
        }

        public int SelectedItemKey
        {
            get
            {
                Int32 piKey = 0;

                if (Int32.TryParse(txtSaveKey.Text, out piKey))
                    return piKey;

                return 0;
            }
        }

        protected void OnSaveToPDFClick(object sender, EventArgs e)
        {
            ISecurityService securityService = ServiceFactory.GetService<ISecurityService>();
            WindowsImpersonationContext windowsImpersonationContext = securityService.BeginImpersonation();

            if (_proposal.Key <= 0)
                return;

            pnlAmortisationSchedule.Render(_proposal.Key, false);
            var pdfFilePath = pnlAmortisationSchedule.SaveToPDFAndReturnFilePath();
            Response.Clear();
            Response.ClearHeaders();
            Response.ClearContent();

            Response.ContentType = "application/pdf";
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + pdfFilePath);
            Response.WriteFile(pdfFilePath);
            Response.Flush();
            Response.End();

            securityService.EndImpersonation(windowsImpersonationContext);
        }
    }
}