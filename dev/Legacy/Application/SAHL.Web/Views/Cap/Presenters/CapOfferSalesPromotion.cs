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
using SAHL.Common.Factories;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Service;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using Castle.ActiveRecord;
using SAHL.Common.Globals;
using SAHL.Common.Exceptions;

namespace SAHL.Web.Views.Cap.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class CapOfferSalesPromotion : CapOfferSalesBase
    {
        #region Private Variables 

        ICapApplicationDetail _cheapestOption;

        #endregion

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public CapOfferSalesPromotion(ICapOfferSales view, SAHLCommonBaseController controller)
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

            BindCapPaymentOptions();
            _view.PaymentOptionDropDownVisible = true;
            LoadCapOffer();

            BindInstalmentValues("Instalment on Quote");
            _view.ReasonDropDownVisible = false;
            _view.PromotionCheckBoxEnabled = false;
            _view.ButtonRowVisible = true;
            _view.CapGridPostBackType = GridPostBackType.NoneWithClientSelect;
            _view.SubmitButtonText = "Accept";
            _view.ConfirmMessageText = "Are you sure you want to Accept the CAP offer?";
            _view.CancelButtonVisible = false;

            _view.OnCancelButtonClicked += new EventHandler(_view_OnCancelButtonClicked);
            _view.OnSubmitButtonClicked += new EventHandler(_view_OnSubmitButtonClicked);

            IList<ICapApplicationDetail> detailList = new List<ICapApplicationDetail>();
            _cheapestOption = _capOffer.CapApplicationDetails[0];
            for (int i = 1; i < _capOffer.CapApplicationDetails.Count; i++)
            {
                if (_cheapestOption.Fee > _capOffer.CapApplicationDetails[i].Fee)
                    _cheapestOption = _capOffer.CapApplicationDetails[i];
            }
            detailList.Add(_cheapestOption);
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
            int detailKey = _cheapestOption.Key;
            //int promotion = 1; 
            if (_view.GridSelectedIndex != -1)
            {
                ILookupRepository lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();
                X2Service svc = ServiceFactory.GetService<IX2Service>() as X2Service;
                TransactionScope txn = new TransactionScope(TransactionMode.Inherits);
                try
                {
                    ValidateOnUpdate();
                    for (int i = 0; i < _capOffer.CapApplicationDetails.Count; i++)
                    {
                        if (_capOffer.CapApplicationDetails[i].Key == detailKey)
                        {
                            _capOffer.CapApplicationDetails[i].CapStatus = lookupRepo.CapStatuses.ObjectDictionary[Convert.ToInt32(CapStatuses.TakenUp).ToString()];
                        }
                        else
                        {
                            _capOffer.CapApplicationDetails[i].CapStatus = lookupRepo.CapStatuses.ObjectDictionary[Convert.ToInt32(CapStatuses.NotTakenUp).ToString()];
                        }
                    }
                    _capOffer.Promotion = true;
                    _capOffer.CapStatus = lookupRepo.CapStatuses.ObjectDictionary[Convert.ToInt32(CapStatuses.TakenUp).ToString()];
                    _capOffer.CAPPaymentOption = GetSelectedCapPaymentOption(_view.CapPaymentOptionSelectedValue);
                    _capRepo.SaveCapApplication(_capOffer);

                    Dictionary<string, string> inputFields = new Dictionary<string, string>();
                    inputFields.Add("CapOfferKey", _capOffer.Key.ToString());
                    inputFields.Add("CapStatusKey", _capOffer.CapStatus.Key.ToString());
                    inputFields.Add("Promotion", Convert.ToInt32(_capOffer.Promotion.Value).ToString());
                    inputFields.Add("CapPaymentOptionKey", _capOffer.CAPPaymentOption.Key.ToString());

                    if (_view.Messages.ErrorMessages.Count == 0)
                    {
                        //svc.CompleteActivity(_view.CurrentPrincipal, null, false);
                        svc.CompleteActivity(_view.CurrentPrincipal, inputFields, false);
                        svc.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
                        txn.VoteCommit();
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
        }
    }
}
