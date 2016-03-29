using System;
using System.Collections.Generic;
using System.Linq;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.CreditProtectionPlan.Interfaces;

namespace SAHL.Web.Views.CreditProtectionPlan.Presenters
{
	public class CreditProtectionSummary : SAHLCommonBasePresenter<ICreditProtectionSummary>
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

		public CreditProtectionSummary(ICreditProtectionSummary view, SAHLCommonBaseController controller)
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

			var creditProtectionAccount = AccountRepository.GetAccountByKey(accountKey) as IAccountCreditProtectionPlan;
			_view.BindAccountSummary(creditProtectionAccount);

            if (creditProtectionAccount != null && creditProtectionAccount.FinancialServices.Count > 0)
            {
                IFinancialService creditProtectionPlan = creditProtectionAccount.FinancialServices.Single(x => x.FinancialServiceType.Key == (int)FinancialServiceTypes.SAHLCreditProtectionPlan);
                if (creditProtectionPlan != null)
                {
                    IList<SAHL.Common.BusinessModel.Interfaces.ILifePolicyClaim> lifePolicyClaims = creditProtectionPlan.GetLifePolicyClaims();
                    if (lifePolicyClaims != null && lifePolicyClaims.Count > 0)
                        _view.BindLifePolicyClaimGrid(lifePolicyClaims);
                }
            }
		}
	}
}