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
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using Castle.ActiveRecord;

namespace SAHL.Web.Views.Administration.Presenters
{
    public class Admin_ContextMenuAdd : Admin_ContextBase<IViewContextMenu>
    {
        public Admin_ContextMenuAdd(SAHL.Web.Views.Administration.Interfaces.IViewContextMenu view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            if (!PrivateCacheData.ContainsKey("SELECTEDCM"))
            {
                IContextMenu cm = RepositoryFactory.GetRepository<IOrganisationStructureRepository>().CreateEmptyContextMenu();
                PrivateCacheData["SELECTEDCM"] = cm;
            }
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage) return;

            _view.OnSubmitClick += new EventHandler(_view_OnSubmitClick);
            _view.OnParentClick += new EventHandler(_view_OnParentClick);
            _view.OnFeatureButtonClicked += new EventHandler(_view_OnFeatureButtonClicked);
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage) return;
            _view.VisibleContextMenu = false;
        }

        void _view_OnSubmitClick(object sender, EventArgs e)
        {
            IOrganisationStructureRepository repo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
            IContextMenu cm = PrivateCacheData["SELECTEDCM"] as IContextMenu;
            cm.Key = _view.ContextKey;
            cm.Description = _view.Description;
            cm.Sequence = _view.Sequence;
            cm.URL = _view.URL;
            using (new SessionScope())
            {
                repo.SaveContextMenu(cm);
                if (!_view.IsValid)
                {
#warning do something.
                }
                else
                {
                }
            }
        }
    }
}
