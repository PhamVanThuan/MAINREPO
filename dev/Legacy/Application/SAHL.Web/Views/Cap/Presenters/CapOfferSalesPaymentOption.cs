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
using SAHL.Common.CacheData;
using SAHL.Common.Globals;
namespace SAHL.Web.Views.Cap.Presenters
{
    public class CapOfferSalesPaymentOption : CapOfferSalesBase
    {
        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public CapOfferSalesPaymentOption(ICapOfferSales view, SAHLCommonBaseController controller)
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
            _view.CapGridPostBackType = GridPostBackType.None;
            _view.SubmitButtonText = "Update Payment Option";
            _view.ConfirmMessageText = "Are you sure you want to update the refund option?";
            _view.OnCancelButtonClicked += new EventHandler(_view_OnCancelButtonClicked);
            _view.OnSubmitButtonClicked += new EventHandler(_view_OnSubmitButtonClicked);
            _view.CancelButtonVisible = false;

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
            X2Service svc = ServiceFactory.GetService<IX2Service>() as X2Service;
            TransactionScope txn = new TransactionScope();
            try
            {
                ValidateOnUpdate();
                _capOffer.CAPPaymentOption = GetSelectedCapPaymentOption(_view.CapPaymentOptionSelectedValue);
                _capRepo.SaveCapApplication(_capOffer);

                Dictionary<string, string> inputFields = new Dictionary<string, string>();
                inputFields.Add("CapOfferKey", _capOffer.Key.ToString());
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
