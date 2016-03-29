using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Exchange;
using SAHL.Services.PollingManager.Server.Tests;
using SAHL.Services.PollingManager.Validators;
using System;
using System.Linq;

namespace SAHL.Services.PollingManager.Server.Specs.Helpers.LossControlMailValidatorSpecs
{
    public class when_validating_an_email_and_more_than_one_requirement_is_not_met : WithFakes
    {
        private static bool result;
        private static LossControlMailValidator lossControlMailValidator;
        private static IMailMessage mail;

        private Establish context = () =>
        {
            result = true;
            lossControlMailValidator = new LossControlMailValidator();
            mail = FakeMailMessageFactory.GetMailMessageWithThreeAttachments();
            mail.Subject = "1408282-";
        };

        private Because of = () =>
        {
            result = lossControlMailValidator.ValidateMail(mail);
        };

        private It should_return_false_if_the_subject_is_invalid = () =>
        {
            result.ShouldBeFalse();
        };

        private It should_give_the_correct_error_for_more_than_one_attachment = () =>
        {
            lossControlMailValidator.Errors.ShouldContain("The email contained more than one attachment in either PDF or TIFF format.");
        };

        private It should_give_the_correct_error_for_an_invalid_subject = () =>
        {
            lossControlMailValidator.Errors.ShouldContain("The email subject is in the incorrect format.");
        };
    }
}