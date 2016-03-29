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
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Exceptions;
using Castle.ActiveRecord;

namespace SAHL.Web.Views.Administration.Presenters
{
    /// <summary>
    /// Presenter used for creating new Employer records using the IEmployer view.
    /// </summary>
    public class EmployerAdd : EmployerBase
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public EmployerAdd(SAHL.Web.Views.Administration.Interfaces.IEmployer view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!View.ShouldRunPage) return;

            _view.AddButtonClicked += new EventHandler(_view_AddButtonClicked);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _view_AddButtonClicked(object sender, EventArgs e)
        {
            if (SaveEmployer())
                _view.Navigator.Navigate("EmployerDetails");
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

            _view.EditMode = EmployerDetailsEditMode.EditAll;
            _view.AddButtonVisible = true;
            _view.CancelButtonVisible = true;
        }

    }
}
