using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Validation;
using SAHL.Core.SystemMessages;
using SAHL.Services.ClientDomain.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.ClientDomain.Specs.Rules.AtLeastOneClientContactDetailShouldBeProvidedSpecs
{
    public class when_only_the_home_phone_number_is_provided : WithFakes
    {
        private static AtLeastOneClientContactDetailShouldBeProvidedRule rule;
        private static IValidationUtils validationUtils;
        private static ClientContactDetailsTestModel model;
        private static ISystemMessageCollection messages;

        private Establish context = () =>
        {
            messages = SystemMessageCollection.Empty();
            validationUtils = An<IValidationUtils>();
            model = new ClientContactDetailsTestModel(string.Empty, "7655528", string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
            rule = new AtLeastOneClientContactDetailShouldBeProvidedRule();
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, model);
        };

        private It should_return_an_error_message = () =>
        {
            messages.ErrorMessages().First().Message.ShouldEqual("At least one valid contact detail (An Email Address, Home, Work or Cell Number) is required.");
        };
    }
}