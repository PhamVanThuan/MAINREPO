using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Validation;
using SAHL.Core.Rules;
using SAHL.Core.Testing;
using SAHL.Services.ClientDomain.CommandHandlers;
using SAHL.Services.ClientDomain.Managers;
using SAHL.Services.ClientDomain.Rules;
using SAHL.Services.ClientDomain.Rules.Models;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;
using System.Linq;

namespace SAHL.Services.ClientDomain.Specs.CommandHandlers.UpdateInactiveNaturalPersonClient
{
    public class when_registering_rules : WithCoreFakes
    {
        private static UpdateInactiveNaturalPersonClientCommandHandler handler;
        private static IClientDataManager clientDataManager;
        private static IDomainRuleManager<NaturalPersonClientRuleModel> domainRuleManager;
        private static IValidationUtils validationUtils;

        private Establish context = () =>
        {
           clientDataManager = An<IClientDataManager>();
           validationUtils = An<IValidationUtils>();
           domainRuleManager = An<IDomainRuleManager<NaturalPersonClientRuleModel>>();
        };

        private Because of = () =>
        {
                handler = new UpdateInactiveNaturalPersonClientCommandHandler(clientDataManager, eventRaiser, domainRuleManager, validationUtils);
        };

        private It should_register_the_identity_number_must_be_valid_when_provided_for_a_SA_citizen_rule = () =>
        {
            domainRuleManager.WasToldTo
             (x => x.RegisterPartialRule(Param.IsAny<IdNumberMustBeValidWhenProvidedForASACitizenRule>()));
        };

        private It should_register_the_client_date_of_birth_must_be_prior_to_today_rule = () =>
        {
            domainRuleManager.WasToldTo
             (x => x.RegisterPartialRule(Param.IsAny<ClientDateOfBirthMustBePriorToTodayRule>()));
        };

        private It should_register_the_passport_number_must_be_valid_when_provided_rule = () =>
        {
            domainRuleManager.WasToldTo
             (x => x.RegisterPartialRule(Param.IsAny<PassportNumberMustBeValidWhenProvidedRule>()));
        };

        private It should_register_the_passport_number_cannot_be_a_valid_identity_number_rule = () =>
        {
            domainRuleManager.WasToldTo
             (x => x.RegisterPartialRule(Param.IsAny<PassportNumberCannotBeAValidIdentityNumberRule>()));
        };

        private It should_register_the_client_contact_details_rule = () =>
        {
            domainRuleManager.WasToldTo
             (x => x.RegisterPartialRule<IClientContactDetails>(Param.IsAny<AtLeastOneClientContactDetailShouldBeProvidedRule>()));
        };

        private It should_register_the_client_cannot_be_linked_to_an_open_application_rule = () =>
        {
            domainRuleManager.WasToldTo
             (x => x.RegisterRule(Param.IsAny<ClientCannotBeLinkedToAnOpenApplicationRule>()));
        };

        private It should_register_the_client_cannot_be_linked_to_an_open_account_rule = () =>
        {
            domainRuleManager.WasToldTo
             (x => x.RegisterRule(Param.IsAny<ClientCannotBeLinkedToAnOpenAccountRule>()));
        };
    }
}