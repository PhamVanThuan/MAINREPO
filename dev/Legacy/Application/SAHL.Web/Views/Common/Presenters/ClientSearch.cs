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
using SAHL.Common.BusinessModel.Interfaces.UI;

namespace SAHL.Web.Views.Common.Presenters
{
    public class ClientSearch : SAHLCommonBasePresenter<IClientSearch>
    {
        //ILegalEntityRepository _legalEntityRepository;
        IEventList<ILegalEntity> _results;
        ClientSearchType _searchType;

        public ClientSearch(IClientSearch view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage) return;

            // hookup events
            _view.SearchClientClicked += new EventHandler<ClientSearchClickedEventArgs>(OnSearchClientClicked);
            _view.CancelClicked += new EventHandler(OnCancelClicked);
            _view.ClientSelectedClicked += new EventHandler<ClientSearchSelectedEventArgs>(OnClientSelectedClicked);
            _view.CreateNewClientClicked += new EventHandler(OnCreateNewClientClicked);


            _view.TMPClickHandler += new EventHandler(_view_TMPClickHandler);
            // bind any drop down lists
            
            //_view.BindAccountTypes(LR.Acc

            // restore and saved results
            if (PrivateCacheData.ContainsKey("RESULTS") && PrivateCacheData.ContainsKey("RESULTTYPE"))
            {
                _results = PrivateCacheData["RESULTS"] as IEventList<ILegalEntity>;
                _searchType = (ClientSearchType)PrivateCacheData["RESULTTYPE"];
                View.BindSearchResults(_results, _searchType);
            }
        }

        void _view_TMPClickHandler(object sender, EventArgs e)
        {
            _view.Navigator.Navigate("ClientSuperSearch");
        }

        #region View Events

        void OnSearchClientClicked(object sender, ClientSearchClickedEventArgs ea)
        {
//            _legalEntityRepository = RepositoryFactory.GetRepository<ILegalEntityRepository>();
            //_results = _legalEntityRepository.SearchForLegalEntities(_ea.SearchCriteria);
            _searchType = ea.SearchCriteria.SearchType;
            PrivateCacheData.Add("RESULTS", _results);
            PrivateCacheData.Add("RESULTTYPE", _searchType);

            if (_results.Count == 51)
            {
//                ValidateSearchCtrl.ErrorMessage = "Your search criteria returned more than 50 records";
//                ValidateSearchCtrl.IsValid = false;
            }
            View.BindSearchResults(_results, _searchType);
        }

        void OnCreateNewClientClicked(object sender, EventArgs e)
        {
            
        }

        void OnClientSelectedClicked(object sender, ClientSearchSelectedEventArgs ea)
        {
            // add the selected legal entity to the cbo and navigate
            CBOMenuNode LegalEntitiesNode = CBOService.GetCurrentCBONode(_view.CurrentPrincipal, CBONodeSet.CBONODESET) as CBOMenuNode;
            ICBOMenu ClientNameTemplate = GetLegalEntityTemplate(LegalEntitiesNode);

            CBOService.AddCBOMenuNodeToSelection(_view.Messages, _view.CurrentPrincipal, ClientNameTemplate, ea.LegalEntityKey, CBONodeSet.CBONODESET);
        }

        void OnCancelClicked(object sender, EventArgs e)
        {
            base.Navigator.Navigate("Cancel");
        }

        #endregion

        private static ICBOMenu GetLegalEntityTemplate(CBOMenuNode legalEntitiesNode)
        {
            for (int i = 0; i < legalEntitiesNode.CBOMenu.ChildMenus.Count; i++)
            {
                if (legalEntitiesNode.CBOMenu.ChildMenus[i].Description == "ClientName")
                    return legalEntitiesNode.CBOMenu.ChildMenus[i];
            }
            return null;
        }
    }
}
