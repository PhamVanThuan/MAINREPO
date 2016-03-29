using SAHL.Common.CacheData;
using SAHL.Common.Globals;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.ApplicationDomain.Queries;
using SAHL.V3.Framework;
using SAHL.V3.Framework.DomainServices;
using SAHL.Web.Views.AffordabilityAssessment.Interfaces;
using System;
using System.Linq;

namespace SAHL.Web.Views.AffordabilityAssessment.Presenters
{
    /// <summary>
    ///
    /// </summary>
    public class DetailsBase : SAHLCommonBasePresenter<IDetails>
    {
        private CBOMenuNode cboNode;
        protected IV3ServiceManager v3ServiceManager;
        protected IApplicationDomainService applicationDomainService;
        protected SAHLPrincipalCache spc;
        protected int affordabilityAssessmentKey;
        protected ISystemMessageCollection systemMessageCollection;

        /// <summary>
        ///
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public DetailsBase(IDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage)
                return;

            if (GlobalCacheData.ContainsKey(ViewConstants.AffordabilityAssessmentKey))
            {
                affordabilityAssessmentKey = Convert.ToInt32(GlobalCacheData[ViewConstants.AffordabilityAssessmentKey]);
                GlobalCacheData.Remove(ViewConstants.AffordabilityAssessmentKey);
            }
            else
            {
                cboNode = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;

                if (cboNode == null)
                    throw new NullReferenceException(StaticMessages.NullCurrentCBONode);

                affordabilityAssessmentKey = cboNode.GenericKey;
            }

            spc = SAHLPrincipalCache.GetPrincipalCache(_view.CurrentPrincipal);

            v3ServiceManager = V3ServiceManager.Instance;
            applicationDomainService = v3ServiceManager.Get<IApplicationDomainService>();

            GetAffordabilityAssessmentByKeyQuery getAffordabilityAssessmentByKeyQuery = new GetAffordabilityAssessmentByKeyQuery(affordabilityAssessmentKey);
            systemMessageCollection = applicationDomainService.PerformQuery(getAffordabilityAssessmentByKeyQuery);
            if (!systemMessageCollection.HasErrors)
            {
                if (getAffordabilityAssessmentByKeyQuery.Result != null)
                {
                    _view.AffordabilityAssessment = getAffordabilityAssessmentByKeyQuery.Result.Results.FirstOrDefault();
                }
            }
            else
            {
                v3ServiceManager.HandleSystemMessages(systemMessageCollection);
            }
        }
    }
}