using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Exchange;
using SAHL.Services.PollingManager.Server.Tests;
using SAHL.Services.PollingManager.Validators;
using System;
using System.Linq;

namespace SAHL.Services.PollingManager.Server.Specs.Helpers.LossControlMailValidatorSpecs
{
    public class when_validating_an_email_with_an_empty_subject : WithFakes
    {
        private static LossControlMailValidator lossControlMailValidator;
        private static IMailMessage mail;
        private static bool result;

        private Establish context = () =>
        {
            mail = FakeMailMessageFactory.GetMailMessageWithSingleValidAttachmentAndNoSubjectLine();
            result = true;
            lossControlMailValidator = new LossControlMailValidator();
            mail.Subject = null;
        };

        private Because of = () =>
        {
            result = lossControlMailValidator.ValidateMail(mail);
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };

        private It should_set_invalid_to_false = () =>
        {
            lossControlMailValidator.IsValid.ShouldBeFalse();
        };

        private It should_return_the_correct_error_message = () =>
        {
            lossControlMailValidator.Errors.First().ShouldEqual("The email subject is blank.");
        };
    }
}