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
using SAHL.Common.Web.UI.Events;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using Castle.ActiveRecord;
using System.Collections.Generic;
using SAHL.Common.Globals;
using SAHL.Web.Views.Administration.Interfaces;
using SAHL.Common.DomainMessages;

namespace SAHL.Web.Views.Administration.Presenters
{
    public class Admin_AdUserBase : SAHLCommonBasePresenter<IAduser>
    {

        private ILookupRepository _lookupRepo;
        private IActiveDirectoryRepository _activeDirectoryrepo;
        private IOrganisationStructureRepository _organisationStructureRepo;
        private ILegalEntityRepository _legalEntityRepo;
        private ICommonRepository _commonRepo;

        protected ILookupRepository LookupRepo
        {
            get { return _lookupRepo; }
            set { _lookupRepo = value; }
        }

        protected IActiveDirectoryRepository ActiveDirectoryrepo
        {
            get { return _activeDirectoryrepo; }
            set { _activeDirectoryrepo = value; }
        }
        protected IOrganisationStructureRepository OrganisationStructureRepo
        {
            get { return _organisationStructureRepo; }
            set { _organisationStructureRepo = value; }
        }
        protected ILegalEntityRepository LegalEntityRepo
        {
            get { return _legalEntityRepo; }
            set { _legalEntityRepo = value; }
        }
        protected ICommonRepository CommonRepo
        {
            get { return _commonRepo; }
            set { _commonRepo = value; }
        }

        public Admin_AdUserBase(IAduser view, SAHLCommonBaseController controller)
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

            _lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();
            _activeDirectoryrepo = RepositoryFactory.GetRepository<IActiveDirectoryRepository>();
            _organisationStructureRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
            _legalEntityRepo = RepositoryFactory.GetRepository<ILegalEntityRepository>();
            _commonRepo = RepositoryFactory.GetRepository<ICommonRepository>();

            _view.OnAdUserSelected += new KeyChangedEventHandler(_view_OnAdUserSelected);

            _view.BindStatusDropDown(_lookupRepo.GeneralStatuses.Values);
        }

        /// <summary>
        /// Set the relevant properties for displaying controls within the view
        /// </summary>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage)
                return;

            if (PrivateCacheData.ContainsKey("SELECTEDADUSERNAME"))
                _view.VisibleMaint=true;
        }

        void _view_OnAdUserSelected(object sender, KeyChangedEventArgs e)
        {
            string selectedADUserName = e.Key.ToString();

            PrivateCacheData.Remove("SELECTEDADUSERNAME");
            PrivateCacheData.Add("SELECTEDADUSERNAME", selectedADUserName);
           
            // get the active directory user
            ActiveDirectoryUserBindableObject selectedADUser = _activeDirectoryrepo.GetActiveDirectoryUser(selectedADUserName);

            // check if the user already exists in the database
            string checkUser = @"SAHL\" + selectedADUserName;

            IADUser usr = RepositoryFactory.GetRepository<IOrganisationStructureRepository>().GetAdUserForAdUserName(checkUser);
            if (usr != null && String.Compare(usr.ADUserName, checkUser, true) == 0)
                _view.UserExistsInDatabase = true;
            else
                _view.UserExistsInDatabase = false;
            
            // bind the data
            _view.BindAdUser(selectedADUser,usr);
        }

    }
}
