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
using SAHL.Common.Service;

using System.Threading;
using SAHL.Common.DomainMessages;
using SAHL.Common;

namespace SAHL.Web.Views.Common.Presenters.Reassign
{
    /// <summary>
    /// 
    /// </summary>
    public class ReassignSeniorCreditAnalyst : SAHLCommonBasePresenter<IReassignUser>
    {
        int _applicationKey;
        CBOMenuNode _node;
        IOrganisationStructureRepository _oSR;
        IADUser user;
        IApplicationRole _ar;
        InstanceNode _instanceNode;
        long _instanceID;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public ReassignSeniorCreditAnalyst(IReassignUser view, SAHLCommonBaseController controller)
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

            _oSR = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
            _ar = _oSR.GetApplicationRoleForADUser(_applicationKey, Thread.CurrentPrincipal.Identity.Name);

            if (_ar != null)
            {
                user = _oSR.GetAdUserForAdUserName(Thread.CurrentPrincipal.Identity.Name);

                IOrganisationStructureRepository osRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                IEventList<IADUser> users = osRepo.GetOrganisationStructureForKey(1008).ADUsers;
                Dictionary<int, string> LEs = new Dictionary<int, string>();
                foreach (IADUser usr in users)
                {
                    if (!LEs.ContainsKey(usr.LegalEntity.Key) && usr.GeneralStatusKey.Key == (int)GeneralStatuses.Active)
                    {
                        LEs.Add(usr.LegalEntity.Key, usr.ADUserName);
                    }
                }

                _view.SetPostBackType = false;
                _view.BindConsultantsAsPerMandates(LEs);
                _view.BindSelectedUserByMandate(user);
                _view.OnSubmitButtonClicked += new EventHandler(_view_OnSubmitButtonClicked);
                _view.OnCancelButtonClicked += new EventHandler(_view_OnCancelButtonClicked);
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

        //private Dictionary<int, string> GetSeniorCreditAnalystsByMandate()
        //{
            //IMandateService ms = new MandateService();
            ////TODO : Change these groups to the proper names once we know what they are..
            //string[] MandateList = new string[] { "Group1", "Group2", "Group3", "Group4", "Group5", "Group6", "Group7", "Group8", "Group9", "Group10" };
            //List<IADUser> AllUsers = new List<IADUser>();
            //List<string> p_AllUsers = new List<string>();
            //Dictionary<int, string> LEs = new Dictionary<int, string>();

            //foreach (string MandateGroup in MandateList)
            //{
            //    IList<IADUser> users = ms.ExecuteMandateSet(MandateGroup, new object[] { appML });
            //    if (users.Count > 0)
            //    {
            //        foreach (IADUser usr in users)
            //        {
            //            if (!AllUsers.Contains(usr))
            //            {
            //                AllUsers.Add(usr);
            //                p_AllUsers.Add(usr.ADUserName);
            //                for (int y = 0; y < usr.UserOrganisationStructure[0].OrganisationStructure.ApplicationRoleTypes.Count; y++)
            //                {
            //                    if (usr.UserOrganisationStructure[0].OrganisationStructure.ApplicationRoleTypes[y].Key == (int)OfferRoleTypes.CreditSupervisorD)
            //                        LEs.Add(usr.LegalEntity.Key, usr.ADUserName);
            //                    break;
            //                }
            //            }
            //        }
            //    }
            //}
            //return LEs;
          //  return null;
        //}

        void _view_OnSubmitButtonClicked(object sender, EventArgs e)
        {
            // Add exclusion set
            this.ExclusionSets.Add(RuleExclusionSets.LegalEntityLeadApplicants);

            IX2Service svc = ServiceFactory.GetService<IX2Service>();
            IADUser userSelected = _oSR.GetAdUserByLegalEntityKey(_view.SelectedConsultantKey);
            string message = string.Empty;
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
                //foreach (IApplicationRole _appRole in _application.ApplicationRoles)
                //{
                //    if (_appRole.GeneralStatus.Key == (int)GeneralStatuses.Active &&
                //        _appRole.ApplicationRoleType.Key == (int)OfferRoleTypes.CreditSupervisorD)
                //    {
                //        _appRole.GeneralStatus = lookups.GeneralStatuses[GeneralStatuses.Inactive];
                //        _oSR.SaveApplicationRole(_appRole);
                //    }
                //}

                //IApplicationRole appRole = _oSR.CreateNewApplicationRole();
                //appRole.Application = (IApplication)appML;
                //appRole.ApplicationRoleType = _appRepository.GetApplicationRoleTypeByKey(OfferRoleTypes.CreditSupervisorD);
                //appRole.GeneralStatus = lookups.GeneralStatuses[GeneralStatuses.Active];
                //appRole.LegalEntity = userSelected.LegalEntity;
                //appRole.StatusChangeDate = DateTime.Now;
                //_oSR.SaveApplicationRole(appRole);

                // Deactivate current application role
                //_oSR.DeactivateApplicationRole(_ar.Key);

                // Deactivate any other offerRoles of the offerRoleType that we are assigning to
                //_oSR.DeactivateExistingApplicationRoles(_applicationKey, (int)OfferRoleTypes.CreditSupervisorD);

                // Deactivate existing workflow assignment records
                _oSR.DeactivateWorkflowAssignment(_ar, (int)_instanceID);

                // Create the new application role
                IApplicationRole newAppRole = _oSR.GenerateApplicationRole((int)OfferRoleTypes.CreditSupervisorD, _applicationKey, userSelected.LegalEntity.Key, true);

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
