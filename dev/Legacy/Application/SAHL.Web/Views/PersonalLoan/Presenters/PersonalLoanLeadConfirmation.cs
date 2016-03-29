using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.CacheData;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.X2.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Web.Views.PersonalLoan.Presenters
{
    public class PersonalLoanLeadConfirmation : SAHLCommonBasePresenter<IWorkFlowConfirm>
    {
        private CBOMenuNode _node;
        private int _legalEntityKey;

        public PersonalLoanLeadConfirmation(IWorkFlowConfirm view, SAHLCommonBaseController controller)
            : base(view, controller) { }

        private ILegalEntityRepository legalEntityRepository;

        public ILegalEntityRepository LegalEntityRepository
        {
            get
            {
                if (legalEntityRepository == null)
                {
                    legalEntityRepository = RepositoryFactory.GetRepository<ILegalEntityRepository>();
                }
                return legalEntityRepository;
            }
        }

        #region Events

        /// <summary>
        /// Hook the events fired by the view and call relevant methods to bind control data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage) return;

            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            _legalEntityKey = _node.GenericKey;
            ILegalEntity entity = LegalEntityRepository.GetLegalEntityByKey(_legalEntityKey);
            ILegalEntityType entityType = entity.LegalEntityType;

            if (((LegalEntityTypes)entityType.Key) != LegalEntityTypes.NaturalPerson)
            {
                //Navigate to View
                _view.Navigator.Navigate("Notification");

                SimplePageCacheObjectLifeTime lifeTime = new SimplePageCacheObjectLifeTime(new List<string> { _view.ViewName, "Notification" });
                base.GlobalCacheData.Add("NotificationMessage", "A personal loan lead cannot be created for a company/trust/cc.", lifeTime);

                return;
            }

            base._view.OnYesButtonClicked += new EventHandler(_view_OnYesButtonClicked);
            base._view.OnNoButtonClicked += new EventHandler(_view_OnNoButtonClicked);

            base._view.ShowControls(true);

            _view.TitleText = "Confirm Personal Loan Lead Creation";
        }

        #endregion Events

        #region Event Handlers

        /// <summary>
        /// Handles No button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _view_OnNoButtonClicked(object sender, EventArgs e)
        {
            Navigator.Navigate("Cancel");
        }

        /// <summary>
        /// Handles Yes button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _view_OnYesButtonClicked(object sender, EventArgs e)
        {
            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal, CBOManager.GetCurrentNodeSetName(_view.CurrentPrincipal)) as CBOMenuNode;

            ILegalEntityRepository legalEntityRepository = RepositoryFactory.GetRepository<ILegalEntityRepository>();

            _legalEntityKey = Convert.ToInt32(_node.GenericKey);

            var legalEntity = legalEntityRepository.GetLegalEntityByKey(_legalEntityKey);

            IApplicationUnsecuredLending appPL = null;
            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    var ruleService = ServiceFactory.GetService<IRuleService>();
                    ruleService.ExecuteRule(_view.Messages, "CheckUniquePersonalLoanApplication", _legalEntityKey);
                    ruleService.ExecuteRule(_view.Messages, "LegalEntityUnderDebtCounselling", legalEntity);
                    ruleService.ExecuteRule(_view.Messages, "CheckIfCapitecClient", legalEntity);

                    if (!_view.IsValid)
                    {
                        return;
                    }

                    IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();

                    appPL = appRepo.CreateUnsecuredLendingLead();

                    // Save the Application
                    appRepo.SaveApplication(appPL);

                    // Add the consultant role - this must be added after the application is saved and has been assigned a key
                    legalEntityRepository.InsertExternalRole(ExternalRoleTypes.Client, appPL.Key, GenericKeyTypes.Offer, _legalEntityKey, false);

                    //added by Kedar
                    if (_view.IsValid)
                    {
                        //coded by Kedar 24/01/2012
                        //modified by Deepha accd to review comments.
                        CreateWorkflowCase(appPL.Key);

                        transactionScope.VoteCommit();
                        Navigator.Navigate("Cancel");
                    }
                }
                catch (Exception)
                {
                    transactionScope.VoteRollBack();
                    if (_view.IsValid)
                        throw;
                }
            }
        }

        #endregion Event Handlers

        #region X2 Integration

        /// <summary>
        /// Create a workflow case on lead create
        /// </summary>
        ///<param name="applicationKey"></param>
        private void CreateWorkflowCase(int applicationKey)
        {
            bool created = false;
            try
            {
                // once we have an application create a workflow case
                Dictionary<string, string> Inputs = new Dictionary<string, string>();
                Inputs.Add("ApplicationKey", applicationKey.ToString());

                IX2Info XI = X2Service.GetX2Info(_view.CurrentPrincipal);
                if (XI == null || String.IsNullOrEmpty(XI.SessionID))
                    X2Service.LogIn(_view.CurrentPrincipal);

                SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(_view.CurrentPrincipal);
                X2Service.CreateWorkFlowInstance(_view.CurrentPrincipal,
                                                    SAHL.Common.Constants.WorkFlowProcessName.PersonalLoan,
                                                    (-1).ToString(),
                                                    SAHL.Common.Constants.WorkFlowName.PersonalLoans,
                                                    SAHL.Common.Constants.WorkFlowActivityName.CreatePersonalLoanLead,
                                                    Inputs,
                                                    spc.IgnoreWarnings);

                created = true;
                X2Service.CreateCompleteActivity(_view.CurrentPrincipal, Inputs, spc.IgnoreWarnings, null);
            }
            catch (Exception)
            {
                if (created)
                    X2Service.CancelActivity(_view.CurrentPrincipal);

                if (_view.IsValid) // if not domain validation exc
                    throw;
            }
        }

        #endregion X2 Integration
    }
}