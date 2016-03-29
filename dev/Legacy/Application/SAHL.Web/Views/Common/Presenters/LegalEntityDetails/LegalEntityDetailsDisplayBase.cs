using System;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;

namespace SAHL.Web.Views.Common.Presenters.LegalEntityDetails
{
    public class LegalEntityDetailsDisplayBase : SAHLCommonBasePresenter<ILegalEntityDetails>
    {
        private ILegalEntity _legalEntity;
        private CBONode _node;
        private int _legalEntityKey;
        private ILegalEntityRepository _legalEntityRepository;
        private ILookupRepository _lookupRepository;

        public ILegalEntity LegalEntity
        {
            set { _legalEntity = value; }
            get { return _legalEntity; }
        }

        public CBONode Node
        {
            get { return _node; }
            set { _node = value; }
        }

        protected ILegalEntityRepository LegalEntityRepository
        {
            get { return _legalEntityRepository; }
            set { _legalEntityRepository = value; }
        }

        protected ILookupRepository LookupRepository
        {
            get { return _lookupRepository; }
            set { _lookupRepository = value; }
        }

        public LegalEntityDetailsDisplayBase(ILegalEntityDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage) return;

            // I've moved this code from the constructor to try stop wierd crashes. (AP)
            // Populate the repositories
            _legalEntityRepository = RepositoryFactory.GetRepository<ILegalEntityRepository>();
            _lookupRepository = RepositoryFactory.GetRepository<ILookupRepository>();

            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;

            _legalEntityKey = _node.GenericKey;

            // Get the binding current legal entity object
            LegalEntity = LegalEntityRepository.GetLegalEntityByKey(_legalEntityKey);

            // the code i've moved stops here (AP)

            _view.SelectAllMarketingOptionsExcluded = true;
            _view.BindMarketingOptons(_lookupRepository.MarketingOptionsActive);

            // Call the bind method
            switch ((LegalEntityTypes)LegalEntity.LegalEntityType.Key)
            {
                case LegalEntityTypes.NaturalPerson:
                    _view.BindLegalEntityReadOnlyNaturalPerson((ILegalEntityNaturalPerson)LegalEntity);
                    break;

                case LegalEntityTypes.CloseCorporation:
                    _view.BindLegalEntityReadOnlyCompany((ILegalEntityCloseCorporation)LegalEntity);
                    break;

                case LegalEntityTypes.Company:
                    _view.BindLegalEntityReadOnlyCompany((ILegalEntityCompany)LegalEntity);
                    break;

                case LegalEntityTypes.Trust:
                    _view.BindLegalEntityReadOnlyCompany((ILegalEntityTrust)LegalEntity);
                    break;
            }
        }

        protected override void OnViewLoaded(object sender, EventArgs e)
        {
            if (!_view.ShouldRunPage)
                return;

            base.OnViewLoaded(sender, e);
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage)
                return;

            // Set the visibility
            _view.PanelAddVisible = false;
            _view.PanelNaturalPersonAddVisible = false;
            _view.PanelCompanyAddVisible = false;
            _view.UpdateControlsVisible = false;
            _view.LockedUpdateControlsVisible = false;
            _view.NonContactDetailsDisabled = false;
            _view.MarketingOptionsEnabled = false;
            _view.CancelButtonVisible = false;
            _view.SubmitButtonVisible = false;

            _view.PanelNaturalPersonDisplayVisible = (LegalEntity is ILegalEntityNaturalPerson);
            _view.PanelCompanyDisplayVisible = !(LegalEntity is ILegalEntityNaturalPerson);

            _view.AddRoleTypeVisible = false;
            _view.UpdateRoleTypeVisible = false;
            _view.DisplayRoleTypeVisible = false;
        }
    }
}