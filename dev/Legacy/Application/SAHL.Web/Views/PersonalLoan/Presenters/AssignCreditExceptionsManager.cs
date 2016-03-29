using SAHL.Common;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;
using SAHL.Web.Views.PersonalLoan.Interfaces;
using System;

namespace SAHL.Web.Views.PersonalLoan.Presenters
{
    public class AssignCreditExceptionsManager : SAHLCommonBasePresenter<IAssignUser>
    {
        private CBOMenuNode node;
        private IWorkflowAssignmentRepository workflowRespository;
        private IX2Repository x2Repository;
        private string user;

        InstanceNode instanceNode;

        long instanceID;

        public AssignCreditExceptionsManager(IAssignUser view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            this.node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            this.instanceNode = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal, CBONodeSetType.X2) as InstanceNode;
            this.instanceID = instanceNode.InstanceID;
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!View.ShouldRunPage) return;

            _view.OnCancelButtonClicked += _view_OnCancelButtonClicked;
            _view.OnSubmitButtonClicked += _view_OnSubmitButtonClicked;
            _view.onSelectedUserChanged += _view_onSelectedUserChanged;

            x2Repository = RepositoryFactory.GetRepository<IX2Repository>();
            workflowRespository = RepositoryFactory.GetRepository<IWorkflowAssignmentRepository>();
            var users = workflowRespository.GetAdUsersByWorkflowRoleTypeKey((int)WorkflowRoleTypes.PLCreditExceptionsManagerD);

            View.BindUsers(users);
        }

        private void _view_onSelectedUserChanged(object sender, KeyChangedEventArgs e)
        {
            user = e.Key.ToString();
        }

        private void _view_OnSubmitButtonClicked(object sender, EventArgs e)
        {
            IX2Service svc = ServiceFactory.GetService<IX2Service>();

            svc.CompleteActivity(_view.CurrentPrincipal, null, false, user);
            svc.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
        }

        private void _view_OnCancelButtonClicked(object sender, EventArgs e)
        {
            IX2Service svc = ServiceFactory.GetService<IX2Service>();
            svc.CancelActivity(_view.CurrentPrincipal);
            svc.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
        }
    }
}