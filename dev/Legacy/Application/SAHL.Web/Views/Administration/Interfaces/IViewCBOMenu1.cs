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
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Web.Views.Administration.Interfaces
{
    public interface IViewCBOMenu1 : IViewBase
    {
        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnNextClick;
        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnTreeSelected;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Features"></param>
        /// <param name="TopLevelKey"></param>
        /// <param name="Selected"></param>
        void BindFeatureList(List<IBindableTreeItem> Features, int TopLevelKey, int Selected);
    }
}
