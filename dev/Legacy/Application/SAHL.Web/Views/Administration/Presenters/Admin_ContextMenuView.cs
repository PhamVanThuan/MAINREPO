using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.Administration.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using System.Collections.Generic;
using SAHL.Common.Web.UI.Events;
using Castle.ActiveRecord;

namespace SAHL.Web.Views.Administration.Presenters
{
    public class Admin_ContextMenuView : Admin_ContextBase<IViewContextMenu>
    {
        public Admin_ContextMenuView(SAHL.Web.Views.Administration.Interfaces.IViewContextMenu view, SAHLCommonBaseController controller)
            : base(view, controller) {}

        

        /// <summary>
        /// Hook the events fired by the view and call relevant methods to bind control data
        /// </summary>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage) return;

            _view.OnParentClick += new EventHandler(_view_OnParentClick);
            _view.OnParentSelected += new EventHandler(_view_OnParentSelected);
            _view.OnFeatureTreeNodeSelected += new EventHandler(_view_OnFeatureTreeNodeSelected);
            _view.OnFeatureButtonClicked += new EventHandler(_view_OnFeatureButtonClicked);
        }

    }
}
