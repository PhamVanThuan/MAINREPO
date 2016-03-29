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
using SAHL.Common.UI;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Web.Views.Common.Presenters.ManualDebitOrder
{
    /// <summary>
    /// 
    /// </summary>
    public class ManualDebitOrderBase : SAHLCommonBasePresenter<IManualDebitOrder>
    {
        /// <summary>
        /// 
        /// </summary>
        protected CBOMenuNode _node;

        /// <summary>
        /// 
        /// </summary>
        public CBOMenuNode Node
        {
            get
            {
                return _node;
            }
            set
            {
                _node = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="View"></param>
        /// <param name="Controller"></param>
        public ManualDebitOrderBase(IManualDebitOrder View, SAHLCommonBaseController Controller)
            : base(View, Controller)
        {
            
        }

    }
}
