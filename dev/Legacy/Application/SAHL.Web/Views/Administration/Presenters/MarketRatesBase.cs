using System;
using SAHL.Web.Views.Administration.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.Factories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Views.Administration.Presenters
{
    /// <summary>
    /// Serves as a base presenter class for all presenters to do with the Market Rates admin screens.
    /// </summary>
    public class MarketRatesBase : SAHLCommonBasePresenter<IMarketRates>
    {
        public MarketRatesBase(IMarketRates view, SAHLCommonBaseController Controller)
            : base(view, Controller)
        {
        }

        #region Attributes

        private IMarketRateRepository _marketRateRepo;
        private ILookupRepository _lookupRepo;

        #endregion

        #region Members

        /// <summary>
        /// Gets a Market Rate repository for use in the presenter - this will be created the first time this property is called.
        /// </summary>
        protected IMarketRateRepository MarketRateRepository
        {
            get
            {
                if (_marketRateRepo == null)
                    _marketRateRepo = RepositoryFactory.GetRepository<IMarketRateRepository>();
                return _marketRateRepo;
            }
        }

        /// <summary>
        /// Gets a Market Rate repository for use in the presenter - this will be created the first time this property is called.
        /// </summary>
        protected ILookupRepository LookupRepository
        {
            get
            {
                if (_lookupRepo == null)
                    _lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();
                return _lookupRepo;
            }
        }

        #endregion

        #region EventHandlers

        #endregion

        #region Events

        /// <summary>
        /// Hook the events fired by the view and call relevant methods to bind control data
        /// </summary>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage)
                return;

            if (MarketRateRepository != null)
            {
                IReadOnlyEventList<IMarketRate> marketRates = MarketRateRepository.GetMarketRates();
                _view.BindMarketRatesGrid(marketRates);
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

            bindDisplay();
        }

        #endregion

        #region Methods

        private void bindDisplay()
        {
            int marketRateKey = _view.SelectedMarketRateKey;
            if (marketRateKey >= 0)
            {
                IMarketRate marketRate = MarketRateRepository.GetMarketRateByKey(marketRateKey);
                if (marketRate != null)
                {
                    _view.lblMarketRateKeyText = marketRate.Key.ToString();
                    _view.lblMarketRateDescriptionText = marketRate.Description.ToString();
                    _view.MarketRateValue = new double?((double)(marketRate.Value * 100));
                    _view.lblMarketRateValueText = marketRate.Value.ToString(SAHL.Common.Constants.RateFormat3Decimal);

                    _view.SubmitButtonEnabled = true;
                    _view.txtMarketRateValueEnabled = true;

                }
            }
        }

        #endregion
    }
}
