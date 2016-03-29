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
    /// View used to display config settings for HALO.
    /// </summary>
    public interface IHaloConfig : IViewBase
    {

        /// <summary>
        /// Binds a list of Control objects to the view.
        /// </summary>
        /// <param name="controlValues"></param>
        void BindControlTableValues(IEventList<IControl> controlValues);
    }
}
