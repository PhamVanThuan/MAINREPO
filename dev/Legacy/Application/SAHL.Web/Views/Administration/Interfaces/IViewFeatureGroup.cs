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
using System.Collections.Generic;

namespace SAHL.Web.Views.Administration.Interfaces
{
    public class BindableFeatureGroup
    {
        internal string _ADUserGroup;
        public string ADUserGroup { get { return _ADUserGroup; } }
        internal int _FeatureKey;
        public int FeatureKey { get { return _FeatureKey; } }
        internal int _Key;
        public int Key { get { return _Key; } }
        public BindableFeatureGroup(IFeatureGroup fg)
        {
            _Key=fg.Key;
            _FeatureKey = fg.Feature.Key;
            _ADUserGroup = fg.ADUserGroup;
        }

        public BindableFeatureGroup() {}
    }

    public interface IViewFeatureGroup : IViewBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="FeatureGroups"></param>
        void BindADUserGroup(List<BindableFeatureGroup> FeatureGroups);
        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnFeatureGroupChanged;
        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnSubmitClick;
        /// <summary>
        /// 
        /// </summary>
        bool VisibleSummaryAccess { set; }
        /// <summary>
        /// 
        /// </summary>
        bool VisibleTree { set; }
    }
}
