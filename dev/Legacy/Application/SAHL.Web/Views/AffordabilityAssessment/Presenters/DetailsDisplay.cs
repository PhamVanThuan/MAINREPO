using SAHL.Common.Web.UI;
using SAHL.Web.Views.AffordabilityAssessment.Interfaces;
using System;

namespace SAHL.Web.Views.AffordabilityAssessment.Presenters
{
    /// <summary>
    ///
    /// </summary>
    public class DetailsDisplay : DetailsBase
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public DetailsDisplay(IDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            _view.AffordabilityAssessmentMode = AffordabilityAssessmentMode.Display;
            _view.ButtonRowVisible = false; 
            
            if (_view.AffordabilityAssessment != null)
                _view.BindAffordabilityAssessmentDetail();
        }
    }
}