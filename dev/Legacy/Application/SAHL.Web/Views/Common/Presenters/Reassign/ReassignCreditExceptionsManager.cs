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
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.UI;
using System.Collections.Generic;
using SAHL.Common.BusinessModel;
using SAHL.Common.Service.Interfaces;
using Castle.ActiveRecord;
using SAHL.Common.Exceptions;
using SAHL.Common.Globals;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Collections;

using System.Threading;
using SAHL.Common.DomainMessages;
using SAHL.Common;

namespace SAHL.Web.Views.Common.Presenters.Reassign
{
    /// <summary>
    /// 
    /// </summary>
    public class ReassignCreditExceptionsManager : SAHLCommonBasePresenter<IReassignUser>
    {
        private int _applicationKey;
        private CBOMenuNode _node;
        private IOrganisationStructureRepository _oSR;
        private IADUser user;
        private IApplicationRole _ar;
        InstanceNode _instanceNode;
        long _instanceID;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public ReassignCreditExceptionsManager(IReassignUser view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            _applicationKey = Convert.ToInt32(_node.GenericKey);
            _instanceNode = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal, CBONodeSetType.X2) as InstanceNode;
            _instanceID = _instanceNode.InstanceID;
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

            _view.OnCancelButtonClicked += new EventHandler(_view_OnCancelButtonClicked);
            _view.OnSubmitButtonClicked += new EventHandler(_view_OnSubmitButtonClicked);

            _oSR = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
            _ar = _oSR.GetApplicationRoleForADUser(_applicationKey, Thread.CurrentPrincipal.Identity.Name);

            if (_ar != null)
            {
                user = _oSR.GetAdUserForAdUserName(Thread.CurrentPrincipal.Identity.Name);
                IApplicationRoleType art = _oSR.GetApplicationRoleTypeByKey((int)OfferRoleTypes.CreditExceptionsD);
                IEventList<IADUser> lstUsers = _oSR.GetUsersForDynamicRole(art, false);

                _view.SetPostBackType = false;
                _view.BindConsultantList(lstUsers);
                _view.BindSelectedUser(user);
            }
            else
            {
                string errorMessage = string.Format("Action cannot be performed as User {0} does not have a current active role in the application. The application should be assigned to the user in order to carry out this action.", Thread.CurrentPrincipal.Identity.Name);
                _view.Messages.Add(new Error(errorMessage, errorMessage));
                _view.RoleVisible = _view.SubmitButtonVisible = _view.CancelButtonVisible = _view.ConsultantsRowVisible = false;
                IX2Service svc = ServiceFactory.GetService<IX2Service>();
                svc.CancelActivity(_view.CurrentPrincipal);
            }
        }

        void _view_OnSubmitButtonClicked(object sender, EventArgs e)
        {
            // Add exclusion set
            this.ExclusionSets.Add(RuleExclusionSets.LegalEntityLeadApplicants);

            IX2Service svc = ServiceFactory.GetService<IX2Service>();
            IADUser userSelected = _oSR.GetADUserByKey(_view.SelectedConsultantKey);
            string message = string.Empty;

            if (user.Key != userSelected.Key)
            {
                TransactionScope txn = new TransactionScope();

                try
                {
                    //ILookupRepository lookups = RepositoryFactory.GetRepository<ILookupRepository>();

                    // If the ApplicationRole is null then this point should have never been reach
                    //_ar.GeneralStatus = lookups.GeneralStatuses[GeneralStatuses.Inactive];
                    //_oSR.SaveApplicationRole(_ar);

                    // Deactivate any other offerRoles of the offerRoleType that we are assigning to
                    //IApplicationRepository _appRepository = RepositoryFactory.GetRepository<IApplicationRepository>();
                    //IApplication _application = _appRepository.GetApplicationByKey(_applicationKey);

                    //foreach (IApplicationRole appRole in _application.ApplicationRoles)
                    //{
                    //    if (appRole.GeneralStatus.Key == (int)GeneralStatuses.Active &&
                    //        appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.CreditExceptionsD)
                    //    {
                    //        appRole.GeneralStatus = lookups.GeneralStatuses[GeneralStatuses.Inactive];
                    //        _oSR.SaveApplicationRole(appRole);
                    //    }
                    //}

                    //IApplicationRole _appRole = _oSR.CreateNewApplicationRole();
                    //IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
                    //IApplication app = appRepo.GetApplicationByKey(_applicationKey);
                    //_appRole.Application = app;
                    //_appRole.ApplicationRoleType = appRepo.GetApplicationRoleTypeByKey(OfferRoleTypes.CreditExceptionsD);
                    //_appRole.GeneralStatus = lookups.GeneralStatuses[GeneralStatuses.Active];
                    //_appRole.StatusChangeDate = DateTime.Now;
                    //_appRole.LegalEntity = userSelected.LegalEntity;
                    //_oSR.SaveApplicationRole(_appRole);

                    // Deactivate current application role
                    //_oSR.DeactivateApplicationRole(_ar.Key);

                    // Deactivate any other offerRoles of the offerRoleType that we are assigning to
                    //_oSR.DeactivateExistingApplicationRoles(_applicationKey, (int)OfferRoleTypes.CreditExceptionsD);

                    // Deactivate existing workflow assignment records
                    _oSR.DeactivateWorkflowAssignment(_ar, (int)_instanceID);

                    // Create the new application role
                    IApplicationRole newAppRole = _oSR.GenerateApplicationRole((int)OfferRoleTypes.CreditExceptionsD, _applicationKey, userSelected.LegalEntity.Key, true);

                    // Do X2 Stuff
                    _oSR.CreateWorkflowAssignment(newAppRole, (int)_instanceID, GeneralStatuses.Active);

                    // Create History from X2
                    IADUser fromUser = _oSR.GetAdUserByLegalEntityKey(_ar.LegalEntity.Key);
                    IADUser toUser = _oSR.GetAdUserByLegalEntityKey(newAppRole.LegalEntity.Key);
                    message = string.Format("Reassigned from {0} to {1}", fromUser.ADUserName, toUser.ADUserName);

                    this.ExclusionSets.Remove(RuleExclusionSets.LegalEntityLeadApplicants);
                    txn.VoteCommit();
                }
                catch (Exception)
                {
                    txn.VoteRollBack();
                    if (_view.IsValid)
                    {
                        svc.CancelActivity(_view.CurrentPrincipal);
                        throw;
                    }
                }
                finally
                {
                    txn.Dispose();
                }
            }
            if (_view.Messages.ErrorMessages.Count == 0)
            {
                svc.CompleteActivity(_view.CurrentPrincipal, null, false, message);
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
