using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Collections.Interfaces;
using System.Threading;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using Castle.ActiveRecord;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Collections;
using SAHL.Common.DomainMessages;
using SAHL.Common.Exceptions;
using SAHL.Common.UI;
using SAHL.Common;

namespace SAHL.Web.Views.Common.Presenters.Reassign
{
    public class AssignPolicyOverride : ReassignUserBase
    {
        private IApplicationRole _ar;
        private IApplicationRoleType _appRoleType;
        private IOrganisationStructure _organisationStructure;
         /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public AssignPolicyOverride(IReassignUser view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            _applicationKey = Convert.ToInt32(_node.GenericKey);
            _instanceNode = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal, CBONodeSetType.X2) as InstanceNode;
            _instanceID = _instanceNode.InstanceID;
        } /// <summary>
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
            _view.onSelectedRoleChanged += new KeyChangedEventHandler(_view_onSelectedRoleChanged);

            _ar = _oSR.GetApplicationRoleForADUser(_applicationKey, Thread.CurrentPrincipal.Identity.Name);

            if (_ar != null)
            {
                // Get the relevant organisation structure the aduser belongs to based on the role type
                _organisationStructure = _oSR.GetOrganisationStructForADUser(_ar);
                if (_ar.ApplicationRoleType.Key == (int)OfferRoleTypes.CreditExceptionsD)
                {
                    string errorMessage = "Exceptions Management cannot Request Policy Override";
                    _view.Messages.Add(new Error(errorMessage, errorMessage));
                    _view.RoleVisible = _view.SubmitButtonVisible = _view.CancelButtonVisible = _view.ConsultantsRowVisible = false;
                    IX2Service svc = ServiceFactory.GetService<IX2Service>();
                    svc.CancelActivity(_view.CurrentPrincipal);
                }
                else
                {
                    //IEventList<IApplicationRole> lstAppRoles = _oSR.GetApplicationRolesByAppKey(_applicationKey, Thread.CurrentPrincipal.Identity.Name, _instanceID);
                    IEventList<IApplicationRoleType> lstAppRoles = _oSR.GetCreditRoleTypes(1005); // (int)OrganisationStructures.Credit
                    IEventList<IApplicationRoleType> lstNewAppRoles = new EventList<IApplicationRoleType>();
                    foreach (IApplicationRoleType AppRoleType in lstAppRoles)
                    {
                        if (_ar.ApplicationRoleType.Key == (int)OfferRoleTypes.CreditUnderwriterD)
                        {
                            //For Junior Analysts, only display senior analysts, credit management and exception management
                            if (AppRoleType.Key == (int)OfferRoleTypes.CreditSupervisorD ||
                                AppRoleType.Key == (int)OfferRoleTypes.CreditManagerD ||
                                AppRoleType.Key == (int)OfferRoleTypes.CreditExceptionsD)
                            {
                                lstNewAppRoles.Add(null, AppRoleType);
                            }
                        }
                        else if (_ar.ApplicationRoleType.Key == (int)OfferRoleTypes.CreditSupervisorD)
                        {
                            //For Senior Analysts, only display credit management and exception management
                            if (AppRoleType.Key == (int)OfferRoleTypes.CreditManagerD ||
                                AppRoleType.Key == (int)OfferRoleTypes.CreditExceptionsD)
                            {
                                lstNewAppRoles.Add(null, AppRoleType);
                            }
                        }
                        else if (_ar.ApplicationRoleType.Key == (int)OfferRoleTypes.CreditManagerD)
                        {
                            //For Credit Management, only display exception management
                            if (AppRoleType.Key == (int)OfferRoleTypes.CreditExceptionsD)
                            {
                                lstNewAppRoles.Add(null, AppRoleType);
                            }
                        }
                    }
                    _view.SetPostBackType = false;
                    _view.SetPostBackTypeRole = true;
                    _view.BindRoles(lstNewAppRoles);
                    _view.ConsultantsRowVisible = false;
                    _view.ShowRolesDropDown();
                }
                
                
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

        protected void _view_onSelectedRoleChanged(object sender, KeyChangedEventArgs e)
        {
            if (Convert.ToString(e.Key) != "-select-")
            {
                _appRoleType = _oSR.GetApplicationRoleTypeByKey(Convert.ToInt32(e.Key));
                IEventList<IADUser> adusers = _oSR.GetUsersForRoleTypeAndOrgStruct(_appRoleType, _organisationStructure);
                _view.ConsultantsRowVisible = true;
                _view.BindUsers(adusers);
            }
        }

        void _view_OnSubmitButtonClicked(object sender, EventArgs e)
        {
            // Add exclusion set
            this.ExclusionSets.Add(RuleExclusionSets.LegalEntityLeadApplicants);
            IX2Service svc = ServiceFactory.GetService<IX2Service>();
            IADUser userSelected = _oSR.GetADUserByKey(_view.SelectedConsultantKey);

            if (ValidateOnSubmitButtonClicked())
            {
                TransactionScope txn = new TransactionScope();
                try
                {
                    // Deactivate current application role
                    //_oSR.DeactivateApplicationRole(_appRole.Key);

                    // Deactivate existing workflow assignment records
                    _oSR.DeactivateWorkflowAssignment(_ar, (int)_instanceID);

                    // Create the new application role
                    IApplicationRole newAppRole = _oSR.GenerateApplicationRole(_view.SelectedRoleTypeKey, _applicationKey, userSelected.LegalEntity.Key, true);

                    // Do X2 Stuff
                    _oSR.CreateWorkflowAssignment(newAppRole, (int)_instanceID, GeneralStatuses.Active);

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

                if (_view.Messages.ErrorMessages.Count == 0)
                {
                    svc.CompleteActivity(_view.CurrentPrincipal, null, false);
                    svc.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
                }
            }
        }

        bool ValidateOnSubmitButtonClicked()
        {
            string errorMsg = string.Empty;

            if (Convert.ToInt32(_view.SelectedRoleTypeKey) == -1)
                errorMsg = "Please select an Application Role Type.";
            else if (Convert.ToInt32(_view.SelectedConsultantKey) == -1)
                errorMsg = "Please select a User.";

            if (string.IsNullOrEmpty(errorMsg))
                return true;
            else
            {
                View.Messages.Add(new Error(errorMsg, errorMsg));
                return false;
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
