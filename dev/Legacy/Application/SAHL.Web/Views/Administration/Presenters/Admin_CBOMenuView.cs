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
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.CacheData;

namespace SAHL.Web.Views.Administration.Presenters
{
    public class Admin_CBOMenuView : SAHLCommonBasePresenter<IViewCBOMenu>
    {
        public Admin_CBOMenuView(SAHL.Web.Views.Administration.Interfaces.IViewCBOMenu view, SAHLCommonBaseController controller)
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
            BindLookups();
            BindUIStatement();

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
            _view.BindUIStatement(Bind, "");
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

        void _view_OnNextClick(object sender, EventArgs e)
        {
            IOrganisationStructureRepository repo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
            ICBOMenu cbo = repo.CreateEmptyCBO();
            cbo.Description = _view.Desc;
            cbo.NodeType = _view.NodeType;
            cbo.URL = _view.URL;
            cbo.MenuIcon = _view.MenuIcon;
            int Key = _view.GenericKeyTYpe;
            cbo.GenericKeyType.Key = Key;
            //IEventList<IGenericKeyType> gkts = RepositoryFactory.GetRepository<ILookupRepository>().GenericKeyType;
            //IGenericKeyType gkt = null;
            //foreach(IGenericKeyType gk in gkts)
            //{
            //    if (gk.Key == Key)
            //    {
                    
            //        break;
            //    }
            //}
            cbo.HasOriginationSource = _view.HasOriginationSource;
            cbo.IsRemovable = _view.IsRemovable;
            cbo.ExpandLevel = Convert.ToInt32(_view.ExpandLevel);
            cbo.IncludeParentHeaderIcons = _view.IncludeParentHeaderIcons;

            if (!_view.UIStatementName.Contains("Please"))
                cbo.StatementNameKey = _view.UIStatementName;


            IList<ICacheObjectLifeTime> LifeTimes = new List<ICacheObjectLifeTime>();
            GlobalCacheData.Add("CURRENTCBO", cbo, LifeTimes);
            Controller.Navigator.Navigate("CBOMenu1");
        }

        /// <summary>
        /// Set the relevant properties for displaying controls within the view
        /// </summary>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {

            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage) return;
            _view.VisibleMaint = true;
        }

    }
}