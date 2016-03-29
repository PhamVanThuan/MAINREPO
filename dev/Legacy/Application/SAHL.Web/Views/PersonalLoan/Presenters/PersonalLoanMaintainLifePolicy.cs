using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.PersonalLoan.Interfaces;
using System;
using System.Linq;

namespace SAHL.Web.Views.PersonalLoan.Presenters
{
    public class PersonalLoanMaintainLifePolicy : SAHLCommonBasePresenter<IPersonalLoanMaintainLifePolicy>
    {
        public PersonalLoanMaintainLifePolicy(IPersonalLoanMaintainLifePolicy view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        IApplicationRepository applicationRepository;

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

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage)
            {
                return;
            }
            CanCaptureExternalCreditLifePolicy();

            this.View.OnCancelButtonClicked += View_OnCancelButtonClicked;
            this.View.OnSubmitButtonClicked += View_OnSubmitButtonClicked;

            ILookupRepository lookups = RepositoryFactory.GetRepository<ILookupRepository>();

            this.View.BindStatus(lookups.LifePolicyStatuses.BindableDictionary.Where(x => x.Key == "3" || x.Key == "12").ToDictionary(x => x.Key, y => y.Value));
        }

        private void CanCaptureExternalCreditLifePolicy()
        {
            CBOMenuNode cboNode = CBOManager.GetCurrentCBONode(View.CurrentPrincipal).ParentNode as CBOMenuNode;
            IApplication application = ApplicationRepository.GetApplicationByKey(cboNode.GenericKey);
            if (application.HasAttribute(OfferAttributeTypes.Life))
            {
                View.Navigator.Navigate("CancelView");
            }
        }

        private void View_OnSubmitButtonClicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void View_OnCancelButtonClicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}