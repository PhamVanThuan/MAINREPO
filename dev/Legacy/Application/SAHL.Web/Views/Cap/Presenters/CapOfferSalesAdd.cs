using System;
using SAHL.Web.Views.Cap.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.BusinessModel.Interfaces;
using System.Collections.Generic;
using Castle.ActiveRecord;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Service;
using SAHL.Common.Factories;
using SAHL.Common.Exceptions;
using SAHL.Common.CacheData;
using SAHL.X2.Framework.Interfaces;

namespace SAHL.Web.Views.Cap.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class CapOfferSalesAdd : CapOfferSalesBase
    {
        #region Private Variables

        private bool _recalculatingOffer;

        #endregion

        #region Constructor

        public CapOfferSalesAdd(ICapOfferSales view, SAHLCommonBaseController controller)
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

            if (GlobalCacheData.ContainsKey("RecalculatedCapOffer"))
            {
                _recalculatingOffer = true;
                _view.CancelButtonVisible = false;
            }
            else
            {
                _recalculatingOffer = false;
            }

            CreateCapOffer(_recalculatingOffer);
            BindCapPaymentOptions();
            _view.PaymentOptionDropDownVisible = false;
            _view.ReasonDropDownVisible = false;
            _view.PromotionCheckBoxEnabled = false;
            _view.ButtonRowVisible = true;
            _view.CapGridPostBackType = GridPostBackType.None;
            _view.SubmitButtonText = "Add Offer";
            _view.ConfirmMessageText = "Are you sure you want to Add the CAP offer?";

            _view.OnCancelButtonClicked += new EventHandler(_view_OnCancelButtonClicked);
            _view.OnSubmitButtonClicked += new EventHandler(_view_OnSubmitButtonClicked);

            IList<ICapApplicationDetail> detailList = new List<ICapApplicationDetail>();
            if (_capOffer != null)
            {
                for (int i = 0; i < _capOffer.CapApplicationDetails.Count; i++)
                {
                    detailList.Add(_capOffer.CapApplicationDetails[i]);
                }
            }
            _view.BindCapGrid(detailList);

            if (detailList.Count == 0)
                _view.SubmitButtonEnabled = false;

            //if (_view.Messages.Count > 0)
            //    _view.SubmitButtonEnabled = false;
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
            _view.Messages.Clear();
            X2Service svc = ServiceFactory.GetService<IX2Service>() as X2Service;
            TransactionScope txn = new TransactionScope(TransactionMode.Inherits);
            Dictionary<string, string> inputFields = new Dictionary<string, string>();
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(_view.CurrentPrincipal);
            IX2Info x2Info = spc.X2Info as IX2Info;

            try
            {

                DateTime capExpire = DateTime.Now;
                _capRepo.SaveCapApplication(_capOffer);
                ICapTypeConfiguration capType = _capOffer.CapTypeConfiguration;
                if (capType != null)
                    capExpire = capType.ApplicationEndDate;

                inputFields.Add("CapOfferKey", _capOffer.Key.ToString());
                inputFields.Add("CapBroker", _view.CurrentPrincipal.Identity.Name);
                inputFields.Add("LegalEntityName", _LegalEntityName);
                inputFields.Add("AccountKey", _capOffer.Account.Key.ToString());
                inputFields.Add("CapExpireDate", capExpire.ToString());
                inputFields.Add("CapStatusKey", _capOffer.CapStatus.Key.ToString());

                if (_view.IsValid)
                    txn.VoteCommit();
                else
                    txn.VoteRollBack();
            }
            catch (Exception)
            {
                txn.VoteRollBack();
                if (_view.IsValid)
                    throw;
            }
            finally
            {
                if (GlobalCacheData.ContainsKey("RecalculatedCapOffer"))
                    GlobalCacheData.Remove("RecalculatedCapOffer");

                txn.Dispose();
            }

            if (_view.IsValid)
            {
				try
				{
					if (_recalculatingOffer == false)
					{
						svc.CreateWorkFlowInstance(_view.CurrentPrincipal, SAHL.Common.Constants.WorkFlowName.Cap2Offers, (-1).ToString(), SAHL.Common.Constants.WorkFlowProcessName.Cap2Offers, "Create CAP2 lead", inputFields, false);
						x2Info = spc.X2Info as IX2Info;
						if (spc.X2Info != null)
							X2Service.CreateCompleteActivity(_view.CurrentPrincipal, null, false, null);
					}
					else
					{
						x2Info = spc.X2Info as IX2Info;
						if (spc.X2Info != null)
							svc.CompleteActivity(_view.CurrentPrincipal, inputFields, false);
					}

					// add the instanceID to the global cache for our redirect view to use
					GlobalCacheData.Remove(ViewConstants.InstanceID);
					GlobalCacheData.Add(ViewConstants.InstanceID, x2Info.InstanceID, new List<ICacheObjectLifeTime>());

					// navigate to the workflow redirect view
					Navigator.Navigate("X2InstanceRedirect");
				}
				catch (Exception ex)
				{
					if (_view.IsValid)
					{
						throw;
					}
				}
            }
        }
    }
}