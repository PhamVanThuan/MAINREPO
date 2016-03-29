using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Views.Life.Interfaces
{
    /// <summary>
    ///
    /// </summary>
    public interface IPremiumHistory : IViewBase
    {
        /// <summary>
        ///
        /// </summary>
        event EventHandler OnCancelButtonClicked;

        /// <summary>
        ///
        /// </summary>
        bool ShowCancelButton { set; }

        /// <summary>
        ///
        /// </summary>
        bool ShowLifeWorkFlowHeader { set; }

        /// <summary>
        /// Binds the Premium History Grid data
        /// </summary>
        /// <param name="lstLifePremiumHistory"></param>
        void BindPremiumHistoryGrid(IList<ILifePremiumHistory> lstLifePremiumHistory);
    }
}