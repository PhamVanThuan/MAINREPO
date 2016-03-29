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
    public interface IConditionsUpdate : IViewBase
    {

        /// <summary>
        /// 
        /// </summary>
        event EventHandler btnRestoreStringClicked;

        /// <summary>
        /// 
        /// </summary>
        event EventHandler btnAddClicked;


        /// <summary>
        /// 
        /// </summary>
        event EventHandler btnUpdateClicked;


        /// <summary>
        /// 
        /// </summary>
        event EventHandler btnCancelClicked;

    }
}
