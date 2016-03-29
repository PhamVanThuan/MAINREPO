using System;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.UI;
using System.Collections.Generic;
using Castle.ActiveRecord;
using SAHL.Common.Exceptions;
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.X2.BusinessModel.Interfaces;

namespace SAHL.Web.Views.Common.Presenters.Reassign
{
    public class AssignInitialConsultant : SAHLCommonBasePresenter<IReassignUser>
    {
        private int _applicationKey;
        private CBOMenuNode _node;
        private IOrganisationStructureRepository _oSR;
        private InstanceNode _instanceNode;
        private Int64 _instanceID = -1;
        string Prefix = "Branch";

        public AssignInitialConsultant(IReassignUser view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }

        public static string GetDynamicRolePrefixForCreatorOwner(Int64 Instance)
        {
            IX2Repository x2Repo = RepositoryFactory.GetRepository<IX2Repository>();
            IOrganisationStructureRepository osRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
            IInstance IID = x2Repo.GetInstanceByKey(Instance);
            string CreatorUser = IID.CreatorADUserName;
            IADUser user = osRepo.GetAdUserForAdUserName(CreatorUser);
            IX2Service x2svc = ServiceFactory.GetService<IX2Service>();
            IDictionary<string, object> props = x2svc.GetX2DataRow(Instance);
            int ApplicationKey = Convert.ToInt32(props["ApplicationKey"]);
            IEventList<IApplicationRole> roles = osRepo.FindApplicationRolesForApplicationKeyAndLEKey(ApplicationKey, user.LegalEntity.Key);
            string s = roles[0].ApplicationRoleType.Description;
            int idx = s.IndexOf(" ");
            string Prefix = s.Substring(0, idx);
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

            TransactionScope txn = new TransactionScope();

            try
            {
                _view.SetDropDownText = "User";

                _oSR = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                IADUser user = _oSR.GetAdUserForAdUserName(_view.CurrentPrincipal.Identity.Name);

                Prefix = GetDynamicRolePrefixForCreatorOwner(_instanceID);
                IEventList<IADUser> users = _oSR.GetBranchUsersForUserInThisBranch(user, OrganisationStructureGroup.Consultant, Prefix);

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

            _view.OnCancelButtonClicked += (_view_OnCancelButtonClicked);
            _view.OnSubmitButtonClicked += (_view_OnSubmitButtonClicked);
        }

        void _view_OnSubmitButtonClicked(object sender, EventArgs e)
        {
            TransactionScope txn = new TransactionScope();
            IX2Service svc = ServiceFactory.GetService<IX2Service>();
            IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();

            try
            {
                // only fire minimum required field validation
                ExclusionSets.Add(RuleExclusionSets.LegalEntityLeadApplicants);

                IADUser user = _oSR.GetADUserByKey(_view.SelectedConsultantKey);
                IApplicationRole _ar ;
                IApplicationRoleType artNew ;
                IApplicationRole ccAppRole = null;
                IApplication app  = appRepo.GetApplicationByKey(_applicationKey);

                if (Prefix.ToUpper() == "BRANCH")
                {
                    artNew = appRepo.GetApplicationRoleTypeByKey(OfferRoleTypes.BranchConsultantD);
                    _ar = _oSR.GetLatestApplicationRoleByApplicationKeyAndRoleTypeKey(_applicationKey, (int)OfferRoleTypes.BranchAdminD, (int)GeneralStatuses.Active);
                    ccAppRole = _oSR.GetLatestApplicationRoleByApplicationKeyAndRoleTypeKey(_applicationKey, (int)OfferRoleTypes.CommissionableConsultant, (int)GeneralStatuses.Active);
                }
                else if (Prefix.ToUpper() == "EA")
                {
                    artNew = appRepo.GetApplicationRoleTypeByKey(OfferRoleTypes.EAConsultantD);
                    _ar = _oSR.GetLatestApplicationRoleByApplicationKeyAndRoleTypeKey(_applicationKey, (int)OfferRoleTypes.EAAdminD, (int)GeneralStatuses.Active);
                }
                else
                {
                    throw new Exception("Unknown Admin Type");
                }

                if (_ar != null)
                {
                    // The "BranchAdminD" role should not be overwritten in this part but a new OfferRole should be created

                    //_ar.LegalEntity = user.LegalEntity;
                    //_ar.StatusChangeDate = DateTime.Now;
                    //_ar.ApplicationRoleType = artNew;
                    //_oSR.SaveApplicationRole(_ar);

                    IApplicationRole newAppRole = _oSR.GenerateApplicationRole(artNew.Key, app.Key, user.LegalEntity.Key, true);

                    // Do X2 Stuff
                    _oSR.CreateWorkflowAssignment(newAppRole, (int)_instanceID, GeneralStatuses.Active);

                    if (artNew.Key == (int)OfferRoleTypes.BranchConsultantD && ccAppRole == null)
                    {
                        //http://sahls31:8181/trac/SAHL.db/ticket/12791
                        _oSR.GenerateApplicationRole((int)OfferRoleTypes.CommissionableConsultant, app.Key, user.LegalEntity.Key, true);
                    }
                }
                else
                    throw new Exception("Application Role not found");

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

        void _view_OnCancelButtonClicked(object sender, EventArgs e)
        {
            IX2Service svc = ServiceFactory.GetService<IX2Service>();
            svc.CancelActivity(_view.CurrentPrincipal);
            svc.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
        }

    }
}
