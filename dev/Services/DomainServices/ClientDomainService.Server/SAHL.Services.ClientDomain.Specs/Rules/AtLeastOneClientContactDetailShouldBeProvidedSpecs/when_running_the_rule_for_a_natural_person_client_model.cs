using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.BusinessModel.Validation;
using SAHL.Core.SystemMessages;
using SAHL.Services.ClientDomain.Rules;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;
using System.Linq;

namespace SAHL.Services.ClientDomain.Specs.Rules.AtLeastOneClientContactDetailShouldBeProvidedSpecs
{
    public class when_running_the_rule_for_a_natural_person_client_model : WithFakes
    {
        private static AtLeastOneClientContactDetailShouldBeProvidedRule rule;
        private static IValidationUtils validationUtils;
        private static NaturalPersonClientModel model;
        private static ISystemMessageCollection messages;

        private Establish context = () =>
        {
            messages = SystemMessageCollection.Empty();
            validationUtils = An<IValidationUtils>();
            model = new NaturalPersonClientModel("8211045229080", string.Empty, SalutationType.Mr, "Clint", "Speed", "C", "Clint", Gender.Male,
            MaritalStatus.Married_AnteNuptualContract, PopulationGroup.African, CitizenType.SACitizen, DateTime.Now.AddYears(-25), Language.English,
            CorrespondenceLanguage.English, Education.Diploma, "031", "7657192", "021", "2211221", "0860", "1122334",
             "0827702444", "test@sahomeloans.com");
            rule = new AtLeastOneClientContactDetailShouldBeProvidedRule();
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, model);
        };

        private It should_not_return_error_messages = () =>
        {
            messages.ErrorMessages().ShouldBeEmpty();
        };
    }
}