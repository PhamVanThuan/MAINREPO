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
    /// <summary>
    /// 
    /// </summary>
    public interface IApplicationHistory : IViewBase
    {
        #region Properties

        /// <summary>
        /// Sets the application property
        /// </summary>
        IApplication application { set;}

        #endregion


        #region Events     
     
        #endregion

        #region Methods


        void BindGrid();
        
        #endregion
    }
}
