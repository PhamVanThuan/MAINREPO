using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Fakes;
using SAHL.Common.Globals;
using SAHL.Common.Collections;

namespace SAHL.Common.BusinessModel.Specs.Rules.LegalEntity.AddressChangesWillUpdateActiveDomicilium
{
    [Subject(typeof(SAHL.Common.BusinessModel.Rules.LegalEntityAddress.LegalEntityAddressChangesWillUpdateActiveDomicilium))]
    public class when_trying_to_update_legalentityaddress_with_a_shared_address : RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.LegalEntityAddress.LegalEntityAddressChangesWillUpdateActiveDomicilium>
    {
        protected static ILegalEntity legalEntity;
        protected static ILegalEntity relatedLegalEntity;
        protected static ILegalEntityAddress relatedlegalEntityAddress;
        protected static ILegalEntityAddress legalEntityAddress;
        protected static IAddress address;
        protected static ILegalEntityDomicilium legalEntityDomicilium;
        protected static ILegalEntityDomicilium relatedlegalEntityDomicilium;
        protected static ILegalEntityRepository legalEntityRepository;
        protected static IGeneralStatus legalEntityDomiciliumGeneralStatus;
        Establish context = () =>
        {

            address = An<IAddress>();
            address.WhenToldTo(x => x.Key).Return(12);

            legalEntity = An<ILegalEntity>();
            legalEntityAddress = An<ILegalEntityAddress>();
            legalEntityDomiciliumGeneralStatus = An<IGeneralStatus>();
            legalEntityDomicilium = An<ILegalEntityDomicilium>();
            legalEntityRepository = An<ILegalEntityRepository>();

            legalEntity.WhenToldTo(x => x.Key).Return(14);
            legalEntityAddress.WhenToldTo(x => x.Address).Return(address);
            legalEntityAddress.WhenToldTo(x => x.LegalEntity).Return(legalEntity);
            legalEntityAddress.WhenToldTo(x => x.Key).Return(5);
            legalEntityDomiciliumGeneralStatus.WhenToldTo(x => x.Key).Return((int)GeneralStatuses.Active);
            legalEntityDomicilium.WhenToldTo(x => x.GeneralStatus).Return(legalEntityDomiciliumGeneralStatus);
            legalEntityDomicilium.WhenToldTo(x => x.LegalEntityAddress).Return(legalEntityAddress);

            relatedLegalEntity = An<ILegalEntity>();
            relatedlegalEntityAddress = An<ILegalEntityAddress>();
            relatedlegalEntityDomicilium = An<ILegalEntityDomicilium>();
            relatedLegalEntity.WhenToldTo(x => x.Key).Return(1);
            relatedlegalEntityAddress.WhenToldTo(x => x.Key).Return(6);
            relatedlegalEntityAddress.WhenToldTo(x => x.Address).Return(address);
            relatedlegalEntityAddress.WhenToldTo(x => x.LegalEntity).Return(relatedLegalEntity);
            relatedlegalEntityDomicilium.WhenToldTo(x => x.GeneralStatus).Return(legalEntityDomiciliumGeneralStatus);
            relatedlegalEntityDomicilium.WhenToldTo(x => x.LegalEntityAddress).Return(relatedlegalEntityAddress);


            List<ILegalEntityDomicilium> legalEntityDomiciliums = new List<ILegalEntityDomicilium>();
            legalEntityDomiciliums.Add(legalEntityDomicilium);
            legalEntityDomiciliums.Add(relatedlegalEntityDomicilium);

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
