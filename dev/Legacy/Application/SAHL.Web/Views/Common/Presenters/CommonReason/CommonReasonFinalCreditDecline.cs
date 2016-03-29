using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.UI;
using SAHL.Common.Factories;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Service;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.CacheData;
using System.Collections.Generic;
using SAHL.X2.Framework.Interfaces;
using SAHL.Common.X2.BusinessModel.Interfaces;

using SAHL.Common;

namespace SAHL.Web.Views.Common.Presenters.CommonReason
{
    /// <summary>
    /// 
    /// </summary>
    public class CommonReasonFinalCreditDecline : CommonReasonBase
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public CommonReasonFinalCreditDecline(ICommonReason view, SAHLCommonBaseController controller)
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

            if (!_view.ShouldRunPage) return;

            // get the instance node
            InstanceNode node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal, CBONodeSetType.X2) as InstanceNode;
                
            int instanceID = int.Parse(node.InstanceID.ToString());
            IX2Repository x2r = RepositoryFactory.GetRepository<IX2Repository>();
            IInstance instance = x2r.GetInstanceByKey(instanceID);
            IApplicationRepository ar = RepositoryFactory.GetRepository<IApplicationRepository>();
            IApplication app = ar.GetApplicationFromInstance(instance);
            //IApplicationInformation appInfo = app.GetLatestApplicationInformation();
            base.GenericKey = app.Key;

            _view.CancelButtonVisible = true;
        }

        /// <summary>
        /// Overrides the base OnSubmitButtonClicked event so that specific credit decline actions can be performed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void _view_OnSubmitButtonClicked(object sender, SAHL.Common.Web.UI.Events.KeyChangedEventArgs e)
        {          
            base._view_OnSubmitButtonClicked(sender, e);
            CompleteActivityAndNavigate();
        }

        public override void CancelActivity()
        {
            base.X2Service.CancelActivity(_view.CurrentPrincipal);
            base.X2Service.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);

        }

        public override void CompleteActivityAndNavigate()
        {
            X2ServiceResponse rsp = base.X2Service.CompleteActivity(_view.CurrentPrincipal, null, false);
            if (base.sdsdgKeys.Count > 0)
            {
                UpdateReasonsWithStageTransitionKey();
            }
            base.X2Service.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
        }
    }
}
