using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.Cap.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Controls;
using System.Collections;
using SAHL.Common.BusinessModel.Interfaces;
using System.Collections.Generic;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Service;
using SAHL.Common.Factories;
using SAHL.Common.Exceptions;
using SAHL.Common.Globals;
using SAHL.Common.CacheData;

namespace SAHL.Web.Views.Cap.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class CapOfferSalesRecalculate : CapOfferSalesBase
    {
        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public CapOfferSalesRecalculate(ICapOfferSales view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage) return;

            _view.PaymentOptionDropDownVisible = false;
            LoadCapOffer();

            BindInstalmentValues("Instalment on Quote");
            _view.ReasonDropDownVisible = false;
            _view.PromotionCheckBoxEnabled = false;
            _view.ButtonRowVisible = true;
            _view.CapGridPostBackType = GridPostBackType.None;
            _view.SubmitButtonText = "Confirm";
            _view.ConfirmMessageText = "Are you sure you want to Recalculate the CAP offer?";
            _view.CancelButtonVisible = false;

            _view.OnCancelButtonClicked += new EventHandler(_view_OnCancelButtonClicked);
            _view.OnSubmitButtonClicked += new EventHandler(_view_OnSubmitButtonClicked);

            IList<ICapApplicationDetail> detailList = new List<ICapApplicationDetail>();
            for (int i = 0; i < _capOffer.CapApplicationDetails.Count; i++)
            {
                if (_capOffer.CapApplicationDetails[i].CapStatus.Key == _capOffer.CapStatus.Key)
                    detailList.Add(_capOffer.CapApplicationDetails[i]);
            }
            _view.BindCapGrid(detailList);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _view_OnCancelButtonClicked(object sender, EventArgs e)
        {
            _view.Navigator.Navigate("Cancel");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _view_OnSubmitButtonClicked(object sender, EventArgs e)
        {
            if (_capOffer != null)
            {
                ILookupRepository _lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();
                _capOffer.CapStatus = _lookupRepo.CapStatuses.ObjectDictionary[Convert.ToInt32(CapStatuses.Recalculated).ToString()];
                for (int i = 0; i < _capOffer.CapApplicationDetails.Count; i++)
                {
                    _capOffer.CapApplicationDetails[i].CapStatus = _lookupRepo.CapStatuses.ObjectDictionary[Convert.ToInt32(CapStatuses.Recalculated).ToString()];
                }

                // add the cap offer to the global cached so that the add screen can commit the changes
                if (GlobalCacheData.ContainsKey("RecalculatedCapOffer"))
                    GlobalCacheData.Remove("RecalculatedCapOffer");
                IList<ICacheObjectLifeTime> LifeTimes = new List<ICacheObjectLifeTime>();
                GlobalCacheData.Add("RecalculatedCapOffer", _capOffer, LifeTimes);
                //Need to navigate to the add screen
                _view.Navigator.Navigate("Add");

            }            
        }
    }
}