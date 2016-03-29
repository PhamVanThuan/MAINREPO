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
using SAHL.Common;

namespace SAHL.Web.Views.Common.Presenters.Reassign
{
    /// <summary>
    /// 
    /// </summary>
    public class ReassignUser : ReassignUserBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public ReassignUser(IReassignUser view, SAHLCommonBaseController controller)
            : base(view, controller)
        {             }

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

            if (PrivateCacheData.ContainsKey("InstanceID"))
                PrivateCacheData["InstanceID"] = "C";
            else
                PrivateCacheData.Add("InstanceID", "C");

            IEventList<IApplicationRole> lstAppRoles = _oSR.GetApplicationRolesByAppKey(_applicationKey, Thread.CurrentPrincipal.Identity.Name, _instanceID);
            _view.SetPostBackType = false;
            _view.SetPostBackTypeRole = true;
            _view.BindApplicationRoles(lstAppRoles);
            _view.ConsultantsRowVisible = false;
            _view.ShowRolesDropDown();
        }

        protected void _view_onSelectedRoleChanged(object sender, KeyChangedEventArgs e)
        {
            if (Convert.ToString(e.Key) != "-select-")
            {
                _appRole = _oSR.GetApplicationRoleByKey(Convert.ToInt32(e.Key));
                BindAssignToUsers();
            }
        }

        void _view_OnSubmitButtonClicked(object sender, EventArgs e)
        {
            // Add exclusion set
            this.ExclusionSets.Add(RuleExclusionSets.LegalEntityLeadApplicants);
            IX2Service svc = ServiceFactory.GetService<IX2Service>();
            string message = string.Empty;

            TransactionScope txn = new TransactionScope();
            try
            {
                _oSR.ReassignUser(_applicationKey, _instanceID, _view.SelectedConsultantKey, _appRole, out message);

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
                this.ExclusionSets.Remove(RuleExclusionSets.LegalEntityLeadApplicants);
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
