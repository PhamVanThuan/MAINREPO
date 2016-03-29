using SAHL.Common.UI;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using System;
using SAHL.Common;

namespace SAHL.Web.Views.Common.Presenters
{
    public class ApplicationSummaryForCredit : ApplicationSummaryBase
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public ApplicationSummaryForCredit(IApplicationSummary view, SAHLCommonBaseController controller)
            : base(view, controller){}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            if (!_view.ShouldRunPage)
                return;

            // call the base presenter
            base.OnViewInitialised(sender, e);

            //run extra rule required specifically for the credit workflow
            RuleService.ExecuteRule(_view.Messages, Rules.ApplicationHasPrevious30YearTermDisqualification, Application);
            RuleService.ExecuteRule(_view.Messages, Rules.ApplicationHas30YearTermDisqualification, Application);
        }

    }
}