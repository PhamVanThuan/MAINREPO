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
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Web.Views.Common.Interfaces
{
    /// <summary>
    /// HOCSummary Interface
    /// </summary>
    public interface IHOCSummary : IViewBase
    {
        /// <summary>
        /// Binds HOC MasterDataControls to HOCSummary View
        /// </summary>
        /// <param name="lstHOC">lstHOC</param>
        void BindMasterDataControls(IHOC lstHOC);

        /// <summary>
        /// Binds HOC DetailDataGrid to HOCSummary View
        /// </summary>
        /// <param name="lstHOCGrid">lstHOC</param>
        void BindDetailDataGrid(List<IHOC> lstHOCGrid);
    }
}
