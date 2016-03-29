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
using SAHL.Common.Exceptions;
using SAHL.Common.Globals;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace SAHL.Web.Views.Cap.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class CapOfferSalesReadvanceDone : CapOfferSalesBase
    {
        #region Constructor

        public CapOfferSalesReadvanceDone(ICapOfferSales view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        #endregion


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
            _view.SubmitButtonEnabled = false;
            _view.ConfirmMessageText = "Are you sure that the ReAdvance has been done?";
            _view.CancelButtonVisible = false;

            _view.OnCancelButtonClicked += new EventHandler(_view_OnCancelButtonClicked);
            _view.OnSubmitButtonClicked += new EventHandler(_view_OnSubmitButtonClicked);

            IList<ICapApplicationDetail> detailList = new List<ICapApplicationDetail>();
            for (int i = 0; i < _capOffer.CapApplicationDetails.Count; i++)
            {
                if (_capOffer.CapApplicationDetails[i].CapStatus.Key == Convert.ToInt32(CapStatuses.ReadvanceRequired))
                {
                    detailList.Add(_capOffer.CapApplicationDetails[i]);
                    _view.SubmitButtonEnabled = true;
                }
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
                ILookupRepository lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();
                for (int i = 0; i < _capOffer.CapApplicationDetails.Count; i++)
                {
                    if (_capOffer.CapApplicationDetails[i].CapStatus.Key == Convert.ToInt32(CapStatuses.ReadvanceRequired))
                    {
                        _capOffer.CapApplicationDetails[i].CapStatus = lookupRepo.CapStatuses.ObjectDictionary[Convert.ToInt32(CapStatuses.TakenUp).ToString()];
                    }
                }
                _capOffer.Promotion = false;
                _capOffer.CapStatus = lookupRepo.CapStatuses.ObjectDictionary[Convert.ToInt32(CapStatuses.TakenUp).ToString()];
                _capRepo.SaveCapApplication(_capOffer);

                Dictionary<string, string> inputFields = new Dictionary<string, string>();
                inputFields.Add("CapOfferKey", _capOffer.Key.ToString());
                inputFields.Add("CapStatusKey", _capOffer.CapStatus.Key.ToString());
                inputFields.Add("Promotion", Convert.ToInt32(_capOffer.Promotion.Value).ToString());

                if (_view.IsValid)
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