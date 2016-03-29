using System;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Web.UI;
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
    /// Valuation Details Add
    /// </summary>
    public class ValuationDetailsAdd : ValuationDetailsBase
    {
        IValuationDiscriminatedSAHLManual valuationDiscriminatedSAHLManual;

        /// <summary>
        /// Valuation details add
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public ValuationDetailsAdd(Interfaces.IValuationManualDetailsView view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }
        /// <summary>
        /// OnView Initialised
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!View.ShouldRunPage) return;

            _view.SetPresenter = "Add";
            _view.Properties = Properties;
            _view.ShowLabels = false;
            _view.ShowNavigationButtons = false;

            _view.OnddlHOCRoofDescriptionIndexChanged += (_view_OnddlHOCRoofDescriptionIndexChanged);
            _view.OnSubmitButtonClicked += (_view_OnSubmitButtonClicked);
            _view.OnBackButtonClicked += (_view_OnBackButtonClicked);
            _view.OnCancelButtonClicked += (_view_OnCancelButtonClicked);

        }



        /// <summary>
        /// OnView PreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!View.ShouldRunPage) return;

            _view.ShowButtons = true;

        }

        /// <summary>
        /// Submit button clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void _view_OnSubmitButtonClicked(object sender, KeyChangedEventArgs e)
        {
            IPropertyRepository propRepo = RepositoryFactory.GetRepository<IPropertyRepository>();
            TransactionScope txn = new TransactionScope();

            try
            {
                if (GlobalCacheData.ContainsKey(ViewConstants.ValuationManual))
                    valuationDiscriminatedSAHLManual = GlobalCacheData[ViewConstants.ValuationManual] as IValuationDiscriminatedSAHLManual;

                valuationDiscriminatedSAHLManual = _view.GetUpdatedValuation(valuationDiscriminatedSAHLManual) as IValuationDiscriminatedSAHLManual;

                if (valuationDiscriminatedSAHLManual != null)
                {
                    // set the property value here so its in the same session
                    valuationDiscriminatedSAHLManual.Property = _properties[0];

                    valuationDiscriminatedSAHLManual.IsActive = true;

                    // perform validation & save if valid
                    if (valuationDiscriminatedSAHLManual.ValidateEntity())
                    {

                        // Update HOC with the manual valuation 
                        ManualValuationHOCUpdate(valuationDiscriminatedSAHLManual);

                        // NB !! The SaveValuation must happen after the HOC
                        //       
                        // save the valuation - this will update all previous valuations to inactive
                        propRepo.SaveValuation(valuationDiscriminatedSAHLManual);

                        txn.VoteCommit();

                        GlobalCacheData.Remove(ViewConstants.ValuationManual);
                        Navigator.Navigate("Submit");
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
        /// <summary>
        /// HOC Roof selected index changed
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

        protected virtual void _view_OnCancelButtonClicked(object sender, EventArgs e)
        {
            GlobalCacheData.Remove(ViewConstants.ValuationManual);
            Navigator.Navigate("Cancel");
        }

        protected void _view_OnBackButtonClicked(object sender, EventArgs e)
        {
            GlobalCacheData.Remove(ViewConstants.ValuationPresenter);
            GlobalCacheData.Add(ViewConstants.ValuationPresenter, _view.ViewName, new List<ICacheObjectLifeTime>());

            Navigator.Navigate("Back");
        }

    }
}
