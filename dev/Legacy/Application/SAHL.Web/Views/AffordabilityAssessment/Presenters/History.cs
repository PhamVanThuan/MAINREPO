using SAHL.Common.CacheData;
using SAHL.Common.Globals;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
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
    public class History : SAHLCommonBasePresenter<IHistory>
    {
        private IEnumerable<AffordabilityAssessmentSummaryModel> affordabilityAssessments;
        private IV3ServiceManager v3ServiceManager = V3ServiceManager.Instance;
        private IApplicationDomainService applicationDomainService;

        private IList<ICacheObjectLifeTime> _lifeTimes;

        public IList<ICacheObjectLifeTime> LifeTimes
        {
            get
            {
                if (_lifeTimes == null)
                    _lifeTimes = new List<ICacheObjectLifeTime>();

                return _lifeTimes;
            }
        }

        #region Constructor

        /// <summary>
        ///
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public History(IHistory view, SAHLCommonBaseController controller)
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

            _view.OnViewAssessmentButtonClicked += new EventHandler(OnViewAssessmentButtonClicked);

            v3ServiceManager = V3ServiceManager.Instance;
            applicationDomainService = v3ServiceManager.Get<IApplicationDomainService>();

            switch (cboNode.GenericKeyTypeKey)
            {
                case (int)GenericKeyTypes.Offer:
                    affordabilityAssessments = applicationDomainService.GetHistoricalAffordabilityAssessmentsForApplication(cboNode.GenericKey);
                    _view.ShowApplicationKeyInGrid = false;
                    _view.ShowGeneralStatusInGrid = false;
                    _view.AffordabilityAssessmentGridHeaderCaption = "Historical Affordability Assessments";
                    break;
                case (int)GenericKeyTypes.Account:
                case (int)GenericKeyTypes.ParentAccount:
                    affordabilityAssessments = applicationDomainService.GetAffordabilityAssessmentsForAccount(cboNode.GenericKey);
                    _view.ShowApplicationKeyInGrid = true;
                    _view.AffordabilityAssessmentGridHeaderCaption = "Affordability Assessments";
                    _view.ShowGeneralStatusInGrid = true;
                    break;
                default:
                    throw new NotSupportedException(String.Format("The current generic key type key is not supported: {0}", cboNode.GenericKeyTypeKey));
            }

            _view.BindAffordabilityAssessmentsGrid(affordabilityAssessments);
        }

        protected virtual void OnViewAssessmentButtonClicked(object sender, EventArgs e)
        {
            GlobalCacheData.Clear();

            if (_view.SelectedAffordabilityAssessmentKey > 0)
            {
                GlobalCacheData.Add(ViewConstants.AffordabilityAssessmentKey, _view.SelectedAffordabilityAssessmentKey, LifeTimes);
                _view.Navigator.Navigate("View");

            }
        }
    }
}