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
using SAHL.Common.Web.UI.Events;
using SAHL.Common.CacheData;
using System.Collections.Generic;

namespace SAHL.Web.Views.Administration.Presenters
{
    public class Admin_CBOMenuEdit: SAHLCommonBasePresenter<IViewCBOMenu>
    {
        public Admin_CBOMenuEdit(SAHL.Web.Views.Administration.Interfaces.IViewCBOMenu view, SAHLCommonBaseController controller)
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

            _view.OnNextClick += new EventHandler(_view_OnNextClick);
            _view.OnTreeSelected += new EventHandler(_view_OnTreeSelected);
            BindLookups();
            BindCBOTree();
            BindUIStatement();

        }

        void _view_OnTreeSelected(object sender, EventArgs e)
        {
            int Key = Convert.ToInt32(((KeyChangedEventArgs)e).Key);
            ICBOMenu CBO = RepositoryFactory.GetRepository<IOrganisationStructureRepository>().GetCBOByKey(Key);
            GlobalCacheData.Remove("CURRENTCBO");
            IList<ICacheObjectLifeTime> LifeTimes = new List<ICacheObjectLifeTime>();
            GlobalCacheData.Add("CURRENTCBO", CBO, LifeTimes);
            _view.BindCBO(CBO);
        }

        protected void BindUIStatement()
        {
            List<BindableUIStatement> Bind = null;
            if (!PrivateCacheData.ContainsKey("ALLUI"))
            {
                IX2Repository x2Repo = RepositoryFactory.GetRepository<IX2Repository>();
                IEventList<IUIStatement> uis = x2Repo.GetAllUIStatement();
                Bind = new List<BindableUIStatement>();
                foreach (IUIStatement ui in uis)
                {
                    Bind.Add(new BindableUIStatement(ui));
                }
            }
            else
            {
                Bind = PrivateCacheData["ALLUI"] as List<BindableUIStatement>;
            }
            ICBOMenu CurrentCBO = GlobalCacheData["CURRENTCBO"] as ICBOMenu;
            if (CurrentCBO.StatementNameKey.Length > 0)
            {
                _view.BindUIStatement(Bind, CurrentCBO.StatementNameKey);
            }
            else
            {
                _view.BindUIStatement(Bind, "");
            }
        }



        protected void BindLookups()
        {
            List<BindableGenericKeyType> Bind = null;
            if (!PrivateCacheData.ContainsKey("GENERICKEYTYPE"))
            {
                ILookupRepository repo = RepositoryFactory.GetRepository<ILookupRepository>();
                IEventList<IGenericKeyType> list = repo.GenericKeyType;
                Bind = new List<BindableGenericKeyType>();
                foreach (IGenericKeyType gkt in list)
                {
                    Bind.Add(new BindableGenericKeyType(gkt));
                }
            }
            else
            {
                Bind = PrivateCacheData["GENERICKEYTYPE"] as List<BindableGenericKeyType>;
            }
            _view.BindGenericKeyType(Bind);
        }

        protected void BindCBOTree()
        {
            IOrganisationStructureRepository repo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
            List<IBindableTreeItem> Bind = null;
            if (!PrivateCacheData.ContainsKey("ALLCBO"))
            {
                IEventList<ICBOMenu> cbo = repo.GetTopLevelCBONodes();
                Bind = new List<IBindableTreeItem>();
                foreach (ICBOMenu icbo in cbo)
                {
                    Bind.Add(new BindableCBO(icbo, true));
                }
                PrivateCacheData["ALLCBO"] = Bind;
            }
            else
            {
                Bind = PrivateCacheData["ALLCBO"] as List<IBindableTreeItem>;
            }
            _view.BindCBOList(Bind, 0);
            _view.ShowAllCBO = true;
            _view.VisibleMaint = true;
        }

        void _view_OnNextClick(object sender, EventArgs e)
        {
            if (!GlobalCacheData.ContainsKey("CURRENTCBO"))
                return;

            ICBOMenu cbo = GlobalCacheData["CURRENTCBO"] as ICBOMenu;
            cbo.Description = _view.Desc;
            cbo.NodeType = _view.NodeType;
            cbo.URL = _view.URL;
            cbo.MenuIcon = _view.MenuIcon;
            int Key = _view.GenericKeyTYpe;
            cbo.GenericKeyType.Key = Key;
            cbo.HasOriginationSource = _view.HasOriginationSource;
            cbo.IsRemovable = _view.IsRemovable;
            cbo.ExpandLevel = Convert.ToInt32(_view.ExpandLevel);
            cbo.IncludeParentHeaderIcons = _view.IncludeParentHeaderIcons;

            if (!_view.UIStatementName.Contains("Please"))
                cbo.StatementNameKey = _view.UIStatementName;

            IList<ICacheObjectLifeTime> LifeTimes = new List<ICacheObjectLifeTime>();
            GlobalCacheData.Remove("CURRENTCBO");
            GlobalCacheData.Add("CURRENTCBO", cbo, LifeTimes);
            Controller.Navigator.Navigate("CBOMenuEdit1");
        }

        /// <summary>
        /// Set the relevant properties for displaying controls within the view
        /// </summary>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {

            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage) return;
            if (!GlobalCacheData.ContainsKey("CURRENTCBO"))
                _view.ShowAllCBO = true;
        }

        ///// <summary>
        ///// 
        ///// </summary>
        //protected override void OnViewLoaded(object sender, EventArgs e)
        //{

        //    base.OnViewLoaded(sender, e);
        //    if (!_view.ShouldRunPage) return;
        //}
    }
}
