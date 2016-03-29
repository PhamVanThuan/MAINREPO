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
    public class when_all_contact_details_are_valid : WithFakes
    {
        private static AtLeastOneClientContactDetailShouldBeProvidedRule rule;
        private static IValidationUtils validationUtils;
        private static ClientContactDetailsTestModel model;
        private static ISystemMessageCollection messages;

        private Establish context = () =>
        {
            messages = SystemMessageCollection.Empty();
            validationUtils = An<IValidationUtils>();
            model = new ClientContactDetailsTestModel("031", "7657192", "031", "5713036", "0860", "6006001", "0827702444", "clintons@sahomeloans.com");
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