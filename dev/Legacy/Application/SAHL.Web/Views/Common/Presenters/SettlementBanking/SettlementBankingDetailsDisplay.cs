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

namespace SAHL.Web.Views.Common.Presenters.SettlementBanking
{
    public class SettlementBankingDetailsDisplay : SettlementBankingDetailsBase
    {
        public SettlementBankingDetailsDisplay(IBankingDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            _view.BankAccountGridEnabled = false; 
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage) return;

            _view.ShowButtons = false;
           
        }
    }
}
