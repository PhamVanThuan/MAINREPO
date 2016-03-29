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
using SAHL.Common.UI;


namespace SAHL.Web.Views.Cap.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class CapOfferSalesDisplay : CapOfferSalesBase
    {
        #region Constructor

        public CapOfferSalesDisplay(ICapOfferSales view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        #endregion


        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender,e);

            if (!_view.ShouldRunPage) return;

            _view.PaymentOptionDropDownVisible = false;
            LoadCapOffer();

            BindInstalmentValues("Instalment on Quote");
            _view.ReasonDropDownVisible = false;
            _view.PromotionCheckBoxEnabled = false;
            _view.ButtonRowVisible = false;
            _view.CapGridPostBackType = GridPostBackType.None;

            IList<ICapApplicationDetail> detailList = new List<ICapApplicationDetail>();
            for (int i = 0; i < _capOffer.CapApplicationDetails.Count; i++)
            {
                if (
                    _capOffer.CapStatus.Key == Convert.ToInt32(SAHL.Common.Globals.CapStatuses.CheckCashPayment) ||
                    _capOffer.CapStatus.Key == Convert.ToInt32(SAHL.Common.Globals.CapStatuses.PrepareForCredit) ||
                    _capOffer.CapStatus.Key == Convert.ToInt32(SAHL.Common.Globals.CapStatuses.CreditApproval) ||
                    _capOffer.CapStatus.Key == Convert.ToInt32(SAHL.Common.Globals.CapStatuses.GrantedCap2Offer) ||
                    _capOffer.CapStatus.Key == Convert.ToInt32(SAHL.Common.Globals.CapStatuses.ReadvanceRequired) ||
                    _capOffer.CapStatus.Key == Convert.ToInt32(SAHL.Common.Globals.CapStatuses.TakenUp) ||
                    _capOffer.CapStatus.Key == Convert.ToInt32(SAHL.Common.Globals.CapStatuses.AwaitingLA)
                )
                {
                    if (_capOffer.CapApplicationDetails[i].CapStatus.Key == _capOffer.CapStatus.Key)
                        detailList.Add(_capOffer.CapApplicationDetails[i]);
                }
                else
                {
                    detailList.Add(_capOffer.CapApplicationDetails[i]);
                }
            }
            _view.BindCapGrid(detailList);
        }
    }
}
