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
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Globals;
using Castle.ActiveRecord;
using SAHL.Common.CacheData;

namespace SAHL.Web.Views.Cap.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class CapOfferSalesAccept : CapOfferSalesBase
    {
        #region Constructor

        public CapOfferSalesAccept(ICapOfferSales view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        #endregion


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
            for (int i = 0; i < _capOffer.CapApplicationDetails.Count; i++)
            {
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
            if (_view.GridSelectedIndex != -1)
            {
                Dictionary<string, string> inputFields = new Dictionary<string, string>();
                ILookupRepository lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();
                int detailKey = _capOffer.CapApplicationDetails[_view.GridSelectedIndex].Key ;
                //int promotion = 0; 
                X2Service svc = ServiceFactory.GetService<IX2Service>() as X2Service;

                TransactionScope txn = new TransactionScope(TransactionMode.Inherits);
                try
                {
                    ValidateOnUpdate();
                    for (int i = 0; i < _capOffer.CapApplicationDetails.Count; i++)
                    {
                        if (_capOffer.CapApplicationDetails[i].Key == detailKey)
                        {
                            _capOffer.CapApplicationDetails[i].CapStatus = lookupRepo.CapStatuses.ObjectDictionary[Convert.ToInt32(CapStatuses.ReadvanceRequired).ToString()];
                        }
                        else
                        {
                            _capOffer.CapApplicationDetails[i].CapStatus = lookupRepo.CapStatuses.ObjectDictionary[Convert.ToInt32(CapStatuses.NotTakenUp).ToString()];
                        }
                    }

                    _capOffer.Promotion = false;
                    _capOffer.CapStatus = lookupRepo.CapStatuses.ObjectDictionary[Convert.ToInt32(CapStatuses.ReadvanceRequired).ToString()];
                    _capOffer.CAPPaymentOption = GetSelectedCapPaymentOption(_view.CapPaymentOptionSelectedValue);

                    // Status needs to be changed from ReadvanceRequired as it will be sent to Credit
                    if (!_capRepo.CheckLTVThreshold(_capOffer))
                    {
                        _capOffer.CapStatus = lookupRepo.CapStatuses.ObjectDictionary[Convert.ToInt32(CapStatuses.PrepareForCredit).ToString()];
                        foreach (ICapApplicationDetail cad in _capOffer.CapApplicationDetails)
                        {
                            if (cad.CapStatus.Key == (int)CapStatuses.ReadvanceRequired)
                                cad.CapStatus = lookupRepo.CapStatuses.ObjectDictionary[Convert.ToInt32(CapStatuses.PrepareForCredit).ToString()];
                        }
                    }

                    inputFields.Add("CapOfferKey", _capOffer.Key.ToString());
                    inputFields.Add("CapStatusKey", _capOffer.CapStatus.Key.ToString());
                    inputFields.Add("Promotion", Convert.ToInt32(_capOffer.Promotion.Value).ToString());
                    inputFields.Add("CapPaymentOptionKey", _capOffer.CAPPaymentOption.Key.ToString());

                    SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(_view.CurrentPrincipal);
                    IRuleService ruleSVC = ServiceFactory.GetService<IRuleService>();
                    ruleSVC.ExecuteRule(spc.DomainMessages, "ApplicationCap2AllowFurtherLendingSPV", _capOffer.Account.Key, (int)DisbursementTransactionTypes.CAP2ReAdvance, (int)OfferTypes.ReAdvance);
                    ruleSVC.ExecuteRule(spc.DomainMessages, "ApplicationCap2CheckReadvanceRequired", _capOffer);
                    ruleSVC.ExecuteRule(spc.DomainMessages, "ApplicationCap2CheckLTVThreshold", _capOffer);

                    // If there are error messages then the save will fail
                    _capRepo.SaveCapApplication(_capOffer);
                    txn.VoteCommit();

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

                if (_view.IsValid)
                {
                    svc.CompleteActivity(_view.CurrentPrincipal, inputFields, false);
                    svc.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
                }
            }
        }
    }
}