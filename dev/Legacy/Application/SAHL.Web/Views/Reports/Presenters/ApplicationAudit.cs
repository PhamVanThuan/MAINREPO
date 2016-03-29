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
using SAHL.Web.Views.Reports.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.CacheData;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Web.Views.Reports.Presenters
{
    public class ApplicationAudit : SAHLCommonBasePresenter<IApplicationAudit>
    {
        public ApplicationAudit(IApplicationAudit view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage) return;

            _view.OnSubmitButtonClicked += new EventHandler(_view_OnSubmitButtonClicked);

        }

        void _view_OnSubmitButtonClicked(object sender, EventArgs e)
        {
            // exit if there is no valid key
            if (String.IsNullOrEmpty(_view.ApplicationKey) || _view.ApplicationKey.Trim().Length == 0)
                return;

            // convert the string into an int
            int offerKey;
            if (!Int32.TryParse(_view.ApplicationKey, out offerKey))
                return;

            IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
            IEventList<IAudit> auditData = appRepo.GetApplicationAuditData(offerKey);
            _view.BindAuditData(auditData);
        }

    }
}
