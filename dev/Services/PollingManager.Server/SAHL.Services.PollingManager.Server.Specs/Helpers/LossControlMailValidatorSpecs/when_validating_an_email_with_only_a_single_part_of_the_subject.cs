using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Exchange;
using SAHL.Services.PollingManager.Server.Tests;
using SAHL.Services.PollingManager.Validators;
using System;
using System.Linq;

namespace SAHL.Services.PollingManager.Server.Specs.Helpers.LossControlMailValidatorSpecs
{
    public class when_validating_an_email_with_only_a_single_part_of_the_subject : WithFakes
    {
        private static IMailMessage mail;
        private static LossControlMailValidator lossControlMailValidator;
        private static bool result;
        private static Exception error;

        private Establish context = () =>
        {
            lossControlMailValidator = new LossControlMailValidator();
            mail = FakeMailMessageFactory.GetMailMessageWithSingleValidAttachmentAndIncorrectSubjectLineFormat();
            error = new Exception();
            result = true;
        };

        private Because of = () =>
        {
            error = Catch.Exception(() => result = lossControlMailValidator.ValidateMail(mail));
        };

        private It should_return_false_if_subject_is_partial = () =>
        {
            result.ShouldBeFalse();
        };

        private It should_return_the_correct_error_message = () =>
        {
            lossControlMailValidator.Errors.First().ShouldEqual("The email subject is in the incorrect format.");
        };
    }
}