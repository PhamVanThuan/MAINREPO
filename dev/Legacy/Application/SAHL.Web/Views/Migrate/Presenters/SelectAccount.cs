using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAHL.Web.Views.Migrate.Presenters;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Migrate.Interfaces;

namespace SAHL.Web.Views.Migrate.Presenters
{
    public class SelectAccount : CreateBase
    {

        /// <summary>
		/// Constructor
		/// </summary>
		/// <param name="view"></param>
		/// <param name="controller"></param>
        public SelectAccount(ICreateCase view, SAHLCommonBaseController controller)
			: base(view, controller)
		{
		}

        /// <summary>
        /// OnView Initialised event - retrieve data for use by presenters
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage)
                return;

            //base work and navigate
            _view.OnSubmitButtonClicked += new EventHandler(_view_OnSubmitButtonClicked);

            _view.WizardPage = 0;
        }
    }
}