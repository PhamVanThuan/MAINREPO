using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.BusinessModel.Validation;
using SAHL.Core.SystemMessages;
using SAHL.Services.ClientDomain.Rules;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;
using System.Linq;

namespace SAHL.Services.ClientDomain.Specs.Rules.PassportNumberCannotBeAValidIdentityNumberSpecs
{
    public class when_the_passport_is_a_valid_ID_number : WithFakes
    {
        private static PassportNumberCannotBeAValidIdentityNumberRule rule;
        private static NaturalPersonClientModel model;
        private static ISystemMessageCollection messages;
        private static string idNumber;
        private static IValidationUtils validationUtils;

        private Establish context = () =>
        {
            validationUtils = An<IValidationUtils>();
            idNumber = "8211045229080";
            messages = SystemMessageCollection.Empty();
            rule = new PassportNumberCannotBeAValidIdentityNumberRule(validationUtils);
            model = new NaturalPersonClientModel(idNumber, idNumber, SalutationType.Mr, "Clint", "Speed", "C", "Clint", Gender.Male,
            MaritalStatus.Married_AnteNuptualContract, PopulationGroup.African, CitizenType.SACitizen, DateTime.Now.AddYears(-24), Language.English,
            CorrespondenceLanguage.English, Education.Diploma, "031", "7657192", string.Empty, string.Empty, string.Empty, string.Empty,
            string.Empty, "test@sahomeloans.com");
            validationUtils.WhenToldTo(x => x.ValidateIDNumber(Param.IsAny<string>())).Return(true);
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, model);
        };

        private It should_return_no_errors = () =>
        {
            messages.ErrorMessages().First().Message.ShouldEqual("A passport number cannot be a valid identity number.");
        };
    }
}