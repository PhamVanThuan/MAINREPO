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
using SAHL.Common.Factories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using System.Collections.Generic;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.CacheData;

namespace SAHL.Web.Views.Administration.Presenters
{
    public class Admin_CBOMenuView1 : SAHLCommonBasePresenter<IViewCBOMenu1>
    {
        public Admin_CBOMenuView1(SAHL.Web.Views.Administration.Interfaces.IViewCBOMenu1 view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }

        protected void BindFeatureTree()
        {
            List<IBindableTreeItem> Bind = null;
            int TopLevelKey = 0; // SAHL ... get this from ddl in future.
            if (!PrivateCacheData.ContainsKey("ALLFEATURE"))
            {
                IOrganisationStructureRepository repo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                IEventList<IFeature> Features = repo.GetTopLevelFeatureList();
                Bind = new List<IBindableTreeItem>();
                foreach (IFeature f in Features)
                {
                    BindableFeature bf = new BindableFeature(f,true);
                    Bind.Add(bf);
                }
                PrivateCacheData.Add("ALLFEATURE", Bind);
            }
            else
            {
                Bind = PrivateCacheData["ALLFEATURE"] as List<IBindableTreeItem>;
            }
            //Bind = BindOrganisationStructure.GenData();
            _view.BindFeatureList(Bind, TopLevelKey, -1);
        }

        /// <summary>
        /// Hook the events fired by the view and call relevant methods to bind control data
        /// </summary>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage) return;

            _view.OnNextClick += new EventHandler(_view_OnNextClick);
            _view.OnTreeSelected += new EventHandler(_view_OnTreeSelected);
            BindFeatureTree();
        }

        void _view_OnTreeSelected(object sender, EventArgs e)
        {
            int Key = Convert.ToInt32(((KeyChangedEventArgs)e).Key);
            if (PrivateCacheData.ContainsKey("KEY"))
                PrivateCacheData.Remove("KEY");
            PrivateCacheData.Add("KEY", Key);
        }

        void _view_OnNextClick(object sender, EventArgs e)
        {
            ICBOMenu cbo = GlobalCacheData["CURRENTCBO"] as ICBOMenu;
            IOrganisationStructureRepository repo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
            if (PrivateCacheData.ContainsKey("KEY"))
            {
                int Key = Convert.ToInt32(PrivateCacheData["KEY"]);
                IFeature feature = repo.GetFeatureByKey(Key);
                cbo.Feature = feature;
            }

            IList<ICacheObjectLifeTime> LifeTimes = new List<ICacheObjectLifeTime>();
            GlobalCacheData.Remove("CURRENTCBO");
            GlobalCacheData.Add("CURRENTCBO", cbo, LifeTimes);
            Controller.Navigator.Navigate("CBOMenu2");
        }

    }
}
