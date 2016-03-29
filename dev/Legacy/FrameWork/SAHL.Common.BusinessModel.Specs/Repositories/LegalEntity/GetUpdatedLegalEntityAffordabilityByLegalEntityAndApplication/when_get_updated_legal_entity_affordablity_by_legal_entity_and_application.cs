using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAHL.Common.BusinessModel.Specs.Repositories.LegalEntity.GetUpdatedLegalEntityAffordabilityByLegalEntityAndApplication
{
    [Subject(typeof(LegalEntityRepository))]
    public class LegalEntityRepositoryFakeBase : WithFakes
    {
        protected static ILegalEntity legalEntity;
        protected static ILegalEntity updatedLegalEntity;
        protected static IApplication application;

        protected static ICastleTransactionsService castleTransactionsService;

        protected static ILegalEntityRepository legalEntityRepository;
        protected static ILegalEntityAffordability legalEntityAffordability1;
        protected static ILegalEntityAffordability legalEntityAffordability2;
        protected static IApplicationRepository applicationRepository;

        protected static IAffordabilityType affordabilityTypeBasicSalary;
        protected static IAffordabilityType affordabilityTypeCreditCard;
        protected static IAffordabilityType affordabilityTypeFood;
        protected static IAffordabilityType affordabilityTypeMedical;
        protected static IAffordabilityType affordabilityTypeTransport;

        protected static IList<IAffordabilityType> availableAffordabilityTypes;
    }

    public class when_asked_to_get_updated_legal_entity_affordablity_by_legal_entity_and_application : LegalEntityRepositoryFakeBase
    {
        Establish context = () =>
        {
            legalEntity = An<ILegalEntity>();
            updatedLegalEntity = An<ILegalEntity>();
            application = An<IApplication>();
            applicationRepository = An<IApplicationRepository>();

            
            //legalEntityRepository = An<ILegalEntityRepository>();
            legalEntityAffordability1 = An<ILegalEntityAffordability>();
            legalEntityAffordability2 = An<ILegalEntityAffordability>();

            affordabilityTypeBasicSalary = An<IAffordabilityType>();
            affordabilityTypeCreditCard = An<IAffordabilityType>();
            affordabilityTypeFood = An<IAffordabilityType>();
            affordabilityTypeMedical = An<IAffordabilityType>();
            affordabilityTypeTransport = An<IAffordabilityType>();

            // LegalEntityAffordablitities
            legalEntityAffordability1.WhenToldTo(x => x.AffordabilityType.Key).Return(1);
            legalEntityAffordability1.WhenToldTo(x => x.Amount).Return(10000);
            legalEntityAffordability2.WhenToldTo(x => x.AffordabilityType.Key).Return(2);
            legalEntityAffordability2.WhenToldTo(x => x.Amount).Return(800);

            // AvailableAffordabilityTypes
            affordabilityTypeBasicSalary.WhenToldTo(x => x.Key).Return(1);
            affordabilityTypeCreditCard.WhenToldTo(x => x.Key).Return(11);
            affordabilityTypeFood.WhenToldTo(x => x.Key).Return(8);
            affordabilityTypeMedical.WhenToldTo(x => x.Key).Return(18);
            affordabilityTypeTransport.WhenToldTo(x => x.Key).Return(22);

            //castleTransactionsService = An<ICastleTransactionsService>();
            legalEntityRepository = new LegalEntityRepository();

            var legalEntityAffordabilities = new EventList<ILegalEntityAffordability>(
                new[] { legalEntityAffordability1, legalEntityAffordability2 });

            availableAffordabilityTypes = new EventList<IAffordabilityType>(
                new[] { affordabilityTypeBasicSalary, affordabilityTypeCreditCard, affordabilityTypeFood, affordabilityTypeMedical, affordabilityTypeTransport }).ToList();

            legalEntity.WhenToldTo(x => x.LegalEntityAffordabilities).Return(legalEntityAffordabilities);
            legalEntity.WhenToldTo(x => x.Key).Return(232009);

            application.WhenToldTo(x => x.Key).Return(1231429);
            applicationRepository.WhenToldTo(x => x.GetApplicationByKey(Param.IsAny<int>())).Return(application);

            //legalEntityRepository.WhenToldTo(x => x.GetAffordabilityTypes()).Return(availableAffordabilityTypes);

        };

        Because of = () =>
        {
            updatedLegalEntity = legalEntityRepository.GetUpdatedLegalEntityAffordabilityByLegalEntityAndApplication(availableAffordabilityTypes,legalEntity.Key, application.Key);
        };

        It should_call_GetUpdatedLegalEntityAffordabilityByLegalEntityAndApplication = () =>
        {
            //legalEntityRepository.WasToldTo(x => x.GetUpdatedLegalEntityAffordabilityByLegalEntityAndApplication(availableAffordabilityTypes, Param.IsAny<int>(), Param.IsAny<int>()));
        };

        It should_return_a_value = () =>
        {
            //updatedLegalEntity.ShouldNotBeNull(); 
        };

        //It should_return_a_legalEntity_containing_updated_legalentityAffordabilities = () =>
        //{
            
        //};
    }
}
