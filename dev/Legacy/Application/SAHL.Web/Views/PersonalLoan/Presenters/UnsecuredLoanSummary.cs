using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.CacheData;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.PersonalLoan.Interfaces;
using System;
using System.Linq;

namespace SAHL.Web.Views.PersonalLoan.Presenters
{
    public class UnsecuredLoanSummary : SAHLCommonBasePresenter<IUnsecuredLoanSummary>
    {
        private CBOMenuNode node;
        private int accountKey;

        private IAccountRepository accountRepository;

        public IAccountRepository AccountRepository
        {
            get
            {
                if (accountRepository == null)
                {
                    accountRepository = RepositoryFactory.GetRepository<IAccountRepository>();
                }
                return accountRepository;
            }
        }

        public UnsecuredLoanSummary(IUnsecuredLoanSummary view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage)
                return;
        }

        protected override void OnViewLoaded(object sender, EventArgs e)
        {
            base.OnViewLoaded(sender, e);
            if (!_view.ShouldRunPage) return;

            node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            accountKey = node.GenericKey;

            var accountPersonalLoan = AccountRepository.GetAccountByKey(accountKey) as IAccountPersonalLoan;
            CheckRules(accountPersonalLoan);
            _view.BindAccountSummary(accountPersonalLoan);

            if (accountPersonalLoan != null && accountPersonalLoan.RelatedChildAccounts.Count > 0)
            {
                var creditProtectionAccount = accountPersonalLoan.GetRelatedAccountByType(SAHL.Common.Globals.AccountTypes.CreditProtectionPlan, accountPersonalLoan.RelatedChildAccounts) as IAccountCreditProtectionPlan;
                if (creditProtectionAccount != null && creditProtectionAccount.FinancialServices.Count > 0)
                {
                    IFinancialService creditProtectionPlan = creditProtectionAccount.FinancialServices.Single(x => x.FinancialServiceType.Key == (int)FinancialServiceTypes.SAHLCreditProtectionPlan);
                    if (creditProtectionPlan != null)
                    {
                        ILifePolicyClaim lifePolicyClaim = creditProtectionPlan.GetLifePolicyClaimPending();
                        if (lifePolicyClaim != null)
                            _view.BindLifePolicyClaimPending(lifePolicyClaim.ClaimDate.ToShortDateString());
                    }
                }
            }
        }

        /// <summary>
        /// Run these RULES before loading the screen
        /// </summary>
        public void CheckRules(IAccount _account)
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(_view.CurrentPrincipal);
            IRuleService svc = ServiceFactory.GetService<IRuleService>();
            svc.ExecuteRule(spc.DomainMessages, "AccountDebtCounseling", _account);
            svc.ExecuteRule(spc.DomainMessages, "LegalEntitiesUnderDebtCounsellingForAccount", _account);
        }
    }
}