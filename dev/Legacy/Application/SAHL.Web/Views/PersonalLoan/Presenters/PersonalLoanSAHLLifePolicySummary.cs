using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.PersonalLoan.Interfaces;
using System;

namespace SAHL.Web.Views.PersonalLoan.Presenters
{
    public class PersonalLoanSAHLLifePolicySummary : SAHLCommonBasePresenter<IPersonalLoanSAHLLifePolicySummary>
    {
        private int genericKey;
        private int genericKeyType;
        private IAccountRepository _accountRepository;

        public IAccountRepository accountRepository
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

        private IApplicationUnsecuredLendingRepository _applicationUnsecuredLendingRepository;

        public IApplicationUnsecuredLendingRepository applicationUnsecuredLendingRepository
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

        public PersonalLoanSAHLLifePolicySummary(IPersonalLoanSAHLLifePolicySummary view, SAHLCommonBaseController controller)
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

            var CurrentNode = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal);
            genericKey = CurrentNode.GenericKey;
            genericKeyType = CurrentNode.GenericKeyTypeKey;
        }

        protected override void OnViewLoaded(object sender, EventArgs e)
        {
            base.OnViewLoaded(sender, e);
            if (!_view.ShouldRunPage) return;

            switch (genericKeyType)
            {
                case (int)SAHL.Common.Globals.GenericKeyTypes.Account:
                    var creditProtectionAccount = accountRepository.GetAccountByKey(genericKey) as IAccountCreditProtectionPlan;
                    _view.BindInsurerName("SAHL Life");
                    _view.BindAccountSummary(creditProtectionAccount);
                    break;

                case (int)SAHL.Common.Globals.GenericKeyTypes.Offer:
                    var hasSAHLLifeApplied = true;
                    _view.BindInsurerName("SAHL Life");
                    try
                    {
                        hasSAHLLifeApplied = applicationUnsecuredLendingRepository.ApplicationHasSAHLLifeApplied(genericKey);
                    }
                    catch (Exception ex)
                    {
                        // Incase someone tries to view life for a lead
                        hasSAHLLifeApplied = false;
                        _view.Messages.Add(new SAHL.Common.DomainMessages.Error(ex.Message, ex.Message));
                    }
                    if (!hasSAHLLifeApplied)
                        _view.BindInsurerName("-");
                    break;

                default:
                    break;
            }
        }
    }
}