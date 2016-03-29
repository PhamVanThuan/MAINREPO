using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.Administration.Interfaces;
using SAHL.Common.Web.UI;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Web.Views.Administration.Presenters
{

    public class Admin_OrganisationStructureView : Admin_OrganistationStructureBase<IViewOrganisationStructure>
    {
        public Admin_OrganisationStructureView(SAHL.Web.Views.Administration.Interfaces.IViewOrganisationStructure view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }

    }
}
