using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.Web.UI;
using SAHL.Common.UI;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces;
using Castle.ActiveRecord;
using SAHL.Common.Exceptions;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common;


namespace SAHL.Web.Views.Common.Presenters.Reassign
{
    public class AssignEstateAgent : SAHLCommonBasePresenter<IReassignUser>
    {
        private IOrganisationStructureRepository _oSR;
        private int _applicationKey;
        private CBOMenuNode _node;
        IApplicationRepository appRepo;
        private ILookupRepository lookups;
        IApplication app;
        IEventList<IApplicationOriginator> AppOriginators;
        IApplicationRole appRoleAgent;

        public AssignEstateAgent(IReassignUser view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            
            if (_node != null)
                _applicationKey = Convert.ToInt32(_node.GenericKey);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!View.ShouldRunPage) return;

            appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
            _oSR = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
            app = appRepo.GetApplicationByKey(_applicationKey);

            _view.SetDropDownText = "Estate Agency";
            _view.SetHeaderText = "Please select Agency";

            lookups = RepositoryFactory.GetRepository<ILookupRepository>();

            AppOriginators = new EventList<IApplicationOriginator>();
            // Filter only active agents
            for (int i = 0; i < lookups.ApplicationOriginators.Count; i++)
            {
                if (lookups.ApplicationOriginators[i].GeneralStatus.Key == (int)GeneralStatuses.Active)
                    AppOriginators.Add(_view.Messages, lookups.ApplicationOriginators[i]);
            }

            _view.BindAgencies(AppOriginators);

            appRoleAgent = GetSelectedAgent();

            if (appRoleAgent != null)
                _view.BindSelectedApplicationRole(appRoleAgent, AppOriginators);

            _view.OnCancelButtonClicked += new EventHandler(_view_OnCancelButtonClicked);
            _view.OnSubmitButtonClicked += new EventHandler(_view_OnSubmitButtonClicked);

        }

        private IApplicationRole GetSelectedAgent()
        {
            //appRoles = app.GetApplicationRolesByType(OfferRoleTypes.EstateAgent);

            //if (appRoles != null && appRoles.Count > 0)
            //{
            //    for (int i = 0; i < appRoles.Count; i++)
            //    {
            //        if (appRoles[i].GeneralStatus.Key == (int)GeneralStatuses.Active)
            //        {
            //            appRoleAgent = appRoles[i];
            //            break;
            //        }
            //    }
            //}
            //return appRoleAgent;
            return _oSR.GetLatestApplicationRoleByApplicationKeyAndRoleTypeKey(_applicationKey, (int)OfferRoleTypes.EstateAgentChannel, (int)GeneralStatuses.Active);
        }

        void _view_OnSubmitButtonClicked(object sender, EventArgs e)
        {
            TransactionScope txn = new TransactionScope();
            IX2Service svc = ServiceFactory.GetService<IX2Service>();
            try
            {
                // only fire minimum required field validation
                this.ExclusionSets.Add(RuleExclusionSets.LegalEntityLeadApplicants);
                this.ExclusionSets.Add(RuleExclusionSets.LECompCCTrustContactExclude);

                // Get Existing Agency Role - if there is one - and set to inactive
                appRoleAgent = GetSelectedAgent();
                //if (appRoleAgent != null)
                //{
                //    appRoleAgent.GeneralStatus = lookups.GeneralStatuses[GeneralStatuses.Inactive];
                //    _oSR.SaveApplicationRole(appRoleAgent);
                //}    

                //IApplicationOriginator appOriginator = lookups.ApplicationOriginators.ObjectDictionary[_view.SelectedConsultantKey.ToString()];

                //IApplicationRoleType _art = _oSR.GetApplicationRoleTypeByKey((int)OfferRoleTypes.EstateAgent);
                //IApplicationRole _ar = _oSR.CreateNewApplicationRole();
               
                //_ar.Application = app;
                //_ar.ApplicationRoleType = _art;
                //_ar.GeneralStatus = lookups.GeneralStatuses[GeneralStatuses.Active];
                //_ar.StatusChangeDate = DateTime.Now;
                //_ar.LegalEntity = appOriginator.LegalEntity;
                //_oSR.SaveApplicationRole(_ar);

                // http://sahls31:8181/trac/SAHL.db/ticket/12791 
                IApplicationOriginator appOriginator = lookups.ApplicationOriginators.ObjectDictionary[_view.SelectedConsultantKey.ToString()];
                if (appRoleAgent != null)
                {
                    //_oSR.DeactivateApplicationRole(appRoleAgent.Key);
                }

                _oSR.GenerateApplicationRole((int)OfferRoleTypes.EstateAgentChannel, app.Key, appOriginator.LegalEntity.Key, true);

                // Do X2 Stuff -  this has been commented out due to ApplicationOriginator/s not linked to ADUsers
                //_oSR.CreateWorkflowAssignment(newAppRole, (int)_instanceID);
            }
            catch (Exception)
            {
                txn.VoteRollBack();
                svc.CancelActivity(_view.CurrentPrincipal);
                if (_view.IsValid)
                    throw;
            }
            finally
            {
                txn.Dispose();
            }

            if (_view.Messages.ErrorMessages.Count == 0)
            {
                svc.CompleteActivity(_view.CurrentPrincipal, null, false);
                svc.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
            }
        }

        void _view_OnCancelButtonClicked(object sender, EventArgs e)
        {
            IX2Service svc = ServiceFactory.GetService<IX2Service>();
            svc.CancelActivity(_view.CurrentPrincipal);
            svc.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
        }

    }
}
