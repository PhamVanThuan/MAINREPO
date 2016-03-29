using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAHL.Common.Web.UI;

namespace SAHL.Web.Views.Administration.Presenters
{
    public class AttorneyContactView : AttorneyContactBase
    {
        /// <summary>
		/// Constructor
		/// </summary>
		/// <param name="view"></param>
		/// <param name="controller"></param>
        public AttorneyContactView(SAHL.Web.Views.Administration.Interfaces.IAttorneyContact view, SAHLCommonBaseController controller)
			: base(view, controller)
		{
		}

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            _view.ReadOnly = true;

            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage) return;
        }
    }
}