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
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;
using Castle.ActiveRecord;

namespace SAHL.Web.Views.Administration.Presenters
{
    public class Admin_MapUsersToOrgStructure : SAHLCommonBasePresenter<IViewMapUsersToOrgStructure>
    {
        public Admin_MapUsersToOrgStructure(SAHL.Web.Views.Administration.Interfaces.IViewMapUsersToOrgStructure view, SAHLCommonBaseController controller)
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

            _view.OnTreeNodeSeleced += new EventHandler(_view_OnTreeNodeSeleced);
            _view.OnAddClick += new EventHandler(_view_OnAddClick);
            _view.OnRemoveClick += new EventHandler(_view_OnRemoveClick);
            BindTree();
            BindADUsers();
        }

        protected void BindADUsers()
        {
            IOrganisationStructureRepository repo = null;
            IEventList<IADUser> AllUsers = null;
            IOrganisationStructure os = null;
            if (!PrivateCacheData.ContainsKey("ALLUSERS"))
            {
                repo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                AllUsers = repo.GetCompleteAdUserList();
                PrivateCacheData.Add("ALLUSERS", AllUsers);
            }

            if (!PrivateCacheData.ContainsKey("OS"))
                return;
            
            os = PrivateCacheData["OS"] as IOrganisationStructure;
            
            AllUsers = PrivateCacheData["ALLUSERS"] as IEventList<IADUser>;
            List<ADUserBind> In = new List<ADUserBind>();
            List<ADUserBind> Out = new List<ADUserBind>();
            CalculateInOut(ref In, ref Out, os);
            _view.BindMapping(In, Out);
        }

        protected void CalculateInOut(ref List<ADUserBind> In, ref List<ADUserBind> Out, IOrganisationStructure OS)
        {
            List<int> Keys = new List<int>();
            IEventList<IADUser> AllAdUsers = null;
            foreach (IADUser ad in OS.ADUsers)
            {
                In.Add(new ADUserBind(ad));
                Keys.Add(ad.Key);
            }
            AllAdUsers = PrivateCacheData["ALLUSERS"] as IEventList<IADUser>;

            foreach (IADUser ad in AllAdUsers)
            {
                ADUserBind a = new ADUserBind(ad);
                if (!Keys.Contains(a.AdUserKey))
                    Out.Add(a);
            }
            PrivateCacheData.Remove("IN");
            PrivateCacheData.Add("IN", In);
            PrivateCacheData.Remove("OUT");
            PrivateCacheData.Add("OUT", Out);
        }

        protected void BindTree()
        {
            List<IBindableTreeItem> Bind = null;
            int TopLevelKey = 1; 
            if (!PrivateCacheData.ContainsKey("ORGANISATIONSTRUCTURE"))
            {
                IOrganisationStructureRepository repo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                IEventList<IOrganisationStructure> OrgStructures = repo.GetTopLevelOrganisationStructureForOriginationSource(TopLevelKey);
                Bind = new List<IBindableTreeItem>();
                foreach (SAHL.Common.BusinessModel.Interfaces.IOrganisationStructure os in OrgStructures)
                {
                    BindOrganisationStructure bf = new BindOrganisationStructure(os);
                    Bind.Add(bf);
                }
                PrivateCacheData.Add("ORGANISATIONSTRUCTURE", Bind);
            }
            else
            {
                Bind = PrivateCacheData["ORGANISATIONSTRUCTURE"] as List<IBindableTreeItem>;
            }
            //Bind = BindOrganisationStructure.GenData();
            _view.BindOrganisationStructure(Bind, TopLevelKey);
        }

        void _view_OnRemoveClick(object sender, EventArgs e)
        {
            int Key = Convert.ToInt32(((KeyChangedEventArgs)e).Key);
            IOrganisationStructureRepository repo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
            IOrganisationStructure os = PrivateCacheData["OS"] as IOrganisationStructure;
            foreach (IADUser ad in os.ADUsers)
            {
                if (ad.Key == Key)
                {
                    os.ADUsers.Remove(_view.Messages, ad);
                    using (new TransactionScope())
                    {
                        repo.SaveOrganisationStructure(os);
                    }
                    break;
                }
            }
            PrivateCacheData.Remove("OS");
            PrivateCacheData.Add("OS", os);
            List<ADUserBind> In = new List<ADUserBind>();
            List<ADUserBind> Out = new List<ADUserBind>();
            CalculateInOut(ref In, ref Out, os);
            _view.BindMapping(In, Out);
            _view.VisibleMaint = true;
            _view.VisibleButtons = true;
        }

        void _view_OnAddClick(object sender, EventArgs e)
        {
            int Key = Convert.ToInt32(((KeyChangedEventArgs)e).Key);
            IOrganisationStructureRepository repo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
            IOrganisationStructure os = PrivateCacheData["OS"] as IOrganisationStructure;
            IEventList<IADUser> AllUsers = PrivateCacheData["ALLUSERS"] as IEventList<IADUser>;
            foreach (IADUser ad in AllUsers)
            {
                if (ad.Key == Key)
                {
                    os.ADUsers.Add(_view.Messages, ad);
                    using (new TransactionScope())
                    {
                        repo.SaveOrganisationStructure(os);
                    }
                    break;
                }
            }
            PrivateCacheData.Remove("OS");
            PrivateCacheData.Add("OS", os);
            List<ADUserBind> In = new List<ADUserBind>();
            List<ADUserBind> Out = new List<ADUserBind>();
            CalculateInOut(ref In, ref Out, os);
            _view.BindMapping(In, Out);
            _view.VisibleMaint = true;
            _view.VisibleButtons = true;
        }

        void _view_OnTreeNodeSeleced(object sender, EventArgs e)
        {
            int Key = Convert.ToInt32(((KeyChangedEventArgs)e).Key);
            IOrganisationStructureRepository repo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
            IOrganisationStructure OS = repo.GetOrganisationStructureForKey(Key);
            PrivateCacheData.Remove("OS");
            PrivateCacheData.Add("OS", OS);
            List<ADUserBind> In = new List<ADUserBind>();
            List<ADUserBind> Out = new List<ADUserBind>();
            CalculateInOut(ref In, ref Out, OS);
            _view.BindMapping(In, Out);
            _view.VisibleMaint = true;
            _view.VisibleButtons = true;
        }
    }
}
