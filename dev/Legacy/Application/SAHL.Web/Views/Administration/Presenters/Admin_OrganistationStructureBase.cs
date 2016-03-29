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

namespace SAHL.Web.Views.Administration.Presenters
{
    public class Admin_OrganistationStructureBase<T> : SAHLCommonBasePresenter<IViewOrganisationStructure>
    {
        public Admin_OrganistationStructureBase(SAHL.Web.Views.Administration.Interfaces.IViewOrganisationStructure view, SAHLCommonBaseController controller)
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

            List<IBindableTreeItem> Bind = null;
            int TopLevelKey = 1; // SAHL ... get this from ddl in future.
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

        protected void BindLookups()
        {
            ILookupRepository repo = RepositoryFactory.GetRepository<ILookupRepository>();
            if (!PrivateCacheData.ContainsKey("GENERALSTATUS"))
            {
                PrivateCacheData.Add("GENERALSTATUS", repo.GeneralStatuses);
            }
            if (!PrivateCacheData.ContainsKey("OSTYPE"))
            {
                PrivateCacheData.Add("OSTYPE", repo.OrganisationTypes);
            }

            IEventList<IGeneralStatus> status = PrivateCacheData["GENERALSTATUS"] as IEventList<IGeneralStatus>;
            IEventList<IOrganisationType> osType = PrivateCacheData["OSTYPE"] as IEventList<IOrganisationType>;
            _view.BindLookups(status, osType);
        }
    }
}
