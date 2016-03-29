using System;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces;
using Castle.ActiveRecord;
using SAHL.Common.CacheData;
using System.Collections.Generic;

namespace SAHL.Web.Views.Common.Presenters.ManualValuationMainDwellingDetails
{
    public class MainDwellingDetailDisplay : MainDwellingDetailsBase
    {
        protected string presenterName;
        protected IList<ICacheObjectLifeTime> LifeTimes;
          /// <summary>
        /// ValuationDwellingDetail Display constructor
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public MainDwellingDetailDisplay(IValuationManualPropertyDetailsView view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage) 
                return;

            _view.ShowExtendedDetailsGrid = false;
            _view.ShowReadOnlyFieldsForDisplay();

			_view.Property = _property;

            _view.BindValuationDisplay(_valManual, _valMainBuilding, _valCottage, _valCombinedThatchValue);

            presenterName = GlobalCacheData.ContainsKey(ViewConstants.ValuationPresenter) ? GlobalCacheData[ViewConstants.ValuationPresenter].ToString() : _view.ViewName;

            switch (presenterName)
            {
                case "ManualValuationsMainDwellingExtendedDetailsUpdate":
                case "Orig_ManualValuationsMainDwellingExtendedDetailsUpdate":
                    _view.ShowUpdateButton();
                    break;

                case "ManualValuationsMainDwellingExtendedDetailsAdd":
                case "Orig_ManualValuationsMainDwellingExtendedDetailsAdd":
                case "ValuationDetailsAdd":
                    _view.ShowBackButton();
                    break;

                default:
                    break;
            }

            _view.OnCancelButtonClicked += _view_OnCancelButtonClicked;
            _view.OnNextButtonClicked += _view_OnNextButtonClicked;
            _view.OnAddButtonClicked += _view_OnAddButtonClicked;
            _view.OnBackButtonClicked += _view_OnBackButtonClicked; 
        }

        protected virtual void _view_OnCancelButtonClicked(object sender, EventArgs e)
        {
            Navigator.Navigate("Cancel");
        }

        protected virtual void _view_OnNextButtonClicked(object sender, EventArgs e)
        {
            switch (presenterName)
            {
                case "ManualValuationsMainDwellingExtendedDetailsAdd":
                case "Orig_ManualValuationsMainDwellingExtendedDetailsAdd":
                case "ValuationDetailsAdd":
                case "Orig_ValuationDetailsAdd":
                    Navigator.Navigate("Add");
                    break;
                default:
                    Navigator.Navigate("Display");
                    break;
            }
        }

        protected virtual void _view_OnBackButtonClicked(object sender, EventArgs e)
        {
            GlobalCacheData.Remove(ViewConstants.ValuationPresenter);
            GlobalCacheData.Add(ViewConstants.ValuationPresenter, _view.ViewName, LifeTimes);

            switch (presenterName)
            {
                case "ValuationDetailsAdd":
                case "Orig_ValuationDetailsAdd":
                    Navigator.Navigate("Back");
                    break;
                default:
                    Navigator.Navigate(presenterName);
                    break;
            }
        }

        protected virtual void _view_OnAddButtonClicked(object sender, EventArgs e)
        {
            TransactionScope txn = new TransactionScope();
            IValuationDiscriminatedSAHLManual valSAHLManualExistingRec = _propRepo.GetValuationByKey(_valManual.Key) as IValuationDiscriminatedSAHLManual;

            // if we have remove the cottage valuation, then flag for delete
            bool removeValuationCottage = valSAHLManualExistingRec.ValuationCottage != null && _valCottage == null ? true : false;
            
            // if we have remove the combined thatch valuation, then flag for delete
            bool removeValuationCombinedThatch = valSAHLManualExistingRec.ValuationCombinedThatch != null && _valCombinedThatchValue <= 0 ? true : false;

            CopyCacheValuesToTarget(valSAHLManualExistingRec);

            // set the property value here so its in the same session
            valSAHLManualExistingRec.Property = _property;

            try
            {
                _propRepo.SaveValuation(valSAHLManualExistingRec);

                if (removeValuationCottage)
                    _propRepo.DeleteValuationCottage(valSAHLManualExistingRec.Key);
                if (removeValuationCombinedThatch)
                    _propRepo.DeleteValuationCombinedThatch(valSAHLManualExistingRec.Key);

                txn.VoteCommit();

                ClearCache();
                
                Navigator.Navigate("Cancel");
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

    }
}
