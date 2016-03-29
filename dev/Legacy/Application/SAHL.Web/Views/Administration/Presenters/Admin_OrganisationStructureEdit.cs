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
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI.Events;
using Castle.ActiveRecord;

namespace SAHL.Web.Views.Administration.Presenters
{
    public class Admin_OrganisationStructureEdit : Admin_OrganistationStructureBase<IViewOrganisationStructure>
    {
        public Admin_OrganisationStructureEdit(IViewOrganisationStructure view, SAHLCommonBaseController controller)
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
            _view.OnSubmitClick += new EventHandler(_view_OnSubmitClick);
            base.BindLookups();
        }

        void _view_OnSubmitClick(object sender, EventArgs e)
        {
            IOrganisationStructureRepository repo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
            IOrganisationStructure OS = PrivateCacheData["SELECTEDOS"] as IOrganisationStructure;
            OS.Description = _view.Desc;
            int OSType = _view.OSType;
            int GenStatusKey = _view.GeneralStatusKey;
            int ParentKey = _view.ParentKey;
            IEventList<IGeneralStatus> gss = PrivateCacheData["GENERALSTATUS"] as IEventList<IGeneralStatus>;
            IEventList<IOrganisationType> ots = PrivateCacheData["OSTYPE"] as IEventList<IOrganisationType>;
            foreach (IGeneralStatus gs in gss)
            {
                if (gs.Key == GenStatusKey)
                {
                    OS.GeneralStatus = gs;
                    break;
                }
            }
            foreach (IOrganisationType ot in ots)
            {
                if (ot.Key == OSType)
                {
                    OS.OrganisationType = ot;
                    break;
                }
            }
            if (ParentKey > 0)
            {
                IOrganisationStructure Parent = repo.GetOrganisationStructureForKey(ParentKey);
                OS.Parent = Parent;
            }
            using (new TransactionScope())
            {
                repo.SaveOrganisationStructure(OS);
            }
            if (_view.Messages.Count > 0)
            {
                // string wtf = "";
            }
            else
            {
                Controller.Navigator.Navigate("OrganisationStructureView");
            }
        }

        void _view_OnTreeNodeSeleced(object sender, EventArgs e)
        {
            int Key = Convert.ToInt32(((KeyChangedEventArgs)e).Key);
            // BindOrganisationStructure OrgStruct = null;
            IOrganisationStructureRepository repo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
            IOrganisationStructure os = repo.GetOrganisationStructureForKey(Key);
            PrivateCacheData.Add("SELECTEDOS", os);
            _view.BindSingleOrganisationStructure(new BindOrganisationStructure(os, false), Key);
            _view.VisibleMaint = true;
        }
    }
}
