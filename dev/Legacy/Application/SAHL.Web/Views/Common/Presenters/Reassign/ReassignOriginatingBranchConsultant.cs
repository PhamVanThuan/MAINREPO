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
using SAHL.Common.DomainMessages;


namespace SAHL.Web.Views.Common.Presenters.Reassign
{
    public class ReassignOriginatingBranchConsultant : ReassignUserBase
    {

        public ReassignOriginatingBranchConsultant(IReassignUser view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!View.ShouldRunPage) return;

            _view.OnCancelButtonClicked += new EventHandler(_view_OnCancelButtonClicked);
            _view.OnSubmitButtonClicked += new EventHandler(_view_OnSubmitButtonClicked);

            // Bind Users That The Case Can Be Assigned To
            _appRole = _oSR.GetLatestApplicationRoleByApplicationKeyAndRoleTypeKey(_applicationKey, (int)OfferRoleTypes.CommissionableConsultant, (int)GeneralStatuses.Active);
            if (_appRole == null)
            {
                string errorMessage = string.Format("Action cannot be performed as there is no current active {0} role against the application.", OfferRoleTypes.CommissionableConsultant.ToString());
                _view.Messages.Add(new Error(errorMessage, errorMessage));
                DisableAllControls();
                _view.ShouldRunPage = false;
                IX2Service svc = ServiceFactory.GetService<IX2Service>();
                svc.CancelActivity(_view.CurrentPrincipal);
            }
            else
            {
                BindAssignToUsers();
            }
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {

            base.OnViewPreRender(sender, e);
            if (!View.ShouldRunPage) return;

            // Bind History To GridView
            DataTable dt = _oSR.GetReassignUserApplicationRoleList(_applicationKey, (int)OfferRoleTypes.CommissionableConsultant);
            _view.BindGridApplicationRoles(dt);
            // Set Up View For This Presenter
            _view.SetPostBackType = false;
            _view.SetPostBackTypeRole = true;
            _view.RoleVisible = false;
            _view.ShowGrid = true;
            _view.ShowCommentRow = true;
            _view.ShowCheckBoxRow = true;
            _view.SetDropDownText = "Consultant:";
            _view.SetHeaderText = "Please select the new commission earning consultant:";
            // Rule To Check If This Can Be Allowed By Current User Logged In

            if (_view.Messages.Count == 0)
            {
                if (!ValidateUser())
                {
                    IX2Service svc = ServiceFactory.GetService<IX2Service>();
                    svc.CancelActivity(_view.CurrentPrincipal);
                }
            }
        }


        #region  Events

        void _view_OnSubmitButtonClicked(object sender, EventArgs e)
        {
            IX2Service svc = ServiceFactory.GetService<IX2Service>();
            IADUser userSelected = _oSR.GetADUserByKey(_view.SelectedConsultantKey);
            string message = string.Empty;

            TransactionScope txn = new TransactionScope();
            try
            {
                // Deactivate current application role
                //_oSR.DeactivateApplicationRole(_appRole.Key);

                // Create the new application role
                IApplicationRole newAppRole = _oSR.GenerateApplicationRole(_appRole.ApplicationRoleType.Key, _applicationKey, userSelected.LegalEntity.Key, true);

                // Create History from X2
                IADUser fromUser = _oSR.GetAdUserByLegalEntityKey(_appRole.LegalEntity.Key);
                IADUser toUser = _oSR.GetAdUserByLegalEntityKey(newAppRole.LegalEntity.Key);
                message = string.Format("Reassigned Commissionable Consultant from {0} to {1}", fromUser.ADUserName, toUser.ADUserName);

                // ReAssign BranchConsultantD Role if check box ticked
                if (_view.CheckBoxValue)
                {
                    // First check if the user selected already plays an active BranchConsultantD role in the application
                    IApplicationRole currentBCRole = _oSR.GetLatestApplicationRoleByApplicationKeyAndRoleTypeKeyAndLegalEntityKey(_applicationKey, (int)OfferRoleTypes.BranchConsultantD, userSelected.LegalEntity.Key, (int)GeneralStatuses.Active);
                    if (currentBCRole == null)
                    {
                        currentBCRole = _oSR.GetLatestApplicationRoleByApplicationKeyAndRoleTypeKey(_applicationKey, (int)OfferRoleTypes.BranchConsultantD, (int)GeneralStatuses.Active);

                        //if (currentBCRole != null)
                            //_oSR.DeactivateApplicationRole(currentBCRole.Key);

                        IApplicationRole bcAppRole = _oSR.GenerateApplicationRole((int)OfferRoleTypes.BranchConsultantD, _applicationKey, userSelected.LegalEntity.Key, true);
                        message = string.Format("Reassigned Commissionable Consultant, including Branch Consultant from {0} to {1}.", fromUser.ADUserName, toUser.ADUserName);

                        // Do X2 Stuff
                        if (currentBCRole == null)
                            _oSR.CreateWorkflowAssignment(bcAppRole, (int)_instanceID, GeneralStatuses.Inactive);
                        else if (_oSR.HasActiveWorkflowAssignment((int)_instanceID, currentBCRole))
                            _oSR.CreateWorkflowAssignment(bcAppRole, (int)_instanceID, GeneralStatuses.Active);
                        else
                            _oSR.CreateWorkflowAssignment(bcAppRole, (int)_instanceID, GeneralStatuses.Inactive);
                    }
                }

                // Create Memo
                GenerateMemo();
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

        #endregion
    }
}
