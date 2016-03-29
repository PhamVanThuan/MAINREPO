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
using SAHL.Common.BusinessModel.Interfaces;
using System.Collections.Generic;

namespace SAHL.Web.Views.Common.Interfaces
{
    public interface ILastRateChange : IViewBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lstMortgageLoans"></param>
        void BindGrid(IList<IMortgageLoan> lstMortgageLoans);
    }
}
