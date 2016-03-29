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

namespace SAHL.Web.Views.Administration.Presenters
{
    public class Admin_FeatureBase<T> : SAHLCommonBasePresenter<IViewFeature>
    {
        public Admin_FeatureBase(SAHL.Web.Views.Administration.Interfaces.IViewFeature view, SAHLCommonBaseController controller)
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

            _view.OnFeatureSelectedItemChanged += new EventHandler(_view_OnFeatureSelectedItemChanged);
            BindFeatureList();
            BindTree();

        }

        protected void BindFeatureList()
        {
            IOrganisationStructureRepository repo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
            IEventList<IFeature> AllFeatures = null;
            if (!PrivateCacheData.ContainsKey("ALLFEATURE"))
            {
                AllFeatures = repo.GetCompleteFeatureList();
                PrivateCacheData.Add("ALLFEATURE", AllFeatures);
            }
            AllFeatures = PrivateCacheData["ALLFEATURE"] as IEventList<IFeature>;

            _view.BindFeatureList(AllFeatures);
        }

        protected void BindTree()
        {
            IFeature feature = null;
            if (PrivateCacheData.ContainsKey("FEATURE"))
                feature = PrivateCacheData["FEATURE"] as IFeature;

            List<IBindableTreeItem> BindFeatureList = null;
            if (!PrivateCacheData.ContainsKey("FEATURES"))
            {
                IOrganisationStructureRepository repo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                IEventList<SAHL.Common.BusinessModel.Interfaces.IFeature> Features = repo.GetTopLevelFeatureList();
                BindFeatureList = new List<IBindableTreeItem>();
                foreach (SAHL.Common.BusinessModel.Interfaces.IFeature f in Features)
                {
                    BindableFeature bf = new BindableFeature(f, true);
                    BindFeatureList.Add(bf);
                }
            }
            else
            {
                BindFeatureList = PrivateCacheData["FEATURES"] as List<IBindableTreeItem>;
            }
            _view.BindFeatureTree(BindFeatureList);
            if (PrivateCacheData.ContainsKey("FEATURE"))
                _view.BindFeature(new BindableFeature(feature, false));
        }

        protected void _view_OnFeatureSelectedItemChanged(object sender, EventArgs e)
        {
            int Key = Convert.ToInt32(((KeyChangedEventArgs)e).Key);
            IEventList<IFeature> AllFeatures = PrivateCacheData["ALLFEATURE"] as IEventList<IFeature>;
            foreach (IFeature f in AllFeatures)
            {
                if (f.Key == Key)
                {
                    PrivateCacheData["FEATURE"] = f;
                    this.BindTree();
                    break;
                }
            }
        }
    }
}
