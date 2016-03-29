using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Globals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Fakes;

namespace SAHL.Common.BusinessModel.Specs.Rules.LegalEntity.AddressActiveLegalEntityDomiciliumCanNotBeDeleted
{
    [Subject(typeof(SAHL.Common.BusinessModel.Rules.LegalEntityAddress.LegalEntityAddressCanNotBeDeletedIfItIsTheActiveLegalEntityDomicilium))]
    public class when_trying_to_delete_a_legalEntityAddress_linked_to_inactive_domicilium : RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.LegalEntityAddress.LegalEntityAddressCanNotBeDeletedIfItIsTheActiveLegalEntityDomicilium>
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
            legalEntityDomiciliumGeneralStatus.WhenToldTo(x => x.Key).Return((int)GeneralStatuses.Inactive);
            legalEntityDomicilium.WhenToldTo(x => x.GeneralStatus).Return(legalEntityDomiciliumGeneralStatus);
            legalEntityDomicilium.WhenToldTo(x => x.LegalEntityAddress).Return(legalEntityAddress);

            List<ILegalEntityDomicilium> legalEntityDomiciliums = new List<ILegalEntityDomicilium>();
            legalEntityDomiciliums.Add(legalEntityDomicilium);

            legalEntityRepository.WhenToldTo(x => x.GetLegalEntityDomiciliumsForAddressKey(12)).Return(new EventList<ILegalEntityDomicilium>(legalEntityDomiciliums));

            businessRule = new BusinessModel.Rules.LegalEntityAddress.LegalEntityAddressCanNotBeDeletedIfItIsTheActiveLegalEntityDomicilium(legalEntityRepository);
            RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.LegalEntityAddress.LegalEntityAddressCanNotBeDeletedIfItIsTheActiveLegalEntityDomicilium>.startrule.Invoke();
        };

        Because of = () =>
        {
            businessRule.ExecuteRule(messages, legalEntityAddress);
        };

        It should_not_return_messages = () =>
        {
            messages.Count.ShouldEqual(0);
        };
    }
}
