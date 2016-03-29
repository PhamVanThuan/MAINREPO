namespace SAHL.Web.Views.Origination.Presenters
{
    using System;
    using System.Collections.Generic;
    using SAHL.Common;
    using SAHL.Common.BusinessModel.Interfaces;
    using SAHL.Common.BusinessModel.Interfaces.Repositories;
    using SAHL.Common.Factories;
    using SAHL.Common.Globals;
    using SAHL.Common.Service.Interfaces;
    using SAHL.Common.UI;
    using SAHL.Common.Web.UI;
    using SAHL.Web.Views.Origination.Interfaces;

    public class DisplayNetLeadXML : SAHLCommonBasePresenter<IDisplayNetLeadXML>
    {
        IApplicationRepository appRepo;
        long instanceID = 0;
        string stateName;

        public DisplayNetLeadXML(IDisplayNetLeadXML view, SAHLCommonBaseController controller)
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

            ILeadInputInformation leadInputInformation = null;
            appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();

            InstanceNode node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal, CBONodeSetType.X2) as InstanceNode;

            if (node == null)
                throw new NullReferenceException(StaticMessages.NullCurrentCBONode);

            if (node.X2Data != null)
            {
                instanceID = node.InstanceID;
                stateName = node.StateName;
                Dictionary<string, object> _x2Data = node.X2Data as Dictionary<string, object>;

                if (!string.IsNullOrEmpty(_x2Data["NetLeadXML"].ToString()))
                    leadInputInformation = appRepo.DeserializeNetLeadXML(_x2Data["NetLeadXML"].ToString());
            }

            _view.BindRawNetLeadXML(leadInputInformation);
            _view.BindRetryButtonText(stateName);
            _view.OnRetryCreateClicked += new EventHandler(_view_OnRetryCreateClicked);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _view_OnRetryCreateClicked(object sender, EventArgs e)
        {
            if (_view.Messages.ErrorMessages.Count == 0 && instanceID > 0
                && (stateName == "Assign TeleConsultant" || stateName == "InternetCreate"))
            {
                IX2Service x2svc = ServiceFactory.GetService<IX2Service>();
                X2ServiceResponse Rsp = null;

                if (stateName == "Assign TeleConsultant")
                    x2svc.StartActivity(_view.CurrentPrincipal, instanceID, SAHL.Common.Constants.WorkFlowActivityName.RetryAssignTeleConsultant, null, false);
                else if (stateName == "InternetCreate")
                    x2svc.StartActivity(_view.CurrentPrincipal, instanceID, SAHL.Common.Constants.WorkFlowActivityName.RetryInternetCreate, null, false);

                Rsp = x2svc.CompleteActivity(_view.CurrentPrincipal, null, false);

                if (Rsp.IsError)
                    x2svc.CancelActivity(_view.CurrentPrincipal);
                else
                    x2svc.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
            }
        }
    }
}