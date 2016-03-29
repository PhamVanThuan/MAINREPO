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
using SAHL.Web.Views.Common.Interfaces;
using System.Collections.Generic;
using SAHL.Common.CacheData;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Web.Views.Common.Presenters.ValuationDetails;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.UI;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Globals;
using SAHL.Common.Factories;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Collections;
using SAHL.Common.Service.Interfaces;

namespace SAHL.Web.Views.Common.Presenters.ManualValuationMainDwellingDetails
{
    public class MainDwellingExtendedAdd : MainDwellingExtendedBase
    {
        public MainDwellingExtendedAdd(IValuationManualPropertyDetailsView view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            
            if (!View.ShouldRunPage) 
                return;

            // Need to bind on initialise or grid is empty on selected index changed
            if (GlobalCacheData.ContainsKey(ViewConstants.ValuationManual))
                _valManual = GlobalCacheData[ViewConstants.ValuationManual] as IValuationDiscriminatedSAHLManual;

			_view.Property = _property;
            _view.BindExtendedDetailsGrid(PopulateValuationExtendedDetailsList(_valManual));
        }
    }
}
