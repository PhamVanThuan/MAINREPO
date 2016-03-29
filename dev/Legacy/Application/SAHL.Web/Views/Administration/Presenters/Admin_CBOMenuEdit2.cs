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
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Views.Administration.Presenters
{
    public class Admin_CBOMenuEdit2 : SAHLCommonBasePresenter<IViewCBOMenu2>
    {
        public Admin_CBOMenuEdit2(SAHL.Web.Views.Administration.Interfaces.IViewCBOMenu2 view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }

        /// <summary>
        /// Hook the events fired by the view and call relevant methods to bind control data
        /// </summary>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage) return;

            _view.OnTreeNodeSelect += new EventHandler(_view_OnTreeNodeSelect);
            _view.OnFinishClick += new EventHandler(_view_OnFinishClick);
            BindCBOTree();
        }

        void _view_OnTreeNodeSelect(object sender, EventArgs e)
        {
            int Key = Convert.ToInt32(((KeyChangedEventArgs)e).Key);
            IOrganisationStructureRepository repo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
            ICBOMenu cbo = repo.GetCBOByKey(Key);
            PrivateCacheData.Remove("SELECTEDPARENTCBO");
            PrivateCacheData.Add("SELECTEDPARENTCBO", cbo);
        }

        protected void BindCBOTree()
        {
            IOrganisationStructureRepository repo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
            List<IBindableTreeItem> Bind = null;
            if (!PrivateCacheData.ContainsKey("ALLCBO"))
            {
                IEventList<ICBOMenu> cbo = repo.GetTopLevelCBONodes();
                Bind = new List<IBindableTreeItem>();
                foreach(ICBOMenu icbo in cbo)
                {
                    Bind.Add(new BindableCBO(icbo, true));
                }
                PrivateCacheData["ALLCBO"] = Bind;
            }
            else
            {
                Bind = PrivateCacheData["ALLCBO"] as List<IBindableTreeItem>;
            }
            ICBOMenu CurrentCBO = GlobalCacheData["CURRENTCBO"] as ICBOMenu;
            if (null != CurrentCBO.ParentMenu)
            {
                _view.BindCBOList(Bind, 0, CurrentCBO.ParentMenu.Key);
            }
            else
            {
                _view.BindCBOList(Bind, 0, -1);
            }
        }

        void _view_OnFinishClick(object sender, EventArgs e)
        {
            ICBOMenu cbo = GlobalCacheData["CURRENTCBO"] as ICBOMenu;
            ICBOMenu parent = PrivateCacheData["SELECTEDPARENTCBO"] as ICBOMenu;
            cbo.ParentMenu = parent;
            GlobalCacheData["CURRENTCBO"] = cbo;
            Controller.Navigator.Navigate("CBOSummary");
        }

    }
}
