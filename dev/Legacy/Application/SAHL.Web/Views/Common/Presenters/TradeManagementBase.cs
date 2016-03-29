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
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.UI;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Web.UI.Events;
using Castle.ActiveRecord;
using System.Collections;

namespace SAHL.Web.Views.Common.Presenters
{
    public class TradeManagementBase : SAHLCommonBasePresenter<ITradeManagement>
    {

        public enum TradeType
        {
            Cap,
            Fixed
        }

        protected ICapRepository _capRepo;
        protected IDictionary<string, string> _tradeList;
        protected DataTable _resetDates;
        protected DataTable _tradeGroupings;
        protected IList<ITrade> _trades;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public TradeManagementBase(ITradeManagement view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }




        /// <summary>
        /// 
        /// </summary>
        protected void BindTradeTypeDropDown()
        {
            _tradeList = new Dictionary<string, string>();
            _tradeList.Add("C", TradeType.Cap.ToString());
            _tradeList.Add("F", TradeType.Fixed.ToString());
            _view.BindTradeTypeDropDown(_tradeList);
        }
    }
}
