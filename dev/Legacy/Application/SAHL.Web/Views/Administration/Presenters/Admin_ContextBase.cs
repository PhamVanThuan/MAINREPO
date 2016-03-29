using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Microsoft.ApplicationBlocks.UIProcess;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Administration.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Views.Administration.Presenters
{
    public class Admin_ContextBase<T> : SAHLCommonBasePresenter<IViewContextMenu>
    {
        public Admin_ContextBase(IViewContextMenu view, SAHLCommonBaseController controller)
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
            
            _view.OnTreeNodeSelected += new EventHandler(_view_OnTreeNodeSelected);
            _view.OnParentSelected += new EventHandler(_view_OnParentSelected);
            _view.OnFeatureTreeNodeSelected += new EventHandler(_view_OnFeatureTreeNodeSelected);
            BindTree();
            //if (PrivateCacheData.ContainsKey("SELECTEDCM"))
            //{
            //    IContextMenu cm = PrivateCacheData["SELECTEDCM"] as IContextMenu;
            //    _view.BindSingleContextMenu(new BindableContextMenu(cm, false));
            //}
            //if (PrivateCacheData.ContainsKey("CURRENTFEATURE"))
            //{
            //    IFeature f = PrivateCacheData["CURRENTFEATURE"] as IFeature;
            //    _view.FeatureText = f.ShortName;
            //}
            //if (PrivateCacheData.ContainsKey("PARENTCM"))
            //{
            //    IContextMenu parent = PrivateCacheData["PARENTCM"] as IContextMenu;
            //    _view.ParentText = parent.Description;
            //}
        }

        protected void _view_OnFeatureButtonClicked(object sender, EventArgs e)
        {
            if (_view.FeatureButtonText == "Show Feature")
            {
                _view.FeatureButtonText = "Hide Feature";
                _view.VisibleFeature = true;
            }
            else
            {
                _view.FeatureButtonText = "Show Feature";
                _view.VisibleFeature = false;
            }
        }

        protected void _view_OnParentClick(object sender, EventArgs e)
        {
            if (_view.ParentButton == "Show Parent")
            {
                _view.ParentButton = "Hide Parent";
                _view.VisibleParent = true;
            }
            else
            {
                _view.ParentButton = "Show Parent";
                _view.VisibleParent = false;
            }
        }

        protected void UpdateBindParent(IContextMenu Parent)
        {
            if (PrivateCacheData.ContainsKey("SELECTEDCM"))
            {
                IContextMenu cm = PrivateCacheData["SELECTEDCM"] as IContextMenu;
                cm.ParentMenu = Parent;
                PrivateCacheData["SELECTEDCM"] = cm;
                //_view.BindSingleContextMenu(new BindableContextMenu(cm, false));
                _view.ParentText = Parent.Description;
            }
        }

        protected void UpdateBindFeature(IFeature feature)
        {
            if (PrivateCacheData.ContainsKey("SELECTEDCM"))
            {
                IContextMenu cm = PrivateCacheData["SELECTEDCM"] as IContextMenu;
                cm.Feature = feature;
                PrivateCacheData["SELECTEDCM"] = cm;
                //_view.BindSingleContextMenu(new BindableContextMenu(cm, false));
                _view.FeatureText = feature.ShortName;
            }
        }

        protected void BindTree()
        {
            List<IBindableTreeItem> Bind = null;
            List<IBindableTreeItem> FeatureBind = null;
            if (!PrivateCacheData.ContainsKey("ALLCONTEXTMENU"))
            {
                IOrganisationStructureRepository repo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                IEventList<IContextMenu> items = repo.GetTopLevelContextMenuNodes();
                Bind = new List<IBindableTreeItem>();
                foreach (IContextMenu cm in items)
                {
                    Bind.Add(new BindableContextMenu(cm, true));
                }
                PrivateCacheData.Add("ALLCONTEXTMENU", Bind);
            }
            else
            {
                Bind = PrivateCacheData["ALLCONTEXTMENU"] as List<IBindableTreeItem>;
            }
            if (!PrivateCacheData.ContainsKey("ALLFEATURE"))
            {
                IOrganisationStructureRepository repo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                IEventList<IFeature> items = repo.GetTopLevelFeatureList();
                FeatureBind = new List<IBindableTreeItem>();
                foreach (IFeature cm in items)
                {
                    FeatureBind.Add(new BindableFeature(cm, true));
                }
                PrivateCacheData.Add("ALLFEATURE", FeatureBind);
            }
            else
            {
                FeatureBind = PrivateCacheData["ALLFEATURE"] as List<IBindableTreeItem>;
            }
            _view.BindContextMenu(Bind, -1);
            _view.BindFeatures(FeatureBind, -1);
        }

        protected void _view_OnTreeNodeSelected(object sender, EventArgs e)
        {
            int Key = Convert.ToInt32(((KeyChangedEventArgs)e).Key);
            IContextMenu cm = RepositoryFactory.GetRepository<IOrganisationStructureRepository>().GetContextMenuByKey(Key);
            PrivateCacheData.Remove("SELECTEDCM");
            PrivateCacheData.Add("SELECTEDCM", cm);
            _view.BindSingleContextMenu(new BindableContextMenu(cm, false));
        }

        protected void _view_OnFeatureTreeNodeSelected(object sender, EventArgs e)
        {
            int Key = Convert.ToInt32(((KeyChangedEventArgs)e).Key);
            IFeature f = RepositoryFactory.GetRepository<IOrganisationStructureRepository>().GetFeatureByKey(Key);
            _view.FeatureText = f.ShortName;
            PrivateCacheData["CURRENTFEATURE"] = f;
            UpdateBindFeature(f);
        }

        protected void _view_OnParentSelected(object sender, EventArgs e)
        {
            int Key = Convert.ToInt32(((KeyChangedEventArgs)e).Key);
            IContextMenu cm = RepositoryFactory.GetRepository<IOrganisationStructureRepository>().GetContextMenuByKey(Key);
            PrivateCacheData.Remove("PARENTCM");
            PrivateCacheData.Add("PARENTCM", cm);
            _view.ParentText = cm.Description;
            UpdateBindParent(cm);
        }
    }
}
