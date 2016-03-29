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
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Web.Views.PersonalLoan.Presenters
{
    public class PersonalLoanApplicationSummary : SAHLCommonBasePresenter<IPersonalLoanApplicationSummary>
    {
        private int _applicationKey;
        private CBOMenuNode node;
        private IApplicationRepository applicationRepository;
        private IX2Repository x2Repository;
        private IStageDefinitionRepository stageDefinitionRepository;
        private IControlRepository controlRepository;
        private IRuleService ruleService;

        private IControlRepository ControlRepository
        {
            get
            {
                if (controlRepository == null)
                {
                    controlRepository = RepositoryFactory.GetRepository<IControlRepository>();
                }
                return controlRepository;
            }
        }

        private IApplicationRepository ApplicationRepository
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

        private IX2Repository X2Repository
        {
            get
            {
                if (x2Repository == null)
                {
                    x2Repository = RepositoryFactory.GetRepository<IX2Repository>();
                }
                return x2Repository;
            }
        }

        private IStageDefinitionRepository StageDefinitionRepository
        {
            get
            {
                if (stageDefinitionRepository == null)
                {
                    stageDefinitionRepository = RepositoryFactory.GetRepository<IStageDefinitionRepository>();
                }
                return stageDefinitionRepository;
            }
        }

        private IRuleService RuleService
        {
            get
            {
                if (ruleService == null)
                {
                    ruleService = ServiceFactory.GetService<IRuleService>();
                }
                return ruleService;
            }
        }

        public PersonalLoanApplicationSummary(IPersonalLoanApplicationSummary view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage)
                return;

            _view.OnTransitionHistoryClicked += _view_OnTransitionHistoryClicked;
            _view.OnCancelButtonClicked += _view_OnCancelButtonClicked;

            if (GlobalCacheData.ContainsKey(ViewConstants.ApplicationKey))
            {
                // get the applicatonkey from the global cache and clear it out IMMEDIATELY, otherwise we run
                // the risk of the user clicking a menu option and seeing the wrong application - can't rely on
                // button clicks etc - if you need this for postbacks use a PrivateCache
                _applicationKey = Convert.ToInt32(GlobalCacheData[ViewConstants.ApplicationKey]);
                GlobalCacheData.Remove(ViewConstants.ApplicationKey);
            }
            else
            {
                node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
                _applicationKey = node.GenericKey;
            }
        }

        void _view_OnCancelButtonClicked(object sender, EventArgs e)
        {
            _view.ShouldRunPage = false;
            Navigator.Navigate("Cancel");
        }

        void _view_OnTransitionHistoryClicked(object sender, EventArgs e)
        {
            _view.ShouldRunPage = false;
            if (GlobalCacheData.ContainsKey(ViewConstants.ApplicationKey))
                GlobalCacheData.Remove(ViewConstants.ApplicationKey);

            IList<ICacheObjectLifeTime> LifeTimes = new List<ICacheObjectLifeTime>();
            GlobalCacheData.Add(ViewConstants.ApplicationKey, PrivateCacheData[ViewConstants.ApplicationKey], LifeTimes);

            Navigator.Navigate("TransitionHistory");
        }

        protected override void OnViewLoaded(object sender, EventArgs e)
        {
            base.OnViewLoaded(sender, e);
            if (!_view.ShouldRunPage)
                return;

            if (_applicationKey > 0)
            {
                //Populate the Private Cache for navigate to History if it is valid
                if (PrivateCacheData.ContainsKey(ViewConstants.ApplicationKey))
                    PrivateCacheData.Remove(ViewConstants.ApplicationKey);

                PrivateCacheData.Add(ViewConstants.ApplicationKey, _applicationKey);

                var application = ApplicationRepository.GetApplicationByKey(_applicationKey);
                IApplicationInformationPersonalLoan plInfo = null;
                if (application.ApplicationInformations.Count > 0)
                {
                    var latestApplicationInformation = application.CurrentProduct as IApplicationProductPersonalLoan;

                    if (latestApplicationInformation != null)
                    {
                        plInfo = latestApplicationInformation.ApplicationInformationPersonalLoan;
                    }
                }

                RuleService.ExecuteRule(_view.Messages, "CheckAlteredApprovalStageTransition", _applicationKey);

                IApplicationUnsecuredLending appUL = (IApplicationUnsecuredLending)application;
                foreach (IExternalRole er in appUL.ActiveClientRoles)
                {
                    RuleService.ExecuteRule(_view.Messages, "LegalEntityUnderDebtCounselling", er.LegalEntity);
                    RuleService.ExecuteRule(_view.Messages, "LegalEntityOpenFurtherLending", er.LegalEntity);

                    //Run the rule if the Application has not been approved by credit
                    if (!HasCreditApproveApplication(appUL.Key))
                    {
                        RuleService.ExecuteRule(_view.Messages, "HasAccountInArrearsInLast6Months", er.LegalEntity);
                    }
                }

                var consultant = X2Repository.GetWorkflowRoleForGenericKey(application.Key, (int)WorkflowRoleTypes.PLConsultantD, (int)GeneralStatuses.Active).FirstOrDefault();
                IControl monthlyFee = ControlRepository.GetControlByDescription("PersonalLoanMonthlyFee");
                _view.BindApplicationSummary(application, plInfo, consultant, monthlyFee);
            }
        }

        private bool HasCreditApproveApplication(int applicationKey)
        {
            var approveTransitions = StageDefinitionRepository.GetStageTransitionList(applicationKey, (int)GenericKeyTypes.Offer, new List<int> { (int)StageDefinitionStageDefinitionGroups.PersonalLoanCreditApprove });
            var declineTransitions = StageDefinitionRepository.GetStageTransitionList(applicationKey, (int)GenericKeyTypes.Offer, new List<int> { (int)StageDefinitionStageDefinitionGroups.PersonalLoanCreditDecline });
            return approveTransitions.Count > declineTransitions.Count;
        }
    }
}