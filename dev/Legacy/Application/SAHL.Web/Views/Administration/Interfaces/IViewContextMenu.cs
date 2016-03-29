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
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace SAHL.Web.Views.Administration.Interfaces
{
    public interface IBindableTreeItem
    {
        List<IBindableTreeItem> Children { get; }
        string Desc { get; }
        int Key { get; }
        int ParentKey { get; }
    }

    public class BindableContextMenu : IBindableTreeItem
    {
        internal int _Key;
        public int Key { get { return _Key; } }

        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal int CBOKey = -1;
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal string _ParentDesc;
        internal int _ParentKey = -1;
        public int ParentKey { get { return _ParentKey; } }
        internal string _Desc;
        public string Desc { get { return _Desc; } }
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal string _URL;
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal int _FeatureKey = -1;
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal string _FeatureDesc;
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal int _Sequence;
        internal List<IBindableTreeItem> _Children = new List<IBindableTreeItem>();
        public List<IBindableTreeItem> Children { get { return _Children; } }
        public BindableContextMenu() { }
        public BindableContextMenu(IContextMenu cm, bool LoadLittleBrats)
        {
            Populate(cm, LoadLittleBrats);
        }
        private void Populate(IContextMenu cm, bool LoadLittleBrats)
        {
            _Key = cm.Key;
            _Desc = cm.Description;
            if (null != cm.ParentMenu)
            {
                _ParentKey = cm.ParentMenu.Key;
                _ParentDesc = cm.ParentMenu.Description;
            }
            _URL = cm.URL;
            if (cm.Feature != null)
            {
                _FeatureKey = cm.Feature.Key;
                _FeatureDesc = cm.Feature.ShortName;
            }
            _Sequence = cm.Sequence;
            if (!LoadLittleBrats) return;
            foreach (IContextMenu cmm in cm.ChildMenus)
            {
                _Children.Add(new BindableContextMenu(cmm, LoadLittleBrats));
            }
        }
    }

    public interface IViewContextMenu : IViewBase
    {
        bool VisibleFeature { set; }
        string FeatureButtonText { get; set;}
        string FeatureText { set; }
        string Description { get; }
        int ContextKey { get; }
        int Sequence { get; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings")]
        string URL { get; }
        event EventHandler OnFeatureTreeNodeSelected;
        event EventHandler OnFeatureButtonClicked;
        void BindFeatures(List<IBindableTreeItem> Bind, int TopLevelKey);
        bool VisibleParent { set; }
        /// <summary>
        /// 
        /// </summary>
        bool VisibleMaint { set; }
        /// <summary>
        /// 
        /// </summary>
        bool VisibleContextMenu { set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Bind"></param>
        /// <param name="TopLevelKey"></param>
        void BindContextMenu(List<IBindableTreeItem> Bind, int TopLevelKey);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="menu"></param>
        void BindSingleContextMenu(BindableContextMenu menu);
        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnTreeNodeSelected;
        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnSubmitClick;
        event EventHandler OnParentClick;
        string ParentButton { set; get;}
        event EventHandler OnParentSelected;
        string ParentText { set; }
        string SubmitText { set; }
    }
}
