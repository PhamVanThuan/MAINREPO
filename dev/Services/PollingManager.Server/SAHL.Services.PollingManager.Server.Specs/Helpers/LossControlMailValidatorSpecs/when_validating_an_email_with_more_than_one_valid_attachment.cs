using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Exchange;
using SAHL.Services.PollingManager.Server.Tests;
using SAHL.Services.PollingManager.Validators;
using System;
using System.Linq;

namespace SAHL.Services.PollingManager.Server.Specs.Helpers.LossControlMailValidatorSpecs
{
    public class when_validating_an_email_with_more_than_one_valid_attachment : WithFakes
    {
        private static LossControlMailValidator lossControlMailValidator;
        private static IMailMessage mail;
        private static bool result;

        private Establish context = () =>
        {
            mail = FakeMailMessageFactory.GetMailMessageWithTwoValidAttachments();
            result = true;
            lossControlMailValidator = new LossControlMailValidator();
        };

        private Because of = () =>
        {
            result = lossControlMailValidator.ValidateMail(mail);
        };

        private It should_be_false = () =>
        {
            result.ShouldEqual(false);
        };

        private It should_add_the_correct_error = () =>
        {
            lossControlMailValidator.Errors.ShouldNotBeEmpty();
        };

        private It should_decline_the_email_with_the_correct_error_message = () =>
        {
            lossControlMailValidator.Errors.First().ShouldEqual("The email contained more than one attachment in either PDF or TIFF format.");
        };

        private It should_change_isValid_to_false = () =>
        {
            lossControlMailValidator.IsValid.ShouldBeFalse();
        };
    }
}