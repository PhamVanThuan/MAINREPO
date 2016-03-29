using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.DomainMessages;
using SAHL.Common.Factories;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.DebtCounselling.Interfaces;

namespace SAHL.Web.Views.DebtCounselling.Presenters
{
    public class AssignCaseBase : SAHLCommonBasePresenter<IAssignCase>
    {
        private CBOMenuNode node;
        private InstanceNode instanceNode;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public AssignCaseBase(IAssignCase view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        /// <summary>
        /// OnView Initialised event - retrieve data for use by presenters
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage) return;

            _view.SubmitClick += new EventHandler<EventArgs>(OnSubmitClick);
            _view.CancelClick += new EventHandler<EventArgs>(OnCancelClick);

            List<SAHL.Common.Globals.WorkflowRoleTypes> workflowRoletypes = new List<SAHL.Common.Globals.WorkflowRoleTypes>();

            string l = _view.ViewAttributes["workflowroletypekeys"];
            _view.Message = "";

            string[] roles = l.Split(',');
            for (int x = 0; x < roles.Length; x++)
            {
                int key = Convert.ToInt32(roles[x]);
                workflowRoletypes.Add((SAHL.Common.Globals.WorkflowRoleTypes)key);
            }

            IOrganisationStructureRepository _orgRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
            DataTable dtUsers = _orgRepo.GetUsersForWorkflowRoleType(workflowRoletypes);

            node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;

            string assignADUserName = string.Empty;
            if (node is InstanceNode)
            {
                instanceNode = node as InstanceNode;
                assignADUserName = instanceNode.X2Data["AssignADUserName"].ToString();
                if (!string.IsNullOrEmpty(assignADUserName))
                {
                    DataRow[] rows = dtUsers.Select(@"ADUser like '" + assignADUserName + "|%'");
                    if (rows.Length == 1)
                        assignADUserName = rows[0][0].ToString();
                    else
                        assignADUserName = string.Empty;
                }
                else
                    assignADUserName = string.Empty;
            }

            _view.BindUsers(dtUsers, assignADUserName);
        }

        /// <summary>
        /// On Cancel Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnCancelClick(object sender, EventArgs e)
        {
            IX2Service svc = ServiceFactory.GetService<IX2Service>();
            svc.CancelActivity(_view.CurrentPrincipal);
            svc.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
        }

        /// <summary>
        /// On Submit Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnSubmitClick(object sender, EventArgs e)
        {

            IX2Service svc = ServiceFactory.GetService<IX2Service>();
            string message = string.Empty;

            try
            {
                
                //Write userid to datatable
                //DataRow row = (DataRow) _view.UserSelected;
                ListItem SelectedUser = _view.UserSelected;
                string[] values = SelectedUser.Value.Split('|');
                if (values.Length == 2)
                {
                    string adusername = values[0];
                    int workflowroletypekey = Convert.ToInt32(values[1]);

                    Dictionary<string, string> x2Data = new Dictionary<string, string>();
                    x2Data.Add("AssignADUserName", adusername);
                    x2Data.Add("AssignWorkflowRoleTypeKey", workflowroletypekey.ToString());

                    if (x2Data != null)
                    {
                        svc.CompleteActivity(_view.CurrentPrincipal, x2Data, false, message);
                        svc.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
                    }
                }
                else
                {
                    _view.Messages.Add(new Error("Please select a User","Please select a User"));
                }
               
            }
            catch (Exception)
            {
                if (_view.IsValid)
                {
                    svc.CancelActivity(_view.CurrentPrincipal);
                    throw;
                }
            }


        }
    }
}