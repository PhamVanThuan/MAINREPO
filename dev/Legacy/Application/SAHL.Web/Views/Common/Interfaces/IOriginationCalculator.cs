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

namespace SAHL.Web.Views.Common.Interfaces
{
    public interface IOriginationCalculator : IViewBase
    {

        #region Events

        /// <summary>
        /// Raised when the Create Application button is clicked.
        /// </summary>
        event EventHandler CreateApplicationButtonClicked;

        #endregion

        #region Properties

        /// <summary>
        /// Sets whether the Create Application button is visible.
        /// </summary>
        bool CreateApplicationVisible { set; }


        #endregion
    }
}
