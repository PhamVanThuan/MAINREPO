using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.Web.UI;
using SAHL.Common.Collections.Interfaces;
using SAHL.Web.Views.Life.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using System.Collections.Generic;

namespace SAHL.Web.Views.Common.Interfaces
{
    public interface IProductSummary : IViewBase
    {
        /// <summary>
        /// Binds the list of account products to the grid
        /// </summary>
        /// <param name="accounts"></param>
        void BindSummaryGrid(IList<SAHL.Common.BusinessModel.Interfaces.IAccount> accounts);

        /// <summary>
        /// Binds the list of rate overrides to the grid
        /// </summary>
        /// <param name="financialAdjustments"></param>
        void BindFinancialAdjustmentGrid(IList<IFinancialAdjustment> financialAdjustments);
    }
}
