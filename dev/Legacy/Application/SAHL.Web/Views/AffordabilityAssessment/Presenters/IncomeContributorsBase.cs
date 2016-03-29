using SAHL.Common.CacheData;
using SAHL.Common.DomainMessages;
using SAHL.Common.Globals;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;
using SAHL.V3.Framework;
using SAHL.V3.Framework.DomainServices;
using SAHL.Web.Views.AffordabilityAssessment.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Web.Views.AffordabilityAssessment.Presenters
{
    public class IncomeContributorsBase : SAHLCommonBasePresenter<IIncomeContributors>
    {
        protected IV3ServiceManager v3ServiceManager = V3ServiceManager.Instance;
        protected IApplicationDomainService applicationDomainService;
        protected string errorMessage;
        protected CBOMenuNode cboNode = null;

        public IncomeContributorsBase(IIncomeContributors view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!View.ShouldRunPage)
            {
                return;
            }

            v3ServiceManager = V3ServiceManager.Instance;
            applicationDomainService = v3ServiceManager.Get<IApplicationDomainService>();

            _view.OnAddContributorButtonClicked += new EventHandler(OnAddContributorButtonClicked);
            _view.OnCancelButtonClicked += new EventHandler(OnCancelButtonClicked);

            if (GlobalCacheData.ContainsKey(ViewConstants.AffordabilityAssessmentKey))
            {
                GlobalCacheData.Remove(ViewConstants.AffordabilityAssessmentKey);
            }

            cboNode = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            if (cboNode == null)
            {
                throw new NullReferenceException(StaticMessages.NullCurrentCBONode);
            }
        }

        protected void OnAddContributorButtonClicked(object sender, EventArgs e)
        {
            if (_view.SelectedLegalEntityKey > 0)
            {
                GlobalCacheData.Add("AffordabilityAssessmentContributorLegalEntityKey", _view.SelectedLegalEntityKey, new List<ICacheObjectLifeTime>());
                _view.Navigator.Navigate("CreateNewEntity");
            }
            else
            {
                errorMessage = "Please select the applicant to relate the non-applicant income contributor to.";
                _view.Messages.Add(new Error(errorMessage, errorMessage));
            }
        }

        protected void OnCancelButtonClicked(object sender, EventArgs e)
        {
            _view.Navigator.Navigate("Cancel");
        }

        protected void ValidateUserInput()
        {
            if (_view.GetContributorsList.Count() == 0)
            {
                errorMessage = "At least one Income Contributor must be selected.";
                _view.Messages.Add(new Error(errorMessage, errorMessage));
            }

            if (_view.NumberOfContributingApplicants <= 0)
            {
                errorMessage = "Contributing Applicants must be greater than 0.";
                _view.Messages.Add(new Error(errorMessage, errorMessage));
            }
        }
    }
}