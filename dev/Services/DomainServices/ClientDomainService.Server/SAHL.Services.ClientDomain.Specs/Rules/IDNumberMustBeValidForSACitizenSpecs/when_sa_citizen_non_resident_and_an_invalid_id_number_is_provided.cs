using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.BusinessModel.Validation;
using SAHL.Core.Identity;
using SAHL.Core.SystemMessages;
using SAHL.Services.ClientDomain.Rules;
using SAHL.Services.Interfaces.ClientDomain.Commands;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;

namespace SAHL.Services.ClientDomain.Specs.RuleSpecs.IDNumberMustBeValidForSACitizen
{
    public class when_sa_citizen_non_resident_and_an_invalid_id_number_is_provided : WithFakes
    {
        private static IdNumberMustBeValidWhenProvidedForASACitizenRule rule;
        private static ISystemMessageCollection messages;
        private static NaturalPersonClientModel model;
        private static string idNumber, passportNumber;
        private static CitizenType citizenshipTypeKey;
        private static IValidationUtils validationUtils;

        private Establish context = () =>
        {
            validationUtils = An<IValidationUtils>();
            messages = SystemMessageCollection.Empty();
            idNumber = "840512509123";
            passportNumber = string.Empty;
            citizenshipTypeKey = CitizenType.SACitizen_NonResident;
            model = new NaturalPersonClientModel(idNumber, passportNumber, SalutationType.Mr, "Clint", "Speed", "C", "Clint", Gender.Male,
            MaritalStatus.Married_AnteNuptualContract, PopulationGroup.African, citizenshipTypeKey, DateTime.Now.AddYears(-31), Language.English,
            CorrespondenceLanguage.English, Education.Diploma, "031", "7657192", string.Empty, string.Empty, string.Empty, string.Empty,
            string.Empty, "test@sahomeloans.com");
            rule = new IdNumberMustBeValidWhenProvidedForASACitizenRule(validationUtils);
            validationUtils.WhenToldTo(x => x.ValidateIDNumber(Param.IsAny<string>())).Return(false);
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, model);
        };

        private It should_return_an_error_message = () =>
        {
            messages.ErrorMessages().ShouldContain(x => x.Message == "The clients Identity Number is invalid.");
        };
    }
}
