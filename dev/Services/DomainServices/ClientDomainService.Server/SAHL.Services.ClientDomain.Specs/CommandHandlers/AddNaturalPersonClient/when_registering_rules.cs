using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Validation;
using SAHL.Core.Rules;
using SAHL.Core.Testing;
using SAHL.Services.ClientDomain.CommandHandlers;
using SAHL.Services.ClientDomain.Managers;
using SAHL.Services.ClientDomain.Rules;
using SAHL.Services.Interfaces.ClientDomain.Models;

namespace SAHL.Services.ClientDomain.Specs.CommandHandlers.AddNaturalPersonClient
{
    public class when_registering_rules : WithCoreFakes
    {
        private static AddNaturalPersonClientCommandHandler handler;
        private static IClientDataManager clientDataManager;
        private static IDomainRuleManager<INaturalPersonClientModel> domainRuleManager;
        private static IValidationUtils validationUtils;

        private Establish context = () =>
        {
            clientDataManager = An<IClientDataManager>();
            domainRuleManager = An<IDomainRuleManager<INaturalPersonClientModel>>();
            validationUtils = An<IValidationUtils>();
        };

        private Because of = () =>
        {
            handler = new AddNaturalPersonClientCommandHandler(clientDataManager, linkedKeyManager, eventRaiser, domainRuleManager, unitOfWorkFactory, validationUtils);
        };

        private It should_register_the_id_number_rule = () =>
        {
            domainRuleManager.WasToldTo(x => x.RegisterRule(Param.IsAny<IdNumberMustBeValidWhenProvidedForASACitizenRule>()));
        };

        private It should_register_the_DOB_rule = () =>
        {
            domainRuleManager.WasToldTo(x => x.RegisterRule(Param.IsAny<ClientDateOfBirthMustBePriorToTodayRule>()));
        };

        private It should_register_the_passport_rule = () =>
        {
            domainRuleManager.WasToldTo(x => x.RegisterRule(Param.IsAny<PassportNumberMustBeValidWhenProvidedRule>()));
        };

        private It should_register_the_passport_cannot_be_an_IDNumber_rule = () =>
        {
            domainRuleManager.WasToldTo(x => x.RegisterRule(Param.IsAny<PassportNumberCannotBeAValidIdentityNumberRule>()));
        };

        private It should_register_the_idNumber_unique_rule = () =>
        {
            domainRuleManager.WasToldTo(x => x.RegisterRule(Param.IsAny<IdNumberMustBeUniqueRule>()));
        };

        private It should_register_the_passport_unique_rule = () =>
        {
            domainRuleManager.WasToldTo(x => x.RegisterRule(Param.IsAny<PassportNumberMustBeUniqueRule>()));
        };

        private It should_register_the_contact_details_rule = () =>
        {
            domainRuleManager.WasToldTo(x => x.RegisterPartialRule(Param.IsAny<AtLeastOneClientContactDetailShouldBeProvidedRule>()));
        };
    }
}


