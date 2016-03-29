using System;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;

namespace SAHL.Web.Views.Common.Presenters
{
    /// <summary>
    /// Base presenter for Legal Entity Domicilium Address
    /// </summary>
    public class LegalEntityDomiciliumAddressBase : SAHLCommonBasePresenter<ILegalEntityDomiciliumAddress>
    {
        protected CBOMenuNode node;

        protected int genericKey;

        protected int genericKeyTypeKey;

        protected ILookupRepository lookupRepo;

        protected IOrganisationStructureRepository osRepo;

        protected ILegalEntityRepository legalentityRepo;

        protected IAddressRepository addressRepo;

        protected IApplicationRepository applicationRepo;

        protected IAccountRepository accountRepo;

        protected ILegalEntity legalEntity;

        public LegalEntityDomiciliumAddressBase(ILegalEntityDomiciliumAddress view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage)
                return;

            node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;

            if (node != null)
            {
                genericKey = Convert.ToInt32(node.GenericKey);
                genericKeyTypeKey = Convert.ToInt32(node.GenericKeyTypeKey);
            }

            osRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
            lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();
            legalentityRepo = RepositoryFactory.GetRepository<ILegalEntityRepository>();
            addressRepo = RepositoryFactory.GetRepository<IAddressRepository>();
            applicationRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
            accountRepo = RepositoryFactory.GetRepository<IAccountRepository>();

            // get the legalentity
            legalEntity = legalentityRepo.GetLegalEntityByKey(genericKey);

            _view.ShowActiveDomiciliumAddressRow = false;
        }

        public bool CheckAddressInCollection(IAddress address, List<AddressBindableObject> bindableAddresses)
        {
            bool exists = false;
            foreach (var ba in bindableAddresses)
            {
                if (address.Key == ba.AddressKey)
                {
                    exists = true;
                    break;
                }
            }
            return exists;
        }
    }
}