using Machine.Specifications;
using Machine.Fakes;
using SAHL.Common.BusinessModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Globals;
using SAHL.Common.Collections;

namespace SAHL.Common.BusinessModel.Specs.Rules.LegalEntity.AddressChangesWillUpdateActiveDomicilium
{
    [Subject(typeof(SAHL.Common.BusinessModel.Rules.LegalEntityAddress.LegalEntityAddressChangesWillUpdateActiveDomicilium))]
    public class when_trying_to_update_legalentityaddress_that_is_the_active_legalentitydomicilium : RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.LegalEntityAddress.LegalEntityAddressChangesWillUpdateActiveDomicilium>
    {
        protected static ILegalEntity legalEntity;
        protected static ILegalEntityAddress legalEntityAddress;
        protected static IAddress address;
        protected static ILegalEntityDomicilium legalEntityDomicilium;
        protected static ILegalEntityRepository legalEntityRepository;
        protected static IGeneralStatus legalEntityDomiciliumGeneralStatus;
        Establish context = () =>
        {
            legalEntity = An<ILegalEntity>();
            legalEntityAddress = An<ILegalEntityAddress>();
            address = An<IAddress>();
            legalEntityDomiciliumGeneralStatus = An<IGeneralStatus>();
            legalEntityDomicilium = An<ILegalEntityDomicilium>();

            legalEntityRepository = An<ILegalEntityRepository>();

            address.WhenToldTo(x => x.Key).Return(12);

            legalEntity.WhenToldTo(x => x.Key).Return(14);
            legalEntityAddress.WhenToldTo(x => x.Address).Return(address);
            legalEntityAddress.WhenToldTo(x => x.LegalEntity).Return(legalEntity);
            legalEntityAddress.WhenToldTo(x => x.Key).Return(5);
            legalEntityDomiciliumGeneralStatus.WhenToldTo(x => x.Key).Return((int)GeneralStatuses.Active);
            legalEntityDomicilium.WhenToldTo(x => x.GeneralStatus).Return(legalEntityDomiciliumGeneralStatus);
            legalEntityDomicilium.WhenToldTo(x => x.LegalEntityAddress).Return(legalEntityAddress);

            List<ILegalEntityDomicilium> legalEntityDomiciliums = new List<ILegalEntityDomicilium>();
            legalEntityDomiciliums.Add(legalEntityDomicilium);

            legalEntityRepository.WhenToldTo(x => x.GetLegalEntityDomiciliumsForAddressKey(12)).Return(new EventList<ILegalEntityDomicilium>(legalEntityDomiciliums));

            businessRule = new BusinessModel.Rules.LegalEntityAddress.LegalEntityAddressChangesWillUpdateActiveDomicilium(legalEntityRepository);
            RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.LegalEntityAddress.LegalEntityAddressChangesWillUpdateActiveDomicilium>.startrule.Invoke();
        };

        Because of = () =>
        {
            businessRule.ExecuteRule(messages, legalEntityAddress);
        };

        It rule_should_fail = () =>
        {
            messages.Count.ShouldEqual(1);
        };
    }
}
