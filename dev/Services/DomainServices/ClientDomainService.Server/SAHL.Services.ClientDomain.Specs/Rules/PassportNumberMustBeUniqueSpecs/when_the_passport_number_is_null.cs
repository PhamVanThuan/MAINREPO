using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.BusinessModel.Validation;
using SAHL.Core.SystemMessages;
using SAHL.Services.ClientDomain.Managers;
using SAHL.Services.ClientDomain.Rules;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.ClientDomain.Specs.Rules.PassportNumberMustBeUniqueSpecs
{
    public class when_the_passport_number_is_null : WithFakes
    {
        private static PassportNumberMustBeUniqueRule rule;
        private static IValidationUtils validationUtils;
        private static IClientDataManager clientDataManager;
        private static NaturalPersonClientModel model;
        private static ISystemMessageCollection messages;

        Establish context = () =>
        {
            messages = SystemMessageCollection.Empty();
            validationUtils = An<IValidationUtils>();
            clientDataManager = An<IClientDataManager>();
            rule = new PassportNumberMustBeUniqueRule(validationUtils, clientDataManager);
            model = new NaturalPersonClientModel("8211045229080", null, SalutationType.Mr, "Clint", "Speed", "C", "Clint", Gender.Male,
            MaritalStatus.Married_AnteNuptualContract, PopulationGroup.African, CitizenType.SACitizen, DateTime.Now.AddYears(-31), Language.English,
            CorrespondenceLanguage.English, Education.Diploma, "031", "7657192", string.Empty, string.Empty, string.Empty, string.Empty,
            string.Empty, "test@sahomeloans.com");
        };

        Because of = () =>
        {
            rule.ExecuteRule(messages, model);
        };

        It should_not_use_the_client_data_manager_to_find_an_existing_client = () =>
        {
            clientDataManager.WasNotToldTo(x => x.FindExistingClientByPassportNumber(model.PassportNumber));
        };

        It should_not_return_an_error_message = () =>
        {
            messages.ErrorMessages().ShouldBeEmpty();
        };

        It should_not_validate_the_passport_number = () =>
        {
            validationUtils.WasNotToldTo(x => x.ValidatePassportNumber(model.PassportNumber));
        };
    }
}
