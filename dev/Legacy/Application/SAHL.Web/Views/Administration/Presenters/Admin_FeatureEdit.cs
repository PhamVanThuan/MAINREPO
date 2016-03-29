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
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Web.UI;
using Castle.ActiveRecord;

namespace SAHL.Web.Views.Administration.Presenters
{
    public class Admin_FeatureEdit : Admin_FeatureBase<IViewFeature>
    {
        public Admin_FeatureEdit(SAHL.Web.Views.Administration.Interfaces.IViewFeature view, SAHLCommonBaseController controller)
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
            _view.OnTreeSelected += new EventHandler(_view_OnTreeSelected);
            _view.OnSubmitClick += new EventHandler(_view_OnSubmitClick);
        }

        protected void _view_OnFeatureSelectedItemChanged(object sender, EventArgs e)
        {
            base._view_OnFeatureSelectedItemChanged(sender, e);
            _view.VisibleButtons = true;
            _view.VisibleFeatureList = true;
        }

        void _view_OnSubmitClick(object sender, EventArgs e)
        {
            IOrganisationStructureRepository repo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
            IEventList<IFeature> AllFeatures = PrivateCacheData["ALLFEATURE"] as IEventList<IFeature>;
            int Key = _view.Key;
            IFeature feature = null;
            foreach (IFeature tmpfeature in AllFeatures)
            {
                if (tmpfeature.Key == Key)
                {
                    feature = tmpfeature;
                    break;
                }
            }

            feature.ShortName = _view.ShortName;
            feature.LongName = _view.LongName;
            feature.Sequence = _view.Sequence;
            feature.HasAccess = true;
            int ParentKey = _view.ParentKey;
            if (ParentKey > 0)
            {
                foreach (IFeature f in AllFeatures)
                {
                    if (f.Key == ParentKey)
                    {
                        feature.ParentFeature = f;
                        break;
                    }
                }
            }
            using (new TransactionScope())
            {
                repo.SaveFeature(feature);
            }
            if (_view.Messages.Count > 0)
            {
#warning messages control here
            }
            else
            {
                Controller.Navigator.Navigate("FeatureView");
            }
        }

        /// <summary>
        /// Set the relevant properties for displaying controls within the view
        /// </summary>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage) return;
            _view.VisibleFeatureList = true;
        }

        void _view_OnTreeSelected(object sender, EventArgs e)
        {
            //return;
            int Key = Convert.ToInt32(((KeyChangedEventArgs)e).Key);
            IEventList<IFeature> AllFeatures = PrivateCacheData["ALLFEATURE"] as IEventList<IFeature>;
            foreach (IFeature f in AllFeatures)
            {
                if (f.Key == Key)
                {
                    _view.BindParent(f.Key, f.ShortName);
                    break;
                }
            }
        }
    }
}
