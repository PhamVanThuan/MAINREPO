using System;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Globals;
using SAHL.Common.Factories;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces.SearchCriteria;
using SAHL.X2.Framework.Interfaces;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Service;
using Castle.ActiveRecord;
using SAHL.Common.DomainMessages;
using SAHL.Common.BusinessModel.SearchCriteria;


namespace SAHL.Web.Views.Life.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class LeadCreate : SAHLCommonBasePresenter<SAHL.Web.Views.Life.Interfaces.ILeadCreate>
    {
        private ILifeRepository _lifeRepo;
        private IAccountRepository _accountRepo;
        private IList<IADUser> _lstConsultants;
        private IAccountSearchCriteria _searchCriteria = new AccountSearchCriteria();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public LeadCreate(SAHL.Web.Views.Life.Interfaces.ILeadCreate view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        
        /// <summary>
        /// Hook the events fired by the view and call relevant methods to bind control data
        /// </summary>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage)
                return;

            _lifeRepo = RepositoryFactory.GetRepository<ILifeRepository>();
            _accountRepo = RepositoryFactory.GetRepository<IAccountRepository>();

            // initialize events
            _view.OnSearchButtonClicked += new EventHandler(OnSearchButtonClicked);
            _view.OnCreateButtonClicked += new EventHandler(OnCreateButtonClicked);
            _view.OnCancelButtonClicked += new EventHandler(OnCancelButtonClicked);

            // Check if the user is an Admin user
            _view.AdminUser = _lifeRepo.IsAdminUser(_view.CurrentPrincipal);

            // get the ACTIVE life consultants
            // if the user is an admin user then all consultants are available for selection
            // non admin users can only select themselves
            _lstConsultants = _view.AdminUser ? _lifeRepo.GetLifeConsultants() : _lifeRepo.GetLifeConsultants(_view.CurrentPrincipal.Identity.Name);
            _view.BindConsultants(_lstConsultants);

            // restore the saved results
            if (PrivateCacheData.ContainsKey(ViewConstants.LifeSearchCriteria))
                _searchCriteria = PrivateCacheData[ViewConstants.LifeSearchCriteria] as IAccountSearchCriteria;
            if (PrivateCacheData.ContainsKey(ViewConstants.LifeSearchResults))
            {
                _view.SearchResults = PrivateCacheData[ViewConstants.LifeSearchResults] as IEventList<SAHL.Common.BusinessModel.Interfaces.IAccount>;
                _view.BindSearchResults();
            }

        }

        void OnSearchButtonClicked(object sender, EventArgs e)
        {
            ValidateSearchCriteria();

            if (_view.IsValid)
            {
                // set the search criteria
                _searchCriteria = new AccountSearchCriteria();

                if (!string.IsNullOrEmpty(_view.SearchAccountKey))
                    _searchCriteria.AccountKey = Convert.ToInt32(_view.SearchAccountKey.Trim());
                if (!string.IsNullOrEmpty(_view.SearchFirstNames))
                    _searchCriteria.FirstNames = _view.SearchFirstNames.Trim();
                if (!string.IsNullOrEmpty(_view.SearchSurname))
                    _searchCriteria.Surname = _view.SearchSurname.Trim();

                _searchCriteria.Products.Add(Products.NewVariableLoan);
                _searchCriteria.Products.Add(Products.SuperLo);
                _searchCriteria.Products.Add(Products.VariableLoan);
                _searchCriteria.Products.Add(Products.VariFixLoan);
                _searchCriteria.Products.Add(Products.DefendingDiscountRate);
                _searchCriteria.Products.Add(Products.Edge);

                PerformSearch();
            }
        }

        private void PerformSearch()
        {
            // perform the search
            _view.SearchResults = _accountRepo.SearchAccounts(_searchCriteria, _view.MaxSearchResults + 1);

            PrivateCacheData.Add(ViewConstants.LifeSearchCriteria, _searchCriteria);
            PrivateCacheData.Add(ViewConstants.LifeSearchResults, _view.SearchResults);

            // bind the search results grid
            _view.BindSearchResults();
        }

        void OnCreateButtonClicked(object sender, EventArgs e)
        {
            ValidateSelection();

            if (_view.IsValid)
            {
                bool error = false;
                foreach (int loanAccountKey in _view.SelectedLoanAccountKeys)
                {
                    TransactionScope ts = new TransactionScope();

                    try
                    {
                        X2Service svc = ServiceFactory.GetService<IX2Service>() as X2Service;

                        // create a workflow instance
                        Dictionary<string, string> Inputs = new Dictionary<string, string>();
                        Inputs.Add("LoanNumber", loanAccountKey.ToString());
                        Inputs.Add("AssignTo", _view.SelectedConsultant);
                        IX2Info XI = svc.GetX2Info(_view.CurrentPrincipal);
                        if (XI == null || String.IsNullOrEmpty(XI.SessionID))
                            svc.LogIn(_view.CurrentPrincipal);
                        svc.CreateWorkFlowInstance(_view.CurrentPrincipal, SAHL.Common.Constants.WorkFlowProcessName.LifeOrigination, (-1).ToString(), SAHL.Common.Constants.WorkFlowName.LifeOrigination, "Create Instance", Inputs, false);

                        // complete the 'Create Instance' activity - this will run the application creation code in the Life Helper
                        X2ServiceResponse response = svc.CreateCompleteActivity(_view.CurrentPrincipal, Inputs, false, null);
                        if (response.IsError)
                            throw new Exception("Error Creating Instance");

                    }
                    catch (Exception)
                    {
                        error = true;
                        ts.VoteRollBack();

                        if (_view.IsValid)
                            throw;

                        break;
                    }
                    finally
                    {
                        ts.Dispose();
                    }
                }

                if (error == false)
                {
                    // refresh results
                    if (PrivateCacheData.ContainsKey(ViewConstants.LifeSearchCriteria))
                    {
                        _searchCriteria = PrivateCacheData[ViewConstants.LifeSearchCriteria] as IAccountSearchCriteria;
                        PerformSearch();
                    }
                }
            }

        }

        void OnCancelButtonClicked(object sender, EventArgs e)
        {
            if (PrivateCacheData.ContainsKey(ViewConstants.LifeSearchResults))
                PrivateCacheData.Remove(ViewConstants.LifeSearchResults);
            if (PrivateCacheData.ContainsKey(ViewConstants.LifeSearchCriteria))
                PrivateCacheData.Remove(ViewConstants.LifeSearchCriteria);

            if (GlobalCacheData.ContainsKey(ViewConstants.NavigateTo))
            {
                string navigateTo = GlobalCacheData[ViewConstants.NavigateTo].ToString();
                GlobalCacheData.Remove(ViewConstants.NavigateTo);
                _view.Navigator.Navigate(navigateTo);
            }
            else
                _view.Navigator.Navigate("Cancel");
        }

        void ValidateSearchCriteria()
        {
            if (String.IsNullOrEmpty(_view.SearchAccountKey) && String.IsNullOrEmpty(_view.SearchFirstNames) && String.IsNullOrEmpty(_view.SearchSurname))
            {
                _view.Messages.Add(new Error("Search Criteria must be entered.", "Search Criteria must be entered."));
            }
        }

        void ValidateSelection()
        {
            if (_view.SelectedLoanAccountKeys.Count <= 0)
                _view.Messages.Add(new Error("Must select at least one Loan.", "Must select at least one Loan."));
            if (String.IsNullOrEmpty(_view.SelectedConsultant))
                _view.Messages.Add(new Error("Must select a Consultant.", "Must select a Consultant."));
        }

    }
}
