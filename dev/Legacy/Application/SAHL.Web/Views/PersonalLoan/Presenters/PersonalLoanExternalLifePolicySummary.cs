using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.PersonalLoan.Interfaces;
using System;

namespace SAHL.Web.Views.PersonalLoan.Presenters
{
    public class PersonalLoanExternalLifePolicySummary : SAHLCommonBasePresenter<IPersonalLoanMaintainLifePolicy>
    {
        private int _genericKey;
        private int _genericKeyType;
        private CBOMenuNode node;
        private IApplicationUnsecuredLendingRepository _applicationUnsecuredLendingRepository;
        private IApplicationUnsecuredLendingRepository applicationUnsecuredLendingRepository
        {
            get
            {
                if (_applicationUnsecuredLendingRepository == null)
                {
                    _applicationUnsecuredLendingRepository = RepositoryFactory.GetRepository<IApplicationUnsecuredLendingRepository>();
                }
                return _applicationUnsecuredLendingRepository;
            }
        }

        private IAccountRepository _accountRepository;
        private IAccountRepository accountRepository
        {
            get
            {
                if (_accountRepository == null)
                {
                    _accountRepository = RepositoryFactory.GetRepository<IAccountRepository>();
                }
                return _accountRepository;
            }
        }

        public PersonalLoanExternalLifePolicySummary(IPersonalLoanMaintainLifePolicy view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage)
            {
                return;
            }

            node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            _genericKey = node.GenericKey > 0 ? node.GenericKey : node.ParentNode.GenericKey;
            _genericKeyType = node.GenericKeyTypeKey > 0 ? node.GenericKeyTypeKey : node.ParentNode.GenericKeyTypeKey;
        }

        protected override void OnViewLoaded(object sender, EventArgs e)
        {
            base.OnViewLoaded(sender, e);
            if (!_view.ShouldRunPage)
                return;

            if (_genericKey <= 0)
                throw new Exception("This presenter only handles personal loan application or account objects.");

            switch (_genericKeyType)
            {
                case (int)SAHL.Common.Globals.GenericKeyTypes.Account:
                    BindPersonalLoanAccountExternalPolicySummary();
                    break;

                case (int)SAHL.Common.Globals.GenericKeyTypes.Offer:
                    this.View.SetupSummaryView();
                    var applicationUnsecuredLending = applicationUnsecuredLendingRepository.GetApplicationByKey(_genericKey);

                    if (applicationUnsecuredLending == null || applicationUnsecuredLending.GetLatestApplicationInformation() == null)
                    {
                        return;
                    }
                    var latestAcceptedOfferInformation = applicationUnsecuredLending.GetLatestApplicationInformation();
                    var applicationProductPersonalLoan = latestAcceptedOfferInformation.ApplicationProduct as IApplicationProductPersonalLoan;
                    var externalLifePolicy = applicationProductPersonalLoan.ExternalLifePolicy;
                    if (externalLifePolicy != null)
                    {
                        this.View.BindMaintainLifePolicyForReadOnly(externalLifePolicy);
                    }
                    break;

                default:
                    break;
            }
        }

        private void BindPersonalLoanAccountExternalPolicySummary()
        {
            if (_genericKey > 0)
            {
                this.View.SetupSummaryView();
                var accountPersonalLoan = accountRepository.GetAccountByKey(Convert.ToInt32(_genericKey)) as IAccountPersonalLoan;
                var externalLifePolicy = accountPersonalLoan.ExternalLifePolicy;
                if (externalLifePolicy != null)
                {
                    this.View.BindMaintainLifePolicyForReadOnly(externalLifePolicy);
                }
            }
        }
    }
}