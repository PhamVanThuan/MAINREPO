using System;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using Castle.ActiveRecord;
using System.Collections.Generic;
using SAHL.Common.CacheData;
using SAHL.Common.Exceptions;

namespace SAHL.Web.Views.Common.Presenters.ValuationDetails
{
    /// <summary>
    /// Valuation Details Update Presenter
    /// </summary>
    public class ValuationDetailsUpdateOrig : ValuationDetailsBase
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public ValuationDetailsUpdateOrig(Interfaces.IValuationManualDetailsView view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        /// <summary>
        /// OnView Initialised event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!View.ShouldRunPage) return;

            if (!_view.IsPostBack)
            {
                GlobalCacheData.Remove(ViewConstants.Properties);
                GlobalCacheData.Remove(ViewConstants.SelectedValuationKey);
                GlobalCacheData.Remove(ViewConstants.ValuationManual);
            }

            if (valSAHLManual != null)
            {
                _view.SetPresenter = "Update";
                _view.Properties = Properties;
                _view.ShowLabels = false;
                _view.ShowNavigationButtons = true;
            }
            else
                _view.HideAllPanels();

            _view.OnddlHOCRoofDescriptionIndexChanged += (_view_OnddlHOCRoofDescriptionIndexChanged);
            _view.OnSubmitButtonClicked += (_view_OnSubmitButtonClicked);
            _view.OnValuationDetailsClicked += (_view_OnValuationDetailsClicked);
            _view.OnCancelButtonClicked+= (_view_OnCancelButtonClicked);
        }

        /// <summary>
        /// On View PreRender event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!View.ShouldRunPage) return;

            if (valSAHLManual != null)
                _view.ShowButtons = true;

        }
        /// <summary>
        /// On Submit Button Clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void _view_OnSubmitButtonClicked(object sender, KeyChangedEventArgs e)
        {
            int key = Convert.ToInt32(e.Key);

            IPropertyRepository propRepo = RepositoryFactory.GetRepository<IPropertyRepository>();
            IValuationDiscriminatedSAHLManual val = propRepo.GetValuationByKey(key) as IValuationDiscriminatedSAHLManual;

            TransactionScope txn = new TransactionScope();

            try
            {
                val = _view.GetUpdatedValuation(val) as IValuationDiscriminatedSAHLManual;

                if (val != null)
                {
                    // Update HOC with the manual valuation 
                    ManualValuationHOCUpdate(val);

                    propRepo.SaveValuation(val);

                    txn.VoteCommit();

                    if (_view.IsValid)
                    {
                        GlobalCacheData.Remove(ViewConstants.Properties);
                        GlobalCacheData.Remove(ViewConstants.SelectedValuationKey);
                        GlobalCacheData.Remove(ViewConstants.ValuationManual);
                        _view.Navigator.Navigate("Cancel");
                    }
                }
            }

            catch (Exception)
            {
                txn.VoteRollBack();

                if (_view.IsValid)
                    throw;
            }
            finally
            {
                txn.Dispose();
            }
        }

        protected void _view_OnValuationDetailsClicked(object sender, KeyChangedEventArgs e)
        {

            GlobalCacheData.Remove(ViewConstants.Properties);
            GlobalCacheData.Add(ViewConstants.Properties, Properties, new List<ICacheObjectLifeTime>());

            GlobalCacheData.Remove(ViewConstants.SelectedValuationKey);
            GlobalCacheData.Add(ViewConstants.SelectedValuationKey, Convert.ToInt32(e.Key), new List<ICacheObjectLifeTime>());

            GlobalCacheData.Remove(ViewConstants.ValuationManual);

            Navigator.Navigate("Next");
        }
        /// <summary>
        /// On HOC Roof selected index change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void _view_OnddlHOCRoofDescriptionIndexChanged(object sender, KeyChangedEventArgs e)
        {
            if ((Convert.ToInt32(e.Key) == (int)HOCRoofs.Conventional))
            {
                _view.SetHOCConventionalAmountForUpdate = true;
                _view.SetHOCThatchAmountForUpdate = false;
            }

            if ((Convert.ToInt32(e.Key) == (int)HOCRoofs.Thatch))
            {
                _view.SetHOCConventionalAmountForUpdate = false;
                _view.SetHOCThatchAmountForUpdate = true;
            }

            if ((Convert.ToInt32(e.Key) == (int)HOCRoofs.Partial))
            {
                _view.SetHOCConventionalAmountForUpdate = true;
                _view.SetHOCThatchAmountForUpdate = true;
            }

        }

        protected void _view_OnCancelButtonClicked(object sender, EventArgs e)
        {
            GlobalCacheData.Remove(ViewConstants.ValuationManual);
            GlobalCacheData.Remove(ViewConstants.Properties);
            GlobalCacheData.Remove(ViewConstants.SelectedValuationKey);

            _view.Navigator.Navigate("Cancel");
        }
    }
}
