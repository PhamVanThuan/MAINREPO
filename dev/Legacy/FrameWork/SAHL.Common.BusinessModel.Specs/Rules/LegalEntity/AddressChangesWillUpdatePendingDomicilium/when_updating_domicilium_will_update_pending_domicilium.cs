using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Globals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAHL.Common.BusinessModel.Specs.Rules.LegalEntity.AddressChangesWillUpdatePendingDomicilium
{
    [Subject(typeof(SAHL.Common.BusinessModel.Rules.LegalEntityAddress.LegalEntityAddressChangesWillUpdatePendingDomicilium))]
    public class when_updating_domicilium_will_update_pending_domicilium : RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.LegalEntityAddress.LegalEntityAddressChangesWillUpdatePendingDomicilium>
    {
        protected static ILegalEntity legalEntity;
        protected static ILegalEntityAddress legalEntityAddress;
        protected static IAddress address;
        protected static IApplicationRepository applicationRepository;
        protected static ILegalEntityDomicilium legalEntityDomicilium;
        protected static ILegalEntityRepository legalEntityRepository;
        protected static IGeneralStatus legalEntityDomiciliumGeneralStatus;
        protected static IApplicationRoleDomicilium applicationRoleDomicilium;
        protected static IApplication application;
        protected static IApplicationRole applicationRole;

        Establish context = () =>
        {
            legalEntity = An<ILegalEntity>();
            applicationRoleDomicilium = An<IApplicationRoleDomicilium>();
            legalEntityAddress = An<ILegalEntityAddress>();
            address = An<IAddress>();
            legalEntityDomiciliumGeneralStatus = An<IGeneralStatus>();
            legalEntityDomicilium = An<ILegalEntityDomicilium>();
            applicationRepository = An<IApplicationRepository>();
            legalEntityRepository = An<ILegalEntityRepository>();
            application = An<IApplication>();
            applicationRole = An<IApplicationRole>();

            address.WhenToldTo(x => x.Key).Return(12);
            application.WhenToldTo(x=>x.IsOpen).Return(true);
            application.WhenToldTo(x=>x.Key).Return(33);
            applicationRole.WhenToldTo(x=>x.Key).Return(12);
            applicationRole.WhenToldTo(x => x.Application).Return(application);
            
            applicationRoleDomicilium.WhenToldTo(x=>x.ApplicationRole).Return(applicationRole);
            legalEntity.WhenToldTo(x => x.Key).Return(14);
            legalEntity.WhenToldTo(x=>x.DisplayName).Return("Display Name");
            applicationRole.WhenToldTo(x=>x.LegalEntity).Return(legalEntity);
            legalEntityAddress.WhenToldTo(x => x.Address).Return(address);
            legalEntityAddress.WhenToldTo(x => x.LegalEntity).Return(legalEntity);
            legalEntityAddress.WhenToldTo(x => x.Key).Return(5);
            legalEntityDomiciliumGeneralStatus.WhenToldTo(x => x.Key).Return((int)GeneralStatuses.Pending);
            legalEntityDomicilium.WhenToldTo(x => x.GeneralStatus).Return(legalEntityDomiciliumGeneralStatus);
            legalEntityDomicilium.WhenToldTo(x => x.LegalEntityAddress).Return(legalEntityAddress);

            List<ILegalEntityDomicilium> legalEntityDomiciliums = new List<ILegalEntityDomicilium>();
            legalEntityDomiciliums.Add(legalEntityDomicilium);
            List<IApplicationRoleDomicilium> applicationDomiciliums = new List<IApplicationRoleDomicilium>();
            applicationDomiciliums.Add(applicationRoleDomicilium);

            EventList<IApplicationRoleDomicilium> applicationRoleDomiciliums = new EventList<IApplicationRoleDomicilium>(applicationDomiciliums);

            applicationRepository.WhenToldTo(x=>x.GetApplicationsThatUseLegalEntityDomicilium(legalEntityAddress)).Return(applicationRoleDomiciliums);
            legalEntityRepository.WhenToldTo(x => x.GetLegalEntityDomiciliumsForAddressKey(12)).Return(new EventList<ILegalEntityDomicilium>(legalEntityDomiciliums));

            businessRule = new SAHL.Common.BusinessModel.Rules.LegalEntityAddress.LegalEntityAddressChangesWillUpdatePendingDomicilium(legalEntityRepository, applicationRepository);
            RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.LegalEntityAddress.LegalEntityAddressChangesWillUpdatePendingDomicilium>.startrule.Invoke();
        };

        Because of = () =>
        {
            businessRule.ExecuteRule(messages, legalEntityAddress);
        };

        It should_fail = () =>
        {
            messages.Count.ShouldEqual(1);
        };
    }
}
