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
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using SAHL.Web.Views.Administration.Interfaces;
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Views.Administration.Presenters
{
    public class Admin_FeatureView : Admin_FeatureBase<SAHL.Web.Views.Administration.Interfaces.IViewFeature>
    {
        public Admin_FeatureView(SAHL.Web.Views.Administration.Interfaces.IViewFeature view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }

        /// <summary>
        /// Set the relevant properties for displaying controls within the view
        /// </summary>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {

            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage) return;
            _view.VisibleButtons = false;
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnViewLoaded(object sender, EventArgs e)
        {
            if (!_view.ShouldRunPage) return;


            base.OnViewLoaded(sender, e);
        }
        //void _view_OnTreeSelected(object sender, EventArgs e)
        //{
        //    return;
        //    //int Key = Convert.ToInt32(((KeyChangedEventArgs)e).Key);
        //    //IEventList<IFeature> AllFeatures = PrivateCacheData["ALLFEATURE"] as IEventList<IFeature>;
        //    //foreach (IFeature f in AllFeatures)
        //    //{
        //    //    if (f.Key == Key)
        //    //    {
        //    //        _view.BindParent(f.Key, f.ShortName);
        //    //        break;
        //    //    }
        //    //}
        //}
    }
}
