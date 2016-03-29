using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.CacheData;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Web.Views.Common.Presenters
{
    /// <summary>
    ///
    /// </summary>
    public class ParentAccountSummary : SAHLCommonBasePresenter<IParentAccountSummary>
    {
        private IAccountRepository _accountRepository;
        private IList<IAccount> _accounts;
        private IList<IFinancialAdjustment> _financialAdjustments;
        private IAccount _account;
        private CBOMenuNode _node;
        private IMortgageLoan _mortgageLoanVariable;

        /// <summary>
        ///
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public ParentAccountSummary(IParentAccountSummary view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            // Get the CBO Node
            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            if (_node == null)
                throw new NullReferenceException(StaticMessages.NullCurrentCBONode);

            _accountRepository = RepositoryFactory.GetRepository<IAccountRepository>();
        }

        /// <summary>
        /// Hook the events fired by the view and call relevant methods to bind control data
        /// </summary>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage)
                return;

            // get the account object
            _account = _accountRepository.GetAccountByKey(_node.GenericKey);

            // Get Variable leg of Mortgage Loan
            // Unsecured MortgageLoan used in the case of RCS
            IMortgageLoanAccount mortgageLoanAccount = _account as IMortgageLoanAccount;
            if (mortgageLoanAccount != null && mortgageLoanAccount.SecuredMortgageLoan != null)
                _mortgageLoanVariable = mortgageLoanAccount.SecuredMortgageLoan;
            else if (mortgageLoanAccount != null &&
                (mortgageLoanAccount.UnsecuredMortgageLoans != null && mortgageLoanAccount.UnsecuredMortgageLoans.Count > 0))
                _mortgageLoanVariable = mortgageLoanAccount.UnsecuredMortgageLoans[0];

            _accounts = new List<IAccount>();
            _financialAdjustments = new List<IFinancialAdjustment>();

            // add the parent account to the collection
            _accounts.Add(_account);

            // add the child accounts to the collection
            foreach (IAccount account in _account.RelatedChildAccounts)
            {
                _accounts.Add(account);
            }

            IRegent regent = _accountRepository.GetRegent(_account.Key, (int)SAHL.Common.Globals.RegentStatus.NewBusiness);
            if (regent != null)
                _accounts.Add(regent as IAccount);

            // Get the list of Rate Overrides for the parent account
            foreach (IFinancialService fs in _account.FinancialServices)
            {
                switch (fs.Account.AccountStatus.Key)
                {
                    case (int)SAHL.Common.Globals.AccountStatuses.Open:
                    case (int)SAHL.Common.Globals.AccountStatuses.Locked:
                    case (int)SAHL.Common.Globals.AccountStatuses.Dormant:
                        foreach (IFinancialAdjustment fa in fs.FinancialAdjustments)
                        {
                            _financialAdjustments.Add(fa);
                        }
                        break;

                    default:
                        break;
                }
            }

            // check if interest only
            if (_mortgageLoanVariable != null)
                _view.InterestOnlyPanelVisible = _mortgageLoanVariable.HasInterestOnly();
            else
                _view.InterestOnlyPanelVisible = false;

            _view.ArearsRowVisible = true;
            _view.SubsidyPanelVisible = true;

            // check if account is linked to subsidy
            bool hasAccountSubsidy = false;
            double subsidyStopOrderAmount = 0;
            foreach (ISubsidy subsidy in _account.Subsidies)
            {
                if (subsidy.GeneralStatus.Key == (int)SAHL.Common.Globals.GeneralStatuses.Active)
                {
                    hasAccountSubsidy = true;
                    subsidyStopOrderAmount += subsidy.StopOrderAmount;
                }
            }

            if (hasAccountSubsidy)
                _view.SubsidyPanelVisible = true;
            else
                _view.SubsidyPanelVisible = false;

            // Bind the Products
            _view.BindSummaryGrid(_accounts);

            // Bind the Instalment Details
            _view.BindInstalmentDetails(_account, subsidyStopOrderAmount);

            // Sort the FADJ's
            _financialAdjustments = _financialAdjustments.OrderBy(x => x.FinancialAdjustmentStatus.Key).ThenBy(y => y.FromDate).ToList<IFinancialAdjustment>();

            // Bind the FADJ's
            _view.BindFinancialAdjustmentGrid(_financialAdjustments);

            // Run rules and display warnings if necessary
            CheckRules();
        }

        /// <summary>
        /// Run these RULES before loading the screen
        /// </summary>
        public void CheckRules()
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(_view.CurrentPrincipal);
            IRuleService svc = ServiceFactory.GetService<IRuleService>();
            svc.ExecuteRule(spc.DomainMessages, "AccountUnderForeClosure", _account);
            svc.ExecuteRule(spc.DomainMessages, "LegalEntitiesUnderDebtCounsellingForAccount", _account);
            svc.ExecuteRule(spc.DomainMessages, "AccountDebtCounseling", _account);
            svc.ExecuteRule(spc.DomainMessages, "ITCAccountApplicationDisputeIndicated", _account);
            svc.ExecuteRule(spc.DomainMessages, "CheckDisputes", _account);
            svc.ExecuteRule(spc.DomainMessages, "ProductVarifixOptInFlag", _account);
            svc.ExecuteRule(spc.DomainMessages, "AccountIsAlphaHousing", _account);
            svc.ExecuteRule(spc.DomainMessages, "NaedoDebitOrderPending", _account);
            svc.ExecuteRule(spc.DomainMessages, "ActiveSubsidyAndSalaryStopOrderConditionExistsError", _account);
        }
    }
}