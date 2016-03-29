using System;
using System.Linq;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;

namespace SAHL.Web.Views.CreditProtectionPlan.Presenters
{
    public class LifePolicyClaimBase : SAHLCommonBasePresenter<SAHL.Web.Views.CreditProtectionPlan.Interfaces.ILifePolicyClaim>
    {
        private ILifeRepository lifeRepository;
        internal ILifeRepository LifeRepository
        {
            get
            {
                if (lifeRepository == null)
                {
                    lifeRepository = RepositoryFactory.GetRepository<ILifeRepository>();
                }
                return lifeRepository;
            }
        }

        private ILookupRepository lookupRepository;
        internal ILookupRepository LookupRepository
        {
            get
            {
                if (lookupRepository == null)
                    lookupRepository = RepositoryFactory.GetRepository<ILookupRepository>();

                return lookupRepository;
            }
        }

        internal int lifePolicyClaimGridIndexSelected;

        internal IList<ILifePolicyClaim> lifePolicyClaims;

        internal IFinancialService financialService;

        public LifePolicyClaimBase(SAHL.Web.Views.CreditProtectionPlan.Interfaces.ILifePolicyClaim view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage)
                return;

            _view.OnCancelButtonClicked += new EventHandler(_view_OnCancelButtonClicked);

            CBOMenuNode node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            if (node == null)
                throw new NullReferenceException(StaticMessages.NullCurrentCBONode);
            int accountKey = node.GenericKey;

            IAccountRepository AccountRepository = RepositoryFactory.GetRepository<IAccountRepository>();

            IAccountCreditProtectionPlan creditProtectionAccount = AccountRepository.GetAccountByKey(accountKey) as IAccountCreditProtectionPlan;

            if (creditProtectionAccount != null && creditProtectionAccount.FinancialServices.Count > 0)
            {
                financialService = creditProtectionAccount.FinancialServices.Single(x => x.FinancialServiceType.Key == (int)FinancialServiceTypes.SAHLCreditProtectionPlan);

                if (financialService != null)
                {
                    lifePolicyClaims = financialService.GetLifePolicyClaims();
                }
            }
        }

        public void _view_OnCancelButtonClicked(object sender, EventArgs e)
        {
            Navigator.Navigate("LifePolicyClaimDisplay");
        }
    }
}