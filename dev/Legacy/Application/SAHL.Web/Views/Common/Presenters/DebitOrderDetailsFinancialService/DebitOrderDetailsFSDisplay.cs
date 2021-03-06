using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.UI;
using SAHL.Common.Factories;
using SAHL.Web.Views.Common.Presenters.FutureDatedTransactions;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Collections;

namespace SAHL.Web.Views.Common.Presenters.DebitOrderDetailsFinancialService
{
    /// <summary>
    /// 
    /// </summary>
    public class DebitOrderDetailsFSDisplay : DebitOrderDetailsFSBase
    {


        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public DebitOrderDetailsFSDisplay(IDebitOrderDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            _view.gridPostBackType = GridPostBackType.None;
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage) return;

            _view.BindBankAccountControl(base.BankAccounts);

            _view.BindGrid(base.FinancialService);
            _view.BindPaymentTypes();
          
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage) return; 
           
            _view.ShowButtons = false;
            _view.ShowControls = false;
            _view.ShowLabels = false;            
            _view.SetEffectiveDateToCurrentDate = true;
        }
    }
}
