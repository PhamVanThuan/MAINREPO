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
using SAHL.Web.Views.Administration.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using Castle.ActiveRecord;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Factories;

namespace SAHL.Web.Views.Administration.Presenters
{
    public class Admin_ContextMenuEdit : Admin_ContextBase<IViewContextMenu>
    {
        public Admin_ContextMenuEdit(SAHL.Web.Views.Administration.Interfaces.IViewContextMenu view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage) return;

            _view.OnSubmitClick += new EventHandler(_view_OnSubmitClick);
            _view.OnParentClick += new EventHandler(_view_OnParentClick);
            _view.OnFeatureButtonClicked += new EventHandler(_view_OnFeatureButtonClicked);
        }

        void _view_OnSubmitClick(object sender, EventArgs e)
        {
            IOrganisationStructureRepository repo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
            //int Key = Convert.ToInt32(((KeyChangedEventArgs)e).Key);
            IContextMenu cm = PrivateCacheData["SELECTEDCM"] as IContextMenu;
            cm.Key = _view.ContextKey;
            cm.URL = _view.URL;
            cm.Sequence = _view.Sequence;
            cm.Description = _view.Description;
            using (new SessionScope())
            {
                repo.SaveContextMenu(cm);
                if (!_view.IsValid)
                {
#warning do something.
                }
                else
                {
                    View.Navigator.Navigate("ContextMenu");
                }
            }
        }

    }
}
