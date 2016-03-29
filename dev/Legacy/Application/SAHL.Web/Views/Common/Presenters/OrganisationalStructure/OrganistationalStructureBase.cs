using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;

namespace SAHL.Web.Views.Common.Presenters.OrganisationalStructure
{
    public class OrganistationalStructureBase : SAHLCommonBasePresenter<IOrganisationalStructure>
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public OrganistationalStructureBase(IOrganisationalStructure view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }
    }
}
