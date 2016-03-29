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
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Service.Interfaces;

namespace SAHL.Web.Views.Common.Presenters
{
    public class LegalEntityEnableUpdateBase : SAHLCommonBasePresenter<ILegalEntityEnableUpdate>
    {
        private ILegalEntity _legalEntity;
        private ILegalEntityRepository _legalEntityRepository;
        private ILookupRepository _lookupRepository;
        private IAccountRepository _accountRepository;
        private IResourceService _resourceService;

        public LegalEntityEnableUpdateBase(ILegalEntityEnableUpdate view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            // Hook the rest of the events
            _view.OnCancelButtonClick += new EventHandler(OnCancelButtonClick);
            _view.OnSubmitButtonClick += new EventHandler(OnSubmitButtonClick);

            _legalEntityRepository = RepositoryFactory.GetRepository<ILegalEntityRepository>();
            _lookupRepository = RepositoryFactory.GetRepository<ILookupRepository>();
            _accountRepository = RepositoryFactory.GetRepository<IAccountRepository>();
            _resourceService = ServiceFactory.GetService<IResourceService>();
        }

        protected IResourceService ResourceService
        {
            set { _resourceService = value; }
            get { return _resourceService; }
        }
        
        protected ILegalEntity LegalEntity
        {
            set { _legalEntity = value; }
            get { return _legalEntity; }
        }

        protected ILegalEntityRepository LegalEntityRepository
        {
            set { _legalEntityRepository = value; }
            get { return _legalEntityRepository; }
        }

        protected ILookupRepository LookupRepository
        {
            set { _lookupRepository = value; }
            get { return _lookupRepository; }
        }

        protected IAccountRepository AccountRepository
        {
            set { _accountRepository = value; }
            get { return _accountRepository; }
        }

        protected virtual void OnSubmitButtonClick(object sender, EventArgs e)
        {
            Navigator.Navigate("Submit");
        }

        protected virtual void OnCancelButtonClick(object sender, EventArgs e)
        {
            Navigator.Navigate("Cancel");
        }
    }
}
