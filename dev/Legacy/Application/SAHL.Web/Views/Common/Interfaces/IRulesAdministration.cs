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
using NHibernate.Expression;
using SAHL.Common.BusinessModel;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;

namespace SAHL.Web.Views.Common.Interfaces
{
    public interface IRulesAdministration : IViewBase
    {
        event EventHandler OnSaveButtonClicked;

        void PopulateRulesGrid();


    }
}
