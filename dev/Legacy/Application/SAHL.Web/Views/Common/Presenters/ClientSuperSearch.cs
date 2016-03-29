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
using SAHL.Web.Views.Common.Interfaces;
using Microsoft.ApplicationBlocks.UIProcess;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.SearchCriteria;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.UI;
using SAHL.Common.Service.Interfaces;

using SAHL.Common;
using SAHL.Common.CacheData;

namespace SAHL.Web.Views.Common.Presenters
{
    public class ClientSuperSearch : SAHLCommonBasePresenter<IClientSuperSearch>
    {
        protected ILegalEntityRepository _legalEntityRepository;

        public ILegalEntityRepository LegalEntityRepo   
        {
            get { return _legalEntityRepository; }
            set { _legalEntityRepository = value; }
        }
	
        protected IEventList<ILegalEntity> _results;

        public ClientSuperSearch(IClientSuperSearch view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage) return;

            _legalEntityRepository = RepositoryFactory.GetRepository<ILegalEntityRepository>();

            // hookup events
            _view.ApplicationClicked += new SAHL.Common.Web.UI.Events.KeyChangedEventHandler(OnApplicationClicked);
            _view.SearchClientClicked += new EventHandler<EventArgs>(OnSearchClientClicked);
            _view.CancelClicked += new EventHandler(OnCancelClicked);
            _view.ClientSelectedClicked += new EventHandler<ClientSuperSearchSelectedEventArgs>(OnClientSelectedClicked);
            _view.CreateNewClientClicked += new EventHandler(OnCreateNewClientClicked);

            // setup the buttons
            _view.CancelButtonVisible = false;
            _view.CreateNewClientButtonVisible = false;

            // bind any drop down lists ( this basically comes from the financialservicegroup table )
            IDictionary<int, string> AccTypes = new Dictionary<int, string>();
            AccTypes.Add(0, "All"); // added by CF 31/01/2008
            AccTypes.Add((int)SAHL.Common.Globals.FinancialServiceTypes.VariableLoan, "MortgageLoan");
            AccTypes.Add((int)SAHL.Common.Globals.FinancialServiceTypes.LifePolicy, "Life");
            AccTypes.Add((int)SAHL.Common.Globals.FinancialServiceTypes.HomeOwnersCover, "HOC");
            AccTypes.Add(-1, "None");
            _view.BindAccountTypes(AccTypes);

            // restore and saved results
            if (PrivateCacheData.ContainsKey("RESULTS"))
            {
                _results = PrivateCacheData["RESULTS"] as IEventList<ILegalEntity>;
                View.BindSearchResults(_results);
            }
        }

        #region View Events

        protected virtual void OnApplicationClicked(object sender, SAHL.Common.Web.UI.Events.KeyChangedEventArgs e)
        {
            int appKey = Convert.ToInt32(e.Key);
            CBOManager.SetCurrentNodeSet(_view.CurrentPrincipal, CBONodeSetType.X2);
            _view.ShouldRunPage = false;

            // this key should only be used on the workflowsupersearch, so add to lifetime cache
            List<string> pages = new List<string>();
            pages.Add("WorkflowSuperSearch");
            GlobalCacheData.Add(ViewConstants.ApplicationKey, appKey, new SimplePageCacheObjectLifeTime(pages));

            Navigator.Navigate("WorkflowSuperSearch");
        }

        protected virtual void OnSearchClientClicked(object sender, EventArgs ea)
        {
            IClientSuperSearchCriteria superSearchCriteria = _view.ClientSuperSearchCriteria;
            IClientSearchCriteria searchCriteria = _view.ClientSearchCriteria;

            // different searches are done depending on which criteria are populated
            if (superSearchCriteria != null)
            {
                if (superSearchCriteria.AccountType == "-1")
                {
                    _results = _legalEntityRepository.SuperSearchForAllLegalEntities(superSearchCriteria);
                }
                else
                {
                    _results = _legalEntityRepository.SuperSearchForClientLegalEntities(superSearchCriteria);
                }
            }
            else
            {
                _results = _legalEntityRepository.SearchLegalEntities(searchCriteria, 51);
            }

            PrivateCacheData.Add("RESULTS", _results);
            View.BindSearchResults(_results);
        }

        protected virtual void OnCreateNewClientClicked(object sender, EventArgs e)
        {
            base.Navigator.Navigate("CreateNewEntity");
        }

        protected virtual void OnClientSelectedClicked(object sender, ClientSuperSearchSelectedEventArgs ea)
        {
            // add the selected legal entity to the cbo and navigate
            CBOMenuNode legalEntitiesNode = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            bool alreadyAdded = false;

            // do a check to ensure that the legal entity hasn't already been added
            foreach (CBOMenuNode childNode in legalEntitiesNode.ChildNodes)
            {
                if (childNode.GenericKey == ea.LegalEntityKey)
                {
                    CBOManager.SetCurrentCBONode(_view.CurrentPrincipal, childNode, CBONodeSetType.CBO);
                    alreadyAdded = true;
                    break;
                }
            }

            if (!alreadyAdded)
            {
                ICBOMenu ClientNameTemplate = GetLegalEntityTemplate(legalEntitiesNode);
                CBOManager.AddCBOMenuNodeToSelection(_view.CurrentPrincipal, ClientNameTemplate, ea.LegalEntityKey, CBONodeSetType.CBO);

                // try and select the new node
                CBOMenuNode newNode = (CBOMenuNode)legalEntitiesNode.ChildNodes[legalEntitiesNode.ChildNodes.Count - 1];
                CBOManager.SetCurrentCBONode(_view.CurrentPrincipal, newNode, CBONodeSetType.CBO);
            }
            
            // navigate to selected node
            PrivateCacheData.Remove("RESULTS");
            base.Navigator.Navigate(CBOManager.GetCurrentCBONode(_view.CurrentPrincipal).URL);

        }

        void OnCancelClicked(object sender, EventArgs e)
        {
            PrivateCacheData.Remove("RESULTS");
            base.Navigator.Navigate("Cancel");
        }

        #endregion

        private static ICBOMenu GetLegalEntityTemplate(CBOMenuNode LegalEntitiesNode)
        {
            for (int i = 0; i < LegalEntitiesNode.CBOMenu.ChildMenus.Count; i++)
            {
                if (LegalEntitiesNode.CBOMenu.ChildMenus[i].Description == "ClientName")
                    return LegalEntitiesNode.CBOMenu.ChildMenus[i];
            }
            return null;
        }
    }
}
