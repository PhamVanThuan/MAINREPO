using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.UI;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Web.Views.Administration.Interfaces;

namespace SAHL.Web.Views.Administration.Presenters.MarketingSource
{
    public class MarketingSourceBase : SAHLCommonBasePresenter<IMarketingSource>
    {
        private ILookupRepository lookups;
        private CBOMenuNode _node;
        /// <summary>
        /// 
        /// </summary>
        public CBOMenuNode MenuNode
        {
            get
            {
                return _node;
            }
            set
            {
                _node = value;
            }
        }

        private IMarketingSourceRepository _marketingSourceRepo;

        /// <summary>
        /// Gets a Market Source repository for use in the presenter - this will be created the first time this property is called.
        /// </summary>
        protected IMarketingSourceRepository MarketingSourceRepository
        {
            get
            {
                if (_marketingSourceRepo == null)
                    _marketingSourceRepo = RepositoryFactory.GetRepository<IMarketingSourceRepository>();
                return _marketingSourceRepo;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public MarketingSourceBase(IMarketingSource view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

                return;
        }

        /// <summary>
        /// Hook the events fired by the view and call relevant methods to bind control data
        /// </summary>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage)
                return;

            if (MarketingSourceRepository != null)
            {
                lookups = RepositoryFactory.GetRepository<ILookupRepository>();
                _view.BindStatusDropDown(lookups.GeneralStatuses.Values);

                IReadOnlyEventList<IApplicationSource> marketingSource = MarketingSourceRepository.GetMarketingSources();
                _view.BindMarketingSourcesGrid(marketingSource);
            }
            else
            {
                // TODO throw an exception GetRepository failed
            }
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

            //bindDisplay();
            _view.UpdatePanelVisible = false;
            _view.CancelButtonVisible = false;
            _view.SubmitButtonVisible = false;
        }

        //private void bindDisplay()
        //{
        //    int marketingSourceKey = _view.SelectedMarketingSourceKey;
        //    if (marketingSourceKey >= 0)
        //    {
        //        IApplicationSource marketingSource = MarketingSourceRepository.GetMarketingSourceByKey(marketingSourceKey);
        //        if (marketingSource != null)
        //        {
        //            _view.txtMarketingSourceDescriptionText = marketingSource.Description;
        //            _view.ddlMarketingSourceStatusSelected = marketingSource.GeneralStatus.Key.ToString();
        //            _view.UpdatePanelVisible = false;
        //            _view.CancelButtonVisible = false;
        //            _view.SubmitButtonVisible = false;
        //            //_view.lblMarketRateKeyText = marketRate.Key.ToString();
        //            //_view.lblMarketRateDescriptionText = marketRate.Description.ToString();
        //            //_view.MarketRateValue = new double?((double)(marketRate.Value * 100));
        //            //_view.lblMarketRateValueText = marketRate.Value.ToString(SAHL.Common.Constants.RateFormat3Decimal);

        //            //if (marketRate.Key == 4 || marketRate.Key == 5)
        //            //{
        //            //    _view.SubmitButtonEnabled = false;
        //            //    _view.txtMarketRateValueEnabled = false;
        //            //}
        //            //else
        //            //{
        //            //    _view.SubmitButtonEnabled = true;
        //            //    _view.txtMarketRateValueEnabled = true;
        //            //}
        //        }
        //    }
        //}

    }
}
