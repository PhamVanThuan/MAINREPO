using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.Life.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.UI;
using SAHL.Common.Globals;
using SAHL.Common.Service;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.CacheData;

using Castle.ActiveRecord;

namespace SAHL.Web.Views.Life.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class Contact : SAHLCommonBasePresenter<IContact>
    {
        private IReadOnlyEventList<ILegalEntity> _lstLegalEntities;
        private CBOMenuNode _node;
        private SAHL.Common.BusinessModel.Interfaces.IAccount _loanAccount;
        private int _accountKey;

        private IApplicationRepository _applicationRepo;
        /// <summary>
        /// 
        /// </summary>
        public IApplicationRepository ApplicationRepo
        {
            get { return _applicationRepo; }
            set { _applicationRepo = value; }
        }
	

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public Contact(IContact view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            // Get the CBO Node    
            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            if (_node == null)
                throw new NullReferenceException(StaticMessages.NullCurrentCBONode);

            if (GlobalCacheData.ContainsKey(ViewConstants.NavigateTo) == true)
                GlobalCacheData.Remove(ViewConstants.NavigateTo);

            GlobalCacheData.Add(ViewConstants.NavigateTo, _view.ViewName, new List<ICacheObjectLifeTime>());

            _applicationRepo = RepositoryFactory.GetRepository<IApplicationRepository>();         
        }

        /// <summary>
        /// Hook the events fired by the view and call relevant methods to bind control data
        /// </summary>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage)
                return;


            _view.OnAddAddressButtonClicked += new KeyChangedEventHandler(OnAddAddressButtonClicked);
            _view.OnUpdateContactDetailsButtonClicked += new KeyChangedEventHandler(OnUpdateContactDetailsButtonClicked); 
            _view.OnLegalEntityGridSelectedIndexChanged += new KeyChangedEventHandler(OnLegalEntityGridSelectedIndexChanged);
            _view.OnNextButtonClicked += new EventHandler(OnNextButtonClicked);
            _view.OnUpdateAddressButtonClicked += new KeyChangedEventHandler(OnUpdateAddressButtonClicked);

            // get the life offer object
            IApplication lifePolicyApplication = _applicationRepo.GetApplicationByKey((int)_node.GenericKey);
            // get the life account
            IAccountLifePolicy accountLifePolicy = lifePolicyApplication.Account as IAccountLifePolicy;

            _accountKey = accountLifePolicy.Key;
          
            // check for assured lives
            _view.AssuredLivesMode = true;
            _lstLegalEntities = accountLifePolicy.GetLegalEntitiesByRoleType(_view.Messages, new int[] { (int)SAHL.Common.Globals.RoleTypes.AssuredLife });

            if (_lstLegalEntities.Count == 0)
            {
                _view.AssuredLivesMode = false;
                // if there are no assured lives then use the main applicants/suretors
                _loanAccount = accountLifePolicy.ParentMortgageLoan;
                _accountKey = _loanAccount.Key;
                _lstLegalEntities = _loanAccount.GetLegalEntitiesByRoleType(_view.Messages, new int[] { (int)SAHL.Common.Globals.RoleTypes.MainApplicant, (int)SAHL.Common.Globals.RoleTypes.Suretor });
            }

            // bind the legal entities grid
            _view.BindLegalEntityGrid(_lstLegalEntities, _accountKey);

            if (_lstLegalEntities.Count > 0)
            {
                _view.ShowUpdateContactButton = true;
                _view.ShowAddAddressButton = true;
                _view.ShowUpdateAddressButton = true;

                _view.BindAssuredLivesDetails(_lstLegalEntities[0]);
                    // Bind Legal Entity Address Data
                _view.BindAddressData(_lstLegalEntities[0].LegalEntityAddresses);

                if (_lstLegalEntities[0].LegalEntityAddresses.Count == 0 )
                    _view.ShowUpdateAddressButton = false;
            }
            else
            {
                _view.BindAssuredLivesDetails(null);
                _view.BindAddressData(null);
                _view.ShowAddAddressButton = false;
                _view.ShowUpdateAddressButton = false;
                _view.ShowUpdateContactButton = false;
            }
        }

        /// <summary>
        /// Handles the event fired by the view when the Legal Entity Grid Selected Index is changed
        /// Populate the BindAssuredDetails controls based on the selected row in the Legal Entity Grid
        /// </summary>
       
        void OnLegalEntityGridSelectedIndexChanged(object sender, KeyChangedEventArgs e) 
        {
            for (int x = 0; x < _lstLegalEntities.Count; x++)
            {
                if (_lstLegalEntities[x].Key == int.Parse(e.Key.ToString()))
                {
                    _view.BindAssuredLivesDetails(_lstLegalEntities[x]);
                    // Bind Legal Entity Address Data
                    _view.BindAddressData(_lstLegalEntities[x].LegalEntityAddresses);

                    if (_lstLegalEntities[x].LegalEntityAddresses.Count == 0)
                        _view.ShowUpdateAddressButton = false;


                    break;
                }
            }
        }          

        /// <summary>
        /// Handles the event fired by the view when the Update Contact Details button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnUpdateContactDetailsButtonClicked(object sender, KeyChangedEventArgs e)
        {
            GlobalCacheData.Remove(ViewConstants.SelectedLegalEntityKey);
            //GlobalCacheData.Remove(ViewConstants.SelectedLifeAccountKey);
            GlobalCacheData.Add(ViewConstants.SelectedLegalEntityKey, e.Key, new List<ICacheObjectLifeTime>());
            //GlobalCacheData.Add(ViewConstants.SelectedLifeAccountKey, _accountKey, new List<ICacheObjectLifeTime>());

            _view.Navigator.Navigate("Update");
        }

        /// <summary>
        /// Handles the event fired by the view when the Add Address button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnAddAddressButtonClicked(object sender, KeyChangedEventArgs e)
        {
            GlobalCacheData.Remove(ViewConstants.SelectedLegalEntityKey);
            GlobalCacheData.Add(ViewConstants.SelectedLegalEntityKey, e.Key, new List<ICacheObjectLifeTime>());

            _view.Navigator.Navigate("AddressAdd");
        }

        /// <summary>
        /// Handles the event fired by the view when the Update Address Button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnUpdateAddressButtonClicked(object sender, KeyChangedEventArgs e)
        {
            GlobalCacheData.Remove(ViewConstants.SelectedLegalEntityKey);
            GlobalCacheData.Add(ViewConstants.SelectedLegalEntityKey, e.Key, new List<ICacheObjectLifeTime>());

            _view.Navigator.Navigate("AddressUpdate");
        }

        /// <summary>
        /// Handles the event fired by the view when the Next Button is Clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnNextButtonClicked(object sender, EventArgs e)
        {
            TransactionScope txn = new TransactionScope();
            try
            {
                // Navigate to the next State
                if (_view.IsValid)
                {
                    X2Service svc = ServiceFactory.GetService<IX2Service>() as X2Service;
                    svc.WorkFlowWizardNext(_view.CurrentPrincipal, _view.ViewName, _view.Navigator);
                }
            }
            catch (Exception)
            {
                txn.VoteRollBack();
                if (_view.IsValid)
                    throw;
            }

            finally
            {
                txn.Dispose();
            }        


        } 

    }
}
