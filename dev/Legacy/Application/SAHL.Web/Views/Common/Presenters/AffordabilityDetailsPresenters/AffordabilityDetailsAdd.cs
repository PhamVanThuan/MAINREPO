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
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using Castle.ActiveRecord;
using SAHL.Common.Exceptions;
using SAHL.BusinessModel.Interfaces.Repositories;
using System.Collections.Generic;

namespace SAHL.Web.Views.Common.Presenters.AffordabilityDetailsPresenters
{
    /// <summary>
    /// 
    /// </summary>
    public class AffordabilityDetailsAdd : AffordabilityDetailsBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="View"></param>
        /// <param name="Controller"></param>
        public AffordabilityDetailsAdd(IAffordabilityDetails View, SAHLCommonBaseController Controller)
            : base(View, Controller)
        {
        }
    }
}
