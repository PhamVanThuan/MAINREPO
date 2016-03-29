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
using SAHL.Web.Controls;

namespace SAHL.Web.Views.Administration.Presenters
{
    /// <summary>
    /// Presenter used for view Employer records using the IEmployer view.
    /// </summary>
    public class EmployerDisplay : EmployerBase
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public EmployerDisplay(SAHL.Web.Views.Administration.Interfaces.IEmployer view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {

            base.OnViewPreRender(sender, e);
            if (!View.ShouldRunPage) return;

            _view.EditMode = EmployerDetailsEditMode.EditName;
        }

    }
}
