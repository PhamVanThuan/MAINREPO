using System;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DomainMessages;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Administration.Interfaces;
using SAHL.Web.Views.Common.Interfaces;

namespace SAHL.Web.Views.Administration.Presenters.MarketingSource
{
    public class MarketingSourceUpdate : MarketingSourceBase
    {
        public MarketingSourceUpdate(IMarketingSource View, SAHLCommonBaseController Controller)
            : base(View, Controller)
        {
            return;
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage) return;

            _view.CancelClick += new EventHandler(CancelClick);
            _view.SubmitClick += new EventHandler(SubmitClick);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!View.ShouldRunPage)
                return;

            bindDisplay();
            _view.SubmitButtonVisible = true;
            _view.CancelButtonVisible = true;
            _view.UpdatePanelVisible = true;
            _view.SubmitButtonText = "Update";
        }

        private void bindDisplay()
        {
            int marketingSourceKey = _view.SelectedMarketingSourceKey;
            if (marketingSourceKey >= 0)
            {
                IApplicationSource marketingSource = MarketingSourceRepository.GetMarketingSourceByKey(marketingSourceKey);
                if (marketingSource != null)
                {
                    _view.txtMarketingSourceDescriptionText = marketingSource.Description;
                    _view.ddlMarketingSourceStatusSelected = marketingSource.GeneralStatus.Key.ToString();
                }
            }
        }

        protected void CancelClick(object sender, EventArgs e)
        {
            _view.Navigator.Navigate("MarketingSourceDisplay");
        }

        protected void SubmitClick(object sender, EventArgs e)
        {
            int marketingSourceKey = _view.SelectedMarketingSourceKey;
            IApplicationSource appSource = MarketingSourceRepository.GetMarketingSourceByKey(marketingSourceKey);

            string strDescription = _view.txtMarketingSourceDescriptionText;
            if (strDescription.Trim().Length == 0)
            {
                _view.Messages.Add(new Error("The Marketing Source description cannot be blank.", "The Marketing Source description cannot be blank."));
                return;
            }
            //if (MarketingSourceRepository.ApplicationSourceExists(strDescription))
            //{
            //    _view.Messages.Add(new Error("The Marketing Source you entered already exists.", "The Marketing Source you entered already exists."));
            //    return;
            //}

            int iGeneralStatus = Convert.ToInt32(_view.ddlMarketingSourceStatusSelected);
            ILookupRepository lookupRepository = RepositoryFactory.GetRepository<ILookupRepository>();
            if (iGeneralStatus == (int)GeneralStatuses.Inactive)
            {
                IGeneralStatus InactiveGeneralStatus = lookupRepository.GeneralStatuses[GeneralStatuses.Inactive];
                appSource.GeneralStatus = InactiveGeneralStatus;
            }
            else
            {
                IGeneralStatus activeGeneralStatus = lookupRepository.GeneralStatuses[GeneralStatuses.Active];
                appSource.GeneralStatus = activeGeneralStatus;
            }

            appSource.Description = strDescription;

            if (marketingSourceKey > 0)
            {
                MarketingSourceRepository.SaveApplicationSource(appSource);
            }

            //rebind the data
            IReadOnlyEventList<IApplicationSource> marketingSource = MarketingSourceRepository.GetMarketingSources();
            _view.BindMarketingSourcesGrid(marketingSource);

            // refresh the cache data
            lookupRepository.ResetLookup(LookupKeys.ApplicationSources);
            IX2Service x2Service = ServiceFactory.GetService<IX2Service>();
            x2Service.ClearLookup(((int)LookupKeys.ApplicationSources).ToString());
        }
    }
}