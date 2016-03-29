using System;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.CacheData;
using SAHL.Common.DomainMessages;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;
using SAHL.Web.Views.DebtCounselling.Interfaces;

namespace SAHL.Web.Views.DebtCounselling.Presenters
{
    /// <summary>
    /// ProposalDetails Maintenance
    /// </summary>
    public class ProposalDetailsMaintenanceBase : ProposalDetailsBase
    {
        protected IProposalItem _proposalItem;
        private int _debtCounsellingKey;

        public ProposalDetailsMaintenanceBase(IProposalDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            if (SelectedProposal == null)
            {
                if (this.GlobalCacheData.ContainsKey(ViewConstants.ProposalKey))
                    SelectedProposal = DebtCounsellingRepo.GetProposalByKey(Convert.ToInt32(this.GlobalCacheData[ViewConstants.ProposalKey])); // update
                else
                    SelectedProposal = DebtCounsellingRepo.CreateEmptyProposal(); // add
            }

            if (this.GlobalCacheData.ContainsKey(ViewConstants.DebtCounsellingKey))
                _debtCounsellingKey = Convert.ToInt32(this.GlobalCacheData[ViewConstants.DebtCounsellingKey]);
            else
                _debtCounsellingKey = -1;

            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage)
                return;

            _view.OnAddButtonClicked += new EventHandler(OnAddButtonClicked);
            _view.OnRemoveButtonClicked += new KeyChangedEventHandler(OnRemoveButtonClicked);
            _view.OnSaveButtonClicked += new EventHandler(OnSaveButtonClicked);

            _view.ShowSaveButton = false;

            _view.OnLifeInclSelectedIndexChanged += new EventHandler(OnLifeInclusiveChanged);
            _view.OnHOCInclSelectedIndexChanged += new EventHandler(OnHOCInclusiveChanged);
        }

        private void SaveCounterProposalNote()
        {
            //When the save proposal reason button was clicked,
            //Ensure that the view is a counter proposal and then ensure that the proposal reason text box contains a value
            //We don't want to unnecesarely go and create a Memo (Proposal Reason) for a proposal
            if (_view.ShowProposals == ProposalTypeDisplays.CounterProposal && !string.IsNullOrEmpty(_view.CounterProposalNote))
            {
                this.SelectedProposal.Memo.ADUser = base.CurrentADUser;
                this.SelectedProposal.Memo.GeneralStatus = LookupRepo.GeneralStatuses[GeneralStatuses.Active];
                this.SelectedProposal.Memo.GenericKey = this.SelectedProposal.Key;
                this.SelectedProposal.Memo.GenericKeyType = LookupRepo.GenericKeyType.ObjectDictionary[((int)GenericKeyTypes.Proposal).ToString()];
                this.SelectedProposal.Memo.InsertedDate = DateTime.Now;
                this.SelectedProposal.Memo.Description = _view.CounterProposalNote;

                MemoRepository.SaveMemo(this.SelectedProposal.Memo);
            }
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage)
                return;

            // setup visible buttons
            _view.ShowAddButton = true;
            _view.ShowRemoveButton = true;
            _view.ShowCancelButton = true;
        }

        private void OnHOCInclusiveChanged(object sender, EventArgs e)
        {
            // perform screen validation
            PopulateAndValidateProposal();

            if (_view.IsValid)
            {
                // save the proposal and item
                SaveProposal(SelectedProposal);

                //rebind to grid
                RebindProposal(SelectedProposal);
            }
        }

        private void OnLifeInclusiveChanged(object sender, EventArgs e)
        {
            // perform screen validation
            PopulateAndValidateProposal();

            if (_view.IsValid)
            {
                // save the proposal and item
                SaveProposal(SelectedProposal);

                //rebind to grid
                RebindProposal(SelectedProposal);
            }
        }

        protected override void OnCancelButtonClicked(object sender, EventArgs e)
        {
            PopulateAndValidateProposal();

            //Save On Exit
            if (_view.IsValid)
            {
                SaveProposal(SelectedProposal);
                base.OnCancelButtonClicked(sender, e);
            }
        }

        private void OnSaveButtonClicked(object sender, EventArgs e)
        {
            SaveProposalItem(true);
        }

        private void SaveProposalItem(bool update)
        {
            PopulateAndValidateProposal();
            PopulateAndValidateProposalItem(update);

            if (_view.IsValid)
            {
                if (!update)
                    _proposalItem.Proposal = SelectedProposal;

                _proposalItem.ADUser = base.CurrentADUser;
                _proposalItem.CreateDate = DateTime.Now;

                if (_view.ShowProposals == ProposalTypeDisplays.Proposal)
                {
                    IDebtCounselling dc = DebtCounsellingRepo.GetDebtCounsellingByKey(_debtCounsellingKey);
                    IAccount acc = dc.Account;
                    IMortgageLoan ml = (IMortgageLoan)acc.FinancialServices[0];
                    double totalLoanInstallment = GetPostDebtCounsellingMortgageLoanInstallment(_debtCounsellingKey, ml);

                    if (this.SelectedProposal.MonthlyServiceFeeInclusive)
                    {
                        IAccountInstallmentSummary ais = dc.Account.InstallmentSummary;
                        totalLoanInstallment += ais.MonthlyServiceFee;
                    }

                    // get the life and hoc premiums
                    if (this.SelectedProposal.HOCInclusive.Value == true || this.SelectedProposal.LifeInclusive.Value == true)
                    {
                        double hocMonthlyPremium, lifePolicyMonthlyPremium, lifeBalance;

                        GetLifeAndHOCPremiums(dc.Account, out hocMonthlyPremium, out lifePolicyMonthlyPremium, out lifeBalance);

                        if (this.SelectedProposal.HOCInclusive.Value == true)
                            totalLoanInstallment += hocMonthlyPremium;

                        if (this.SelectedProposal.LifeInclusive.Value == true)
                            totalLoanInstallment += lifePolicyMonthlyPremium;
                    }

                    if (totalLoanInstallment > 0)
                        _proposalItem.InstalmentPercent = Math.Round(_proposalItem.TotalPayment / totalLoanInstallment, 3);
                    else
                        _proposalItem.InstalmentPercent = null;
                }

                if (!update)
                    SelectedProposal.ProposalItems.Add(_view.Messages, _proposalItem);

                SaveProposal(SelectedProposal);

                RebindProposal(SelectedProposal);

                _view.ResetInputFields();
            }
        }

        private void OnAddButtonClicked(object sender, EventArgs e)
        {
            SaveProposalItem(false);
        }

        private void OnRemoveButtonClicked(object sender, KeyChangedEventArgs e)
        {
            int proposalItemKey = Convert.ToInt32(e.Key);

            if (proposalItemKey <= 0)
            {
                string errorMessage = "Must select a proposal item";
                _view.Messages.Add(new Error(errorMessage, errorMessage));
            }

            if (_view.IsValid)
            {
                // get the proposal item
                IProposalItem pi = DebtCounsellingRepo.GetProposalItemByKey(proposalItemKey);

                // remove the proposal item
                SelectedProposal.ProposalItems.Remove(_view.Messages, pi);

                SaveProposal(SelectedProposal);

                // rebind to grid
                RebindProposal(SelectedProposal);
                _view.ProposalGridUnselectAll();

                _view.ResetInputFields();
            }
        }

        private void RebindProposal(IProposal proposal)
        {
            base.BindProposal(proposal, true);
        }

        private void SaveProposal(IProposal proposal)
        {
            TransactionScope txn = new TransactionScope();

            try
            {
                IDebtCounselling dc = DebtCounsellingRepo.GetDebtCounsellingByKey(_debtCounsellingKey);

                if (proposal.Key == 0)
                {
                    proposal.ProposalType = LookupRepo.ProposalTypes[(ProposalTypes)_view.ShowProposals];
                    proposal.ProposalStatus = LookupRepo.ProposalStatuses[SAHL.Common.Globals.ProposalStatuses.Draft];
                    proposal.ADUser = base.CurrentADUser;
                    proposal.CreateDate = DateTime.Now;
                    proposal.DebtCounselling = dc;
                }

                #region if HOC or Life Inc/Exc value changed, all proposal line items will need to be reworked

                IAccount acc = dc.Account;
                IMortgageLoan ml = (IMortgageLoan)acc.FinancialServices[0];
                IAccountInstallmentSummary ais = acc.InstallmentSummary;

                double lifePolicyMonthlyPremium = 0D;
                double hocMonthlyPremium = 0D;

                double totalLoanInstallment = 0D;

                if (_view.ShowProposals == ProposalTypeDisplays.Proposal)
                {
                    totalLoanInstallment = GetPostDebtCounsellingMortgageLoanInstallment(_debtCounsellingKey, ml);
                }

                if (proposal.MonthlyServiceFeeInclusive)
                    totalLoanInstallment += ais.MonthlyServiceFee;

                double lifeBalance = 0D;

                GetLifeAndHOCPremiums(acc, out hocMonthlyPremium, out lifePolicyMonthlyPremium, out lifeBalance);

                if (proposal.HOCInclusive.Value == Convert.ToBoolean(SAHL.Common.Constants.Proposals.HOCLifeInclusiveKey))
                    totalLoanInstallment += hocMonthlyPremium;

                if (proposal.LifeInclusive.Value == Convert.ToBoolean(SAHL.Common.Constants.Proposals.HOCLifeInclusiveKey))
                    totalLoanInstallment += lifePolicyMonthlyPremium;

                if (totalLoanInstallment == 0)
                    totalLoanInstallment = 1;

                foreach (IProposalItem pitem in proposal.ProposalItems)
                {
                    if (_view.ShowProposals == ProposalTypeDisplays.CounterProposal)
                    {
                        double payment = SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculateInstallment((ais.CurrentBalance - lifeBalance), pitem.InterestRate, ml.RemainingInstallments, false);
                        pitem.Amount = (totalLoanInstallment + payment) * (pitem.InstalmentPercent.HasValue ? pitem.InstalmentPercent.Value : 0D);
                    }
                    else
                    {
                        if (totalLoanInstallment > 0)
                            pitem.InstalmentPercent = Math.Round(pitem.TotalPayment / totalLoanInstallment, 3);
                        else
                            pitem.InstalmentPercent = null;
                    }
                }

                _view.BindProposalItemsGrid(proposal.ProposalItems);

                #endregion if HOC or Life Inc/Exc value changed, all proposal line items will need to be reworked

                DebtCounsellingRepo.SaveProposal(proposal);
                SaveCounterProposalNote();

                // add proposalkey to global cache
                GlobalCacheData.Remove(ViewConstants.ProposalKey);
                GlobalCacheData.Add(ViewConstants.ProposalKey, proposal.Key, LifeTimes);

                txn.VoteCommit();
            }
            catch (Exception)
            {
                txn.VoteRollBack();
                if (_view.IsValid)
                    throw;
            }
            finally
            {
                txn.Dispose();
            }
        }

        private void PopulateAndValidateProposal()
        {
            string errorMessage = "";

            _view.PopulateProposalFromScreen(this.SelectedProposal);

            if (SelectedProposal.HOCInclusive.HasValue == false)
            {
                errorMessage = "Must select whether HOC is "
                    + SAHL.Common.Constants.Proposals.HOCLifeInclusiveDesc
                    + " or "
                    + SAHL.Common.Constants.Proposals.HOCLifeExclusiveDesc
                    + ".";
                _view.Messages.Add(new Error(errorMessage, errorMessage));
            }

            if (SelectedProposal.LifeInclusive.HasValue == false)
            {
                errorMessage = "Must select whether Life is "
                    + SAHL.Common.Constants.Proposals.HOCLifeInclusiveDesc
                    + " or "
                    + SAHL.Common.Constants.Proposals.HOCLifeExclusiveDesc
                    + ".";
                _view.Messages.Add(new Error(errorMessage, errorMessage));
            }

            if (_view.ShowProposals == ProposalTypeDisplays.CounterProposal)
            {
                if (String.IsNullOrEmpty(_view.CounterProposalNote))
                {
                    _view.Messages.Add(new Error("A counter proposal note must be entered", "A counter proposal note must be entered"));
                }
            }
        }

        private void PopulateAndValidateProposalItem(bool update)
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(_view.CurrentPrincipal);
            IRuleService svc = ServiceFactory.GetService<IRuleService>();

            if (update && _view.SelectedItemKey > 0)
                _proposalItem = base.DebtCounsellingRepo.GetProposalItemByKey(_view.SelectedItemKey);
            else
                _proposalItem = base.DebtCounsellingRepo.CreateEmptyProposalItem();

            _view.PopulateProposalItemFromScreen(_proposalItem, update);

            string errorMessage = "";

            svc.ExecuteRule(spc.DomainMessages, "ProposalItemStartDateMandatory", _proposalItem);
            svc.ExecuteRule(spc.DomainMessages, "ProposalItemEndDateMandatory", _proposalItem);
            svc.ExecuteRule(spc.DomainMessages, "ProposalItemDates", _proposalItem);

            // Do validation specific to each proposal
            switch (_view.ShowProposals)
            {
                case ProposalTypeDisplays.CounterProposal:
                    {
                        break;
                    }
                default:
                    {
                        if (_view.SelectedMarketRate == SAHL.Common.Constants.DefaultDropDownItem)
                        {
                            errorMessage = "Must select a Market Rate.";
                            _view.Messages.Add(new Error(errorMessage, errorMessage));
                        }
                        break;
                    }
            }

            svc.ExecuteRule(spc.DomainMessages, "ProposalItemInterestRateMandatory", _proposalItem);
            svc.ExecuteRule(spc.DomainMessages, "ProposalMaxInterestRate", _proposalItem);
            svc.ExecuteRule(spc.DomainMessages, "ProposalItemAmountMandatory", _proposalItem);
            svc.ExecuteRule(spc.DomainMessages, "ProposalItemAmountZeroValue", _proposalItem);
        }
    }
}