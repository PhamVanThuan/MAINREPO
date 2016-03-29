using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;
using SAHL.Web.Views.Common.Interfaces;

namespace SAHL.Web.Views.Common.Presenters
{
    public class ApplicantsExternalRole : SAHLCommonBasePresenter<IApplicantsExternalRole>
    {
        protected CBOMenuNode _node;
        protected ILegalEntity _legalEntity;

        private IApplicationRepository _applicationRepository;
        private IList<IExternalRole> _externalRoles;

        public ApplicantsExternalRole(IApplicantsExternalRole view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            _view.OnGridLegalEntitySelected += new KeyChangedEventHandler(OnGridLegalEntitySelected);
            _applicationRepository = RepositoryFactory.GetRepository<IApplicationRepository>();
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage)
                return;

            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            _view.GridHeading = "Applicants";

            IApplication _application = _applicationRepository.GetApplicationByKey(_node.GenericKey);

            var applicationUnsecuredLending = _application as IApplicationUnsecuredLending;
            _externalRoles = new List<IExternalRole>(applicationUnsecuredLending.ActiveClientRoles);

            _view.BindExternalRoleApplicantsGrid(_externalRoles);

            if (_externalRoles.Count > 0)
            {
                _legalEntity = _externalRoles[0].LegalEntity;
                _view.BindLegalEntityDetails(_legalEntity);
            }
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);

            if (!_view.ShouldRunPage)
                return;
        }

        protected virtual void OnGridLegalEntitySelected(object sender, KeyChangedEventArgs e)
        {
            ILegalEntityRepository legalEntityRepo = RepositoryFactory.GetRepository<ILegalEntityRepository>();
            _legalEntity = legalEntityRepo.GetLegalEntityByKey(_view.SelectedLegalEntityKey);
            _view.BindLegalEntityDetails(_legalEntity);
        }
    }
}