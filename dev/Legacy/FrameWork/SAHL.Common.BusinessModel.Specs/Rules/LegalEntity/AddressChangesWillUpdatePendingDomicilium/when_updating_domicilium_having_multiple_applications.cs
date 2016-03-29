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
    public class when_updating_domicilium_having_multiple_applications : RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.LegalEntityAddress.LegalEntityAddressChangesWillUpdatePendingDomicilium>
    {
        protected static ILegalEntity legalEntity;
        protected static ILegalEntityAddress legalEntityAddress;
        protected static IAddress address;
        protected static IApplicationRepository applicationRepository;
        protected static ILegalEntityDomicilium legalEntityDomicilium;
        protected static ILegalEntityRepository legalEntityRepository;
        protected static IGeneralStatus legalEntityDomiciliumGeneralStatus;
        protected static IApplicationRoleDomicilium applicationRoleDomicilium;
        protected static IApplicationRoleDomicilium applicationRoleDomicilium2;
        protected static IApplication application;
        protected static IApplication application2;
        protected static IApplicationRole applicationRole;
        protected static IApplicationRole applicationRole2;

        Establish context = () =>
        {
            legalEntity = An<ILegalEntity>();
            applicationRoleDomicilium = An<IApplicationRoleDomicilium>();
            applicationRoleDomicilium2 = An<IApplicationRoleDomicilium>();
            legalEntityAddress = An<ILegalEntityAddress>();
            address = An<IAddress>();
            legalEntityDomiciliumGeneralStatus = An<IGeneralStatus>();
            legalEntityDomicilium = An<ILegalEntityDomicilium>();
            applicationRepository = An<IApplicationRepository>();
            legalEntityRepository = An<ILegalEntityRepository>();
            application = An<IApplication>();
            application2 = An<IApplication>();
            applicationRole = An<IApplicationRole>();
            applicationRole2 = An<IApplicationRole>();

            address.WhenToldTo(x => x.Key).Return(12);
            application.WhenToldTo(x=>x.IsOpen).Return(true);
            application.WhenToldTo(x=>x.Key).Return(33);
            applicationRole.WhenToldTo(x=>x.Key).Return(12);
            applicationRole.WhenToldTo(x => x.Application).Return(application);
            application2.WhenToldTo(x => x.IsOpen).Return(true);
            application2.WhenToldTo(x => x.Key).Return(37);
            applicationRole2.WhenToldTo(x => x.Key).Return(19);
            applicationRole2.WhenToldTo(x => x.Application).Return(application2);
            
            applicationRoleDomicilium.WhenToldTo(x=>x.ApplicationRole).Return(applicationRole);
            applicationRoleDomicilium2.WhenToldTo(x => x.ApplicationRole).Return(applicationRole2);
            legalEntity.WhenToldTo(x => x.Key).Return(14);
            legalEntity.WhenToldTo(x=>x.DisplayName).Return("Display Name");
            applicationRole.WhenToldTo(x=>x.LegalEntity).Return(legalEntity);
            applicationRole2.WhenToldTo(x => x.LegalEntity).Return(legalEntity);
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
            applicationDomiciliums.Add(applicationRoleDomicilium2);

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
            messages.Count.ShouldEqual(2);
        };
    }
}

    
