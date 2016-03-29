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
using Castle.ActiveRecord;
using SAHL.Common.Exceptions;

namespace SAHL.Web.Views.Administration.Presenters
{
    /// <summary>
    /// Presenter used for updating existing Employer records using the IEmployer view.
    /// </summary>
    public class EmployerUpdate : EmployerBase
    {

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public EmployerUpdate(SAHL.Web.Views.Administration.Interfaces.IEmployer view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!View.ShouldRunPage) return;

            _view.EmployerSelected += new SAHL.Common.Web.UI.Events.KeyChangedEventHandler(_view_EmployerSelected);
            _view.ClearFormButtonClicked += new EventHandler(_view_ClearFormButtonClicked);
            _view.UpdateButtonClicked += new EventHandler(_view_UpdateButtonClicked);
        }

        void _view_UpdateButtonClicked(object sender, EventArgs e)
        {
            if (SaveEmployer())
                _view.Navigator.Navigate("EmployerDetails");
        }

        void _view_ClearFormButtonClicked(object sender, EventArgs e)
        {
            _view.ClearEmployer();
            _view.EditMode = EmployerDetailsEditMode.EditName;
        }

        void _view_EmployerSelected(object sender, SAHL.Common.Web.UI.Events.KeyChangedEventArgs e)
        {
            _view.EditMode = EmployerDetailsEditMode.EditDetails;
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

            if (!_view.IsPostBack)
                _view.EditMode = EmployerDetailsEditMode.EditName;

            _view.CancelButtonVisible = true;
            _view.ClearFormButtonVisible = true;
            _view.UpdateButtonVisible = true;

            bool canUpdate = _view.EditMode == EmployerDetailsEditMode.EditDetails;
            _view.ClearFormButtonEnabled = canUpdate;
            _view.UpdateButtonEnabled = canUpdate;
        }

    }
}
