using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Validation;
using SAHL.Core.SystemMessages;
using SAHL.Services.ClientDomain.Rules;
using System;
using System.Linq;

namespace SAHL.Services.ClientDomain.Specs.Rules.AtLeastOneClientContactDetailShouldBeProvidedSpecs
{
    public class when_only_the_home_phone_code_and_number_are_provided : WithFakes
    {
        private static AtLeastOneClientContactDetailShouldBeProvidedRule rule;
        private static IValidationUtils validationUtils;
        private static ClientContactDetailsTestModel model;
        private static ISystemMessageCollection messages;

        private Establish context = () =>
        {
            messages = SystemMessageCollection.Empty();
            validationUtils = An<IValidationUtils>();
            model = new ClientContactDetailsTestModel("031", "7665544", string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, null);
            rule = new AtLeastOneClientContactDetailShouldBeProvidedRule();
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, model);
        };

        private It should_return_no_messages = () =>
        {
            messages.ErrorMessages().ShouldBeEmpty();
        };
    }
}