using SAHL.Common.CacheData;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;
using SAHL.V3.Framework;
using SAHL.V3.Framework.DomainServices;
using SAHL.Web.Views.AffordabilityAssessment.Interfaces;
using System;
using System.Collections.Generic;

namespace SAHL.Web.Views.AffordabilityAssessment.Presenters
{
    /// <summary>
    ///
    /// </summary>
    public class Summary : SAHLCommonBasePresenter<ISummary>
    {
        private IV3ServiceManager v3ServiceManager = V3ServiceManager.Instance;
        private IApplicationDomainService applicationDomainService;

        #region Constructor

        /// <summary>
        ///
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public Summary(ISummary view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        #endregion Constructor

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!View.ShouldRunPage) return;

            CBOMenuNode cboNode = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            if (cboNode == null)
            {
                throw new NotSupportedException("CBONode can not be null");
            }
            
            if (cboNode.GenericKeyTypeKey != (int)SAHL.Common.Globals.GenericKeyTypes.Offer)
            {
                throw new NotSupportedException(String.Format("The current generic key type key is not supported: {0}", cboNode.GenericKeyTypeKey));
            }

            v3ServiceManager = V3ServiceManager.Instance;
            applicationDomainService = v3ServiceManager.Get<IApplicationDomainService>();

            int applicationKey = cboNode.GenericKey;

            _view.BindAffordabilityAssessmentsGrid(applicationDomainService.GetAffordabilityAssessmentsForApplication(applicationKey));
        }
    }
}