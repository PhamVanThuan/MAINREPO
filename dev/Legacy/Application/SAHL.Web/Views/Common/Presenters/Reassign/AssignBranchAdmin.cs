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
using Castle.ActiveRecord;
using SAHL.Common.Exceptions;
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.X2.BusinessModel.Interfaces;

using SAHL.Common.Collections;

namespace SAHL.Web.Views.Common.Presenters.Reassign
{
    public class AssignBranchAdmin : SAHLCommonBasePresenter<IReassignUser>
    {
        private int _applicationKey;
        private CBOMenuNode _node;
        private IOrganisationStructureRepository _oSR;
        private InstanceNode _instanceNode;
        private Int64 _instanceID = -1;
        IApplicationRole appRoleBranchAdmin;
        //IReadOnlyEventList<IApplicationRole> appRoles;

        public AssignBranchAdmin(IReassignUser view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;

            if (_node != null)
            {
                if (_node is InstanceNode)
                {
                    _instanceNode = _node as InstanceNode;
                    _applicationKey = Convert.ToInt32(_instanceNode.GenericKey);
                    _instanceID = Convert.ToInt64(_instanceNode.InstanceID);
                }
                else
                {
                    _applicationKey = Convert.ToInt32(_node.GenericKey);
                    _instanceID = 0;
                }
            } 

        }

        public static string GetDynamicRolePrefixForCreatorOwner(Int64 Instance, ref IADUser aduser)
        {
            IOrganisationStructureRepository osRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();

            IX2Service x2svc = ServiceFactory.GetService<IX2Service>();
            IDictionary<string, object> props = x2svc.GetX2DataRow(Instance);
            string CaseOwnerName = props["CaseOwnerName"].ToString();
            int ApplicationKey = Convert.ToInt32(props["ApplicationKey"]);

            // In some instance CaseOwnerName is not populated
            if (String.IsNullOrEmpty(CaseOwnerName) && CaseOwnerName.Trim().Length == 0)
                CaseOwnerName = aduser.ADUserName;

            aduser = osRepo.GetAdUserForAdUserName(CaseOwnerName);
            IEventList<IApplicationRole> roles = osRepo.FindApplicationRolesForApplicationKeyAndLEKey(ApplicationKey, aduser.LegalEntity.Key);
            
            //  This is done to cater
            // 1) Internet Leads 
            // 2) Instances where no CaseOwnerName exists 
            // 3) Where the logged in user does not play a role in the application

            if (roles == null || roles.Count == 0)
            {
                IApplicationRole _appRole = osRepo.GetTopApplicationRoleForApplicationKey(ApplicationKey);
                roles = new EventList<IApplicationRole>();
                roles.Add(null, _appRole);
                // ADUser will need to be set to the top active role found against the application
                aduser = osRepo.GetAdUserByLegalEntityKey(roles[0].LegalEntity.Key);

            }

            string Prefix = string.Empty;
            foreach (IApplicationRole ar in roles)
            {
                if (ar.ApplicationRoleType.ApplicationRoleTypeGroup.Key == (int)OfferRoleTypeGroups.Operator)
                {
                    string s = ar.ApplicationRoleType.Description;
                    int idx = s.IndexOf(" ");
                    Prefix = s.Substring(0, idx);
                    return Prefix;
                }
            }
            return Prefix;
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

            IEventList<IADUser> users = null;

            TransactionScope txn = new TransactionScope();
            try
            {
                _view.SetDropDownText = "Branch Admin";

                _oSR = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                IADUser user = _oSR.GetAdUserForAdUserName(_view.CurrentPrincipal.Identity.Name);
                
                // Admin looking for consultants : pass the aduser of the admin, DynamicRole="Branch Admin D", OrgStucture="Consultant"
                // Consultant wanting to change the admin : pass the aduser of the consult, DynamicRole="Branch Consultant D", OrgStucture="Admin"
                string Prefix = GetDynamicRolePrefixForCreatorOwner(_instanceID,ref user);
                users = _oSR.GetBranchUsersForUserInThisBranch(user, OrganisationStructureGroup.Admin, Prefix);

                if (users != null && users.Count > 0)
                    _view.BindUsers(users);
            }

            catch (Exception)
            {
                txn.VoteRollBack();
                if (_view.IsValid)
                    throw;
            }

            finally
            {
                txn.Dispose();
            }

            appRoleBranchAdmin = GetSelectedBranchAdmin();
            
            if (appRoleBranchAdmin != null)
                _view.BindSelectedBranchAdmin(appRoleBranchAdmin, users);

            _view.OnCancelButtonClicked += new EventHandler(_view_OnCancelButtonClicked);
            _view.OnSubmitButtonClicked += new EventHandler(_view_OnSubmitButtonClicked);

        }

        private IApplicationRole GetSelectedBranchAdmin()
        {
            //appRoles = app.GetApplicationRolesByType(OfferRoleTypes.BranchAdminD);

            //if (appRoles != null && appRoles.Count > 0)
            //{
            //    for (int i = 0; i < appRoles.Count; i++)
            //    {
            //        if (appRoles[i].GeneralStatus.Key == (int)GeneralStatuses.Active)
            //        {
            //            appRoleBranchAdmin = appRoles[i];
            //            break;
            //        }
            //    }
            //}
            //return appRoleBranchAdmin;

            //http://sahls31:8181/trac/SAHL.db/ticket/12791
            return _oSR.GetLatestApplicationRoleByApplicationKeyAndRoleTypeKey(_applicationKey, (int)OfferRoleTypes.BranchAdminD, (int)GeneralStatuses.Active);
        }

        void _view_OnSubmitButtonClicked(object sender, EventArgs e)
        {
            TransactionScope txn = new TransactionScope();
            IX2Service svc = ServiceFactory.GetService<IX2Service>();
            try
            {

                // only fire minimum required field validation
                this.ExclusionSets.Add(RuleExclusionSets.LegalEntityLeadApplicants);

                // Get Existing BranchAdmin Role - if there is one - and set to inactive
                appRoleBranchAdmin = GetSelectedBranchAdmin();
                // if (appRoleBranchAdmin != null)
                // {
                //     appRoleBranchAdmin.GeneralStatus = lookups.GeneralStatuses[GeneralStatuses.Inactive];
                //     _oSR.SaveApplicationRole(appRoleBranchAdmin);
                //}

                //IADUser user = _oSR.GetADUserByKey(_view.SelectedConsultantKey);
                //IApplicationRole _ar = _oSR.CreateNewApplicationRole();
                //IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
                //IApplication app = appRepo.GetApplicationByKey(_applicationKey);
                //_ar.Application = app;
                //_ar.ApplicationRoleType = appRepo.GetApplicationRoleTypeByKey(OfferRoleTypes.BranchAdminD);
                //_ar.GeneralStatus = lookups.GeneralStatuses[GeneralStatuses.Active];
                //_ar.StatusChangeDate = DateTime.Now;
                //_ar.LegalEntity = user.LegalEntity;
                //_oSR.SaveApplicationRole(_ar);

                // http://sahls31:8181/trac/SAHL.db/ticket/12791
                if (appRoleBranchAdmin != null)
                {
                    //_oSR.DeactivateApplicationRole(appRoleBranchAdmin.Key);
                }
                IADUser user = _oSR.GetADUserByKey(_view.SelectedConsultantKey);
                IApplicationRole newAppRole = _oSR.GenerateApplicationRole((int)OfferRoleTypes.BranchAdminD, _applicationKey, user.LegalEntity.Key, true);

                // Do X2 Stuff
                _oSR.CreateWorkflowAssignment(newAppRole, (int)_instanceID, GeneralStatuses.Active);

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
