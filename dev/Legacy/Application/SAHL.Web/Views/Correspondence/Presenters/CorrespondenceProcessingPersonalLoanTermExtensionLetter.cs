using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAHL.Common.Factories;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Correspondence.Interfaces;

namespace SAHL.Web.Views.Correspondence.Presenters.Correspondence
{
    public class CorrespondenceProcessingPersonalLoanTermExtensionLetter : SAHL.Web.Views.Correspondence.Presenters.Correspondence.CorrespondenceProcessing
    {
        public CorrespondenceProcessingPersonalLoanTermExtensionLetter(ICorrespondenceProcessing view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnSendButtonClicked(object sender, EventArgs e)
        {
            IRuleService svc = ServiceFactory.GetService<IRuleService>();
            int rulePassed = svc.ExecuteRule(_view.Messages, "CheckChangeTermStageTransition", GenericKey);
            if (rulePassed == 0)
                return;

            base.OnSendButtonClicked(sender, e);
        }
    }
}