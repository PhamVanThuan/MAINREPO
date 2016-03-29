using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.BusinessModel.Validation;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.SystemMessages;
using SAHL.Services.ClientDomain.Managers;
using SAHL.Services.ClientDomain.Rules;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;
using System.Linq;

namespace SAHL.Services.ClientDomain.Specs.Rules.PassportNumberMustBeUniqueSpecs
{
    public class when_the_passport_number_is_valid_and_doesnt_exist : WithFakes
    {
        private static PassportNumberMustBeUniqueRule rule;
        private static IValidationUtils validationUtils;
        private static IClientDataManager clientDataManager;
        private static NaturalPersonClientModel model;
        private static ISystemMessageCollection messages;
        private static LegalEntityDataModel existingClient;

        private Establish context = () =>
            {
                existingClient = null;
                messages = SystemMessageCollection.Empty();
                validationUtils = An<IValidationUtils>();
                clientDataManager = An<IClientDataManager>();
                rule = new PassportNumberMustBeUniqueRule(validationUtils, clientDataManager);
                model = new NaturalPersonClientModel("8211045229080", "A03396521", SalutationType.Mr, "Clint", "Speed", "C", "Clint", Gender.Male,
                MaritalStatus.Married_AnteNuptualContract, PopulationGroup.African, CitizenType.SACitizen, DateTime.Now.AddYears(-31), Language.English,
                CorrespondenceLanguage.English, Education.Diploma, "031", "7657192", string.Empty, string.Empty, string.Empty, string.Empty,
                string.Empty, "test@sahomeloans.com");
                validationUtils.WhenToldTo(x => x.ValidatePassportNumber(model.PassportNumber)).Return(true);
                clientDataManager.WhenToldTo(x => x.FindExistingClientByPassportNumber(model.PassportNumber)).Return(existingClient);
            };

        private Because of = () =>
            {
                rule.ExecuteRule(messages, model);
            };

        private It should_use_the_client_data_manager_to_find_an_existing_client = () =>
        {
            clientDataManager.WasToldTo(x => x.FindExistingClientByPassportNumber(model.PassportNumber));
        };

        private It should_not_return_an_error_message = () =>
        {
            messages.ErrorMessages().ShouldBeEmpty();
        };
    }
}