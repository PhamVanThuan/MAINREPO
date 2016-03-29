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
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Views.Administration.Presenters
{
    public class Admin_FeatureGroupView : SAHLCommonBasePresenter<IViewFeatureGroup>
    {
        public Admin_FeatureGroupView(SAHL.Web.Views.Administration.Interfaces.IViewFeatureGroup view, SAHLCommonBaseController controller)
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

            _view.OnFeatureGroupChanged += new EventHandler(_view_OnFeatureGroupChanged);
            _view.OnSubmitClick += new EventHandler(_view_OnSubmitClick);
            BindFeatureGroupList();
        }

        protected void BindFeatureGroupList()
        {
            IOrganisationStructureRepository repo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
            //IEventList<IFeatureGroup> FeatureGroups = repo.GetDistinctFeatureGroupList(_view.Messages);
            IEventList<IFeatureGroup> FeatureGroups = repo.GetCompleteFeatureGroupList();
            List<BindableFeatureGroup> Bind = new List<BindableFeatureGroup>();
            List<string> KeysOfSorts = new List<string>();
            foreach (IFeatureGroup fg in FeatureGroups)
            {
                if (!KeysOfSorts.Contains(fg.ADUserGroup))
                {
                    KeysOfSorts.Add(fg.ADUserGroup);
                    Bind.Add(new BindableFeatureGroup(fg));
                }
            }
            _view.BindADUserGroup(Bind);
        }

        protected void BindFeatureTree()
        {
            List<BindableFeature> BindFeatureList = null;
            if (!PrivateCacheData.ContainsKey("FEATURES"))
            {
                IOrganisationStructureRepository repo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                IEventList<IFeature> Features = repo.GetTopLevelFeatureList();
                BindFeatureList = new List<BindableFeature>();
                foreach (SAHL.Common.BusinessModel.Interfaces.IFeature f in Features)
                {
                    BindableFeature bf = new BindableFeature(f, true);
                    BindFeatureList.Add(bf);
                }
            }
            else
            {
                BindFeatureList = PrivateCacheData["FEATURES"] as List<BindableFeature>;
            }

            // now we have to see which ones are selected based on the featuregroup that was selected
#warning wtf?
        }

        void _view_OnSubmitClick(object sender, EventArgs e)
        {
            // string ADUserGroup = ((KeyChangedEventArgs)e).Key.ToString();

        }

        void _view_OnFeatureGroupChanged(object sender, EventArgs e)
        {
            
        }

    }
}
