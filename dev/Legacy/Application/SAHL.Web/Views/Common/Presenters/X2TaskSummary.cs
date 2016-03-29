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
using Microsoft.ApplicationBlocks.UIProcess;

namespace SAHL.Web.Views.Common.Presenters
{
    public class X2TaskSummary : SAHLCommonBasePresenter<IX2TaskSummary>
    {

        public X2TaskSummary(IX2TaskSummary View, SAHLCommonBaseController Controller)
            : base(View, Controller)
        {

        }

        protected override void OnViewPreInitialised()
        {
            base.OnViewPreInitialised();
        }

        protected override void OnViewInitialised()
        {
            base.OnViewInitialised();
        }

        protected override void OnViewLoaded()
        {
            base.OnViewLoaded();
        }
    }
}
