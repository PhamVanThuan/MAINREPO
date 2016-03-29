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
using SAHL.Common.Globals;
using SAHL.Common.Factories;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Service;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Exceptions;
using SAHL.Common.CacheData;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DomainMessages;

namespace SAHL.Web.Views.Cap.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class CapOfferSalesReverse : CapOfferSalesBase
    {
        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public CapOfferSalesReverse(ICapOfferSales view, SAHLCommonBaseController controller)
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
            _view.ReasonDropDownVisible = true;
            _view.PromotionCheckBoxEnabled = false;
            _view.ButtonRowVisible = true;
            _view.CapGridPostBackType = GridPostBackType.None;
            _view.SubmitButtonText = "Cancel CAP2 Offer";
            _view.ConfirmMessageText = "Are you sure you want to Cancel the CAP offer ?";
            _view.SubmitButtonEnabled = false;
            _view.CancelButtonVisible = false;

            _view.OnCancelButtonClicked += new EventHandler(_view_OnCancelButtonClicked);
            _view.OnSubmitButtonClicked += new EventHandler(_view_OnSubmitButtonClicked);

            IList<ICapApplicationDetail> detailList = new List<ICapApplicationDetail>();
            for (int i = 0; i < _capOffer.CapApplicationDetails.Count; i++)
            {
                if (_capOffer.CapApplicationDetails[i].CapStatus.Key == Convert.ToInt32(CapStatuses.TakenUp))
                {
                    detailList.Add(_capOffer.CapApplicationDetails[i]);
                    _view.SubmitButtonEnabled = true;
                }
            }
            _view.BindCapGrid(detailList);

            BindCapNTUReasons();
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

            if (DoValidate())
            {
                int capNTUReason = _view.CapReasonSelectedValue;
                TransactionScope txn = new TransactionScope();
                try
                {
                    ICapNTUReason selectedReason = GetSelectedNTUReason(capNTUReason);
                    ILookupRepository lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();
                    for (int i = 0; i < _capOffer.CapApplicationDetails.Count; i++)
                    {
                        _capOffer.CapApplicationDetails[i].CapStatus = lookupRepo.CapStatuses.ObjectDictionary[Convert.ToInt32(CapStatuses.NotTakenUp).ToString()];
                        _capOffer.CapApplicationDetails[i].CapNTUReason = selectedReason;
                        _capOffer.CapApplicationDetails[i].CapNTUReasonDate = DateTime.Now;
                    }
                    _capOffer.Promotion = false;
                    _capOffer.CapStatus = lookupRepo.CapStatuses.ObjectDictionary[Convert.ToInt32(CapStatuses.NotTakenUp).ToString()];
                    _capRepo.SaveCapApplication(_capOffer);

                    Dictionary<string, string> inputFields = new Dictionary<string, string>();
                    inputFields.Add("CapNTUReasonKey", capNTUReason.ToString());
                    inputFields.Add("CapOfferKey", _capOffer.Key.ToString());
                    inputFields.Add("CapStatusKey", _capOffer.CapStatus.Key.ToString());
                    inputFields.Add("Promotion", Convert.ToInt32(_capOffer.Promotion.Value).ToString());

                    X2Service svc = ServiceFactory.GetService<IX2Service>() as X2Service;
                    if (_view.Messages.ErrorMessages.Count == 0)
                    {
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

        #region Helper Methods

        private bool DoValidate()
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(_view.CurrentPrincipal);
            IDomainMessageCollection dmc = spc.DomainMessages;

            if (_view.CapReasonSelectedValue == -1)
            {
                string errorMessage = "Please select a Reason.";
                dmc.Add(new Error(errorMessage, errorMessage));
                return false;
            }
            return true;
        }

        #endregion
    }
}