using System;
using System.Collections.Generic;
using SAHL.Common.Web.UI;
using SAHL.Common.Globals;
using SAHL.Common.CacheData;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.DomainMessages;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Web.Views.Origination.Interfaces;

namespace SAHL.Web.Views.Origination.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class LoanCalculator : LoanCalculatorBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public LoanCalculator(ILoanCalculator view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        /// <summary>
        /// Hook the events fired by the view and call relevant methods to bind control data
        /// </summary>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage)
                return;

            //_view.OnCalculateButtonClicked += new EventHandler(_view_OnCalculateButtonClicked);
            _view.OnCancelButtonClicked += new EventHandler(_view_OnCancelButtonClicked);
            _view.OnCreateApplicationButtonClicked += new EventHandler(_view_OnCreateApplicationButtonClicked);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void _view_OnCreateApplicationButtonClicked(object sender, EventArgs e)
        {
            CreateNewApplication();

            if (_view.IsValid)
            {
                IList<ICacheObjectLifeTime> lifeTimes = new List<ICacheObjectLifeTime>();

                if (GlobalCacheData.ContainsKey(ViewConstants.CreateApplication))
                    GlobalCacheData.Remove(ViewConstants.CreateApplication);

                if (GlobalCacheData.ContainsKey(ViewConstants.EstateAgentApplication))
                    GlobalCacheData.Remove(ViewConstants.EstateAgentApplication);

                GlobalCacheData.Add(ViewConstants.CreateApplication, (IApplication)_app, lifeTimes);
                GlobalCacheData.Add(ViewConstants.EstateAgentApplication, _view.IsEstateAgentApplication.ToString(), lifeTimes);

                Navigator.Navigate("App_LegalEntityDetailsAdd");
            }
        }
    }
}
