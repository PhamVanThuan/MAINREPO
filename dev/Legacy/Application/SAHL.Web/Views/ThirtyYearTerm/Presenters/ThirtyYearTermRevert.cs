using System;
using System.Data;
using System.Configuration;
using System.Web;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.ThirtyYearTerm.Interfaces;
using SAHL.Common.UI;
using SAHL.Common;
using SAHL.Common.Globals;
using Castle.ActiveRecord;
using SAHL.Common.Service.Interfaces;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.X2.Framework.Interfaces;
using SAHL.Common.CacheData;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;

namespace SAHL.Web.Views.ThirtyYearTerm.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class ThirtyYearTermRevert : SAHLCommonBasePresenter<IWorkFlowConfirm>
    {
        private InstanceNode node;
        private int applicationKey;
        private IApplicationRepository applicationRepo;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public ThirtyYearTermRevert(IWorkFlowConfirm view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage) return;

            // Get the CBO Node
            node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal, CBONodeSetType.X2) as InstanceNode;
            if (node == null)
            {
                throw new NullReferenceException(StaticMessages.NullCurrentCBONode);
            }

            applicationKey = node.GenericKey;


            base._view.OnYesButtonClicked += new EventHandler(_view_OnYesButtonClicked);
            base._view.OnNoButtonClicked += new EventHandler(_view_OnNoButtonClicked);

            base._view.ShowControls(true);
            IX2Info info = X2Service.GetX2Info(_view.CurrentPrincipal);

            if (info != null && !String.IsNullOrEmpty(info.ActivityName))
                _view.TitleText = String.Format("Confirm {0}", info.ActivityName);
            else
                _view.TitleText = "Revert to previous term";
        }

        void _view_OnNoButtonClicked(object sender, EventArgs e)
        {
            //cancel activity
            X2Service.CancelActivity(_view.CurrentPrincipal);
            //Navigate();
            this.X2Service.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
        }

        void _view_OnYesButtonClicked(object sender, EventArgs e)
        {
            SAHLPrincipalCache spc = null;
            try
            {
                var application = ApplicationRepo.GetApplicationByKey(applicationKey);
                ApplicationRepo.RevertToPreviousTermAsAcceptedApplication(application);
                spc = SAHLPrincipalCache.GetPrincipalCache(_view.CurrentPrincipal);
                this.X2Service.CompleteActivity(_view.CurrentPrincipal, null, spc.IgnoreWarnings);
                this.X2Service.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
            }
            catch (Exception)
            {
                var messages = spc.DomainMessages;
                foreach (var message in messages)
                {
                    System.Diagnostics.Trace.WriteLine(String.Format("{0} - {1}", message.Message, message.MessageType.ToString()));
                }
                // we must cancel the activity here, otherwise if the user navigates to another node and 
                // tries to perform a workflow action, X2 may try to perform the action on the wrong 
                // activity
                if (_view.IsValid)
                {
                    this.X2Service.CancelActivity(_view.CurrentPrincipal);
                    throw;
                }
            }
        }

        public IApplicationRepository ApplicationRepo
        {
            get
            {
                if (applicationRepo == null)
                {
                    applicationRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
                }
                return applicationRepo;
            }
        }
    }
}
