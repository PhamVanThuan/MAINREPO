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
using SAHL.Common.X2.BusinessModel.Interfaces;

using SAHL.Common.CacheData;
using SAHL.Common;
using SAHL.X2.Framework.Interfaces;
using SAHL.Common.DomainMessages;

namespace SAHL.Web.Views.Common.Presenters.Reassign
{
    public class ReassignUserState : ReassignUserBase
    {
        private IApplicationRoleType _appRoleType;
        private IOrganisationStructure _organisationStructure;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public ReassignUserState(IReassignUser view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
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
            _view.onSelectedRoleChanged += new KeyChangedEventHandler(_view_onSelectedRoleChanged);
            IEventList<IApplicationRoleType> lstAppRoleTypes = RetrieveReAssignAppRoleTypes();
            if (lstAppRoleTypes.Count > 0)
            {
                _view.SetPostBackType = false;
                _view.SetPostBackTypeRole = true;
                _view.BindRoles(lstAppRoleTypes);
                _view.ConsultantsRowVisible = false;
                _view.ShowRolesDropDown();
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
                    _oSR.DeactivateWorkflowAssignment(_appRole, (int)_instanceID);

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

        void _view_OnCancelButtonClicked(object sender, EventArgs e)
        {
            IX2Service svc = ServiceFactory.GetService<IX2Service>();
            svc.CancelActivity(_view.CurrentPrincipal);
            svc.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
        }

        #region Helper Methods

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
                View.Messages.Add(new Error(errorMsg,errorMsg));
                return false;
            }
        }

        IEventList<IApplicationRoleType> RetrieveReAssignAppRoleTypes()
        {
            IEventList<IApplicationRoleType> appRoleTypeList = new EventList<IApplicationRoleType>();

            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(_view.CurrentPrincipal);
            IX2Info x2Info = spc.X2Info as IX2Info;

            // Retrieve X2 Security List
            _instanceNode = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal, CBONodeSetType.X2) as InstanceNode;
            _instanceID = _instanceNode.InstanceID;

            //_instanceID = 1242651;
            _oSR = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();

            // Determine application role appicable to the present application
            _appRole = _oSR.GetApplicationRoleForADUserAndApplication(Thread.CurrentPrincipal.Identity.Name, _applicationKey);

            if (_appRole == null)
            {
                // If the role can't be found then the application should not be allowed to be re-assigned
                return appRoleTypeList;
            }
            else
            {
                // Get the relevant organisation structure the aduser belongs to based on the role type
                _organisationStructure = _oSR.GetOrganisationStructForADUser(_appRole);
            }

            if (_organisationStructure == null)
            {
                // If the organisation structure can't be found then the application should be allowed to be re-assigned
                return appRoleTypeList;
            }
            else
            {
                // Get all the role that role into this organisation structure
                appRoleTypeList = _oSR.GetAppRoleTypesBasedOnAppAndNextState(_instanceID, x2Info.ActivityName,_organisationStructure, false);
                //appRoleTypeList = _oSR.GetAppRoleTypesBasedOnAppAndNextState(_instanceID, "Manager Reassign", _organisationStructure, false);
            }
            return appRoleTypeList;
        }

        #endregion
    }
}
