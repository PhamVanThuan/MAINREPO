using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Administration.Interfaces;
using SAHL.Web.AJAX;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using SAHL.Common.Security;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Factories;
using System.Security.Principal;
using SAHL.Common;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml;
using System.Diagnostics.CodeAnalysis;

namespace SAHL.Web.Views.Administration
{
    /// <summary>
    /// View used to flush various items from the cache.
    /// </summary>
    public partial class X2Config : SAHLCommonBaseView, IX2Config
    {
        /// <summary>
        /// Binds a data table of Process Info.  A data table was used instead of Process objects as
        /// we don't want all the designer data that comes with the object - we only want the map 
        /// Name and MapVersion
        /// </summary>
        /// <param name="dt"></param>
        public void BindProcessInfo(DataTable dt)
        {
            grdMaps.DataSource = dt;
            grdMaps.DataBind();
        }

    }
}
