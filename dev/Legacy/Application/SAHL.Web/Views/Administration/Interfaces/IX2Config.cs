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
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Web.Views.Administration.Interfaces
{
    /// <summary>
    /// View used to display X2 config settings for HALO.
    /// </summary>
    public interface IX2Config : IViewBase
    {

        /// <summary>
        /// Binds a data table of Process Info.  A data table was used instead of Process objects as
        /// we don't want all the designer data that comes with the object - we only want the map 
        /// Name and MapVersion.
        /// </summary>
        /// <param name="dt"></param>
        void BindProcessInfo(DataTable dt);
    }
}
