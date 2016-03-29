using System;
using System.Data;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.X2.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;


namespace SAHL.Web.Views.Common.Interfaces
{
    public interface IX2WorkList : IViewBase
    {

        event KeyChangedEventHandler OnSelectButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        /// <param name="gridHeading"></param>
        void SetupGrid(DataTable config, string gridHeading);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        void BindGrid(DataTable data);

    }
}
