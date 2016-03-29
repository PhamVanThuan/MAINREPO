using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Identity;
using SAHL.Core.SystemMessages;
using SAHL.Services.ClientDomain.Rules;
using SAHL.Services.Interfaces.ClientDomain.Commands;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;

namespace SAHL.Services.ClientDomain.Specs.RuleSpecs.PassportNumberMustBeValidForForeignClientSpecs
{
    public class when_non_resident_and_a_valid_passport_number_is_provided : WithFakes
    {
        private static PassportNumberMustBeValidWhenProvidedRule rule;
        private static ISystemMessageCollection messages;
        private static NaturalPersonClientModel model;
        private static string idNumber, passportNumber;
        private static CitizenType citizenshipTypeKey;

        private Establish context = () =>
        {
            messages = SystemMessageCollection.Empty();
            idNumber = string.Empty;
            passportNumber = "1967677401";
            citizenshipTypeKey = CitizenType.Non_Resident;
            rule = new PassportNumberMustBeValidWhenProvidedRule();
            model = new NaturalPersonClientModel(idNumber, passportNumber, SalutationType.Mr, "Clint", "Speed", "C", "Clint", Gender.Male,
                MaritalStatus.Married_AnteNuptualContract, PopulationGroup.African, citizenshipTypeKey, DateTime.Now.AddYears(-31), Language.English,
                CorrespondenceLanguage.English, Education.Diploma, "031", "7657192", string.Empty, string.Empty, string.Empty, string.Empty,
                string.Empty, "test@sahomeloans.com");
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, model);
        };

        private It should_not_return_an_error_message = () =>
        {
            messages.ErrorMessages().ShouldBeEmpty();
        };
    }
}
