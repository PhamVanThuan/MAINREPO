using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.CacheData;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.PersonalLoan.Interfaces;
using System;
using System.Linq;

namespace SAHL.Web.Views.PersonalLoan.Presenters
{
    public class PersonalLoanDisbursement : SAHLCommonBasePresenter<IPersonalLoanDisbursement>
    {
        private int genericKey;
        private CBOMenuNode node;
        private IApplicationUnsecuredLendingRepository applicationUnsecuredLendingRepository;

        public IApplicationUnsecuredLendingRepository ApplicationUnsecuredLendingRepository
        {
            get
            {
                if (applicationUnsecuredLendingRepository == null)
                {
                    applicationUnsecuredLendingRepository = RepositoryFactory.GetRepository<IApplicationUnsecuredLendingRepository>();
                }
                return applicationUnsecuredLendingRepository;
            }
        }

        private IApplicationRepository applicationRepository;

        public IApplicationRepository ApplicationRepository
        {
            get
            {
                if (applicationRepository == null)
                {
                    applicationRepository = RepositoryFactory.GetRepository<IApplicationRepository>();
                }
                return applicationRepository;
            }
        }

        public PersonalLoanDisbursement(IPersonalLoanDisbursement view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage)
                return;

            _view.ConfirmClick += new EventHandler<EventArgs>(OnConfirmClick);
            _view.CancelClick += new EventHandler<EventArgs>(OnCancelClick);
        }

        protected override void OnViewLoaded(object sender, EventArgs e)
        {
            base.OnViewLoaded(sender, e);
            if (!_view.ShouldRunPage)
                return;

            node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            genericKey = node.GenericKey;

            var applicationPersonalLoan = ApplicationRepository.GetApplicationByKey(genericKey);
            var latestAcceptedOfferInformation = applicationPersonalLoan.ApplicationInformations.Where(x => x.ApplicationInformationType.Key == (int)OfferInformationTypes.AcceptedOffer)
                                                                            .OrderByDescending(x => x.Key)
                                                                            .FirstOrDefault();

            var bankAccount = applicationPersonalLoan.ApplicationDebitOrders.First().BankAccount;
            var applicationProductPersonalLoan = latestAcceptedOfferInformation.ApplicationProduct as IApplicationProductPersonalLoan;

            _view.BindApplicationInformation(applicationProductPersonalLoan.ApplicationInformationPersonalLoan);
            _view.BindBankAccountInformation(bankAccount);
        }

        private void OnCancelClick(object sender, EventArgs e)
        {
            this.X2Service.CancelActivity(_view.CurrentPrincipal);
            this.X2Service.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
        }

        private void OnConfirmClick(object sender, EventArgs e)
        {
            try
            {
                SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(_view.CurrentPrincipal);
                this.X2Service.CompleteActivity(_view.CurrentPrincipal, null, spc.IgnoreWarnings);
                this.X2Service.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
            }
            catch (Exception)
            {
                if (_view.IsValid)
                {
                    throw;
                }
            }
        }
    }
}