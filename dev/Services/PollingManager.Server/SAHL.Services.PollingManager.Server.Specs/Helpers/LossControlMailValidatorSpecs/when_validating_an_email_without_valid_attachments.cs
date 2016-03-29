using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Exchange;
using SAHL.Services.PollingManager.Server.Tests;
using SAHL.Services.PollingManager.Validators;
using System;
using System.Linq;

namespace SAHL.Services.PollingManager.Server.Specs.Helpers.LossControlMailValidatorSpecs
{
    public class when_validating_an_email_without_valid_attachments : WithFakes
    {
        private static LossControlMailValidator lossControlMailValidator;
        private static IMailMessage mail;
        private static bool result;

        private Establish context = () =>
        {
            mail = FakeMailMessageFactory.GetMailMessageWithInValidAttachments();
            lossControlMailValidator = new LossControlMailValidator();
            result = true;
        };

        private Because of = () =>
        {
            result = lossControlMailValidator.ValidateMail(mail);
        };

        private It should_change_isValid_to_false = () =>
        {
            lossControlMailValidator.IsValid.ShouldEqual(false);
        };

        private It should_give_the_correct_error = () =>
        {
            lossControlMailValidator.Errors.ShouldContain("The email did not contain an attachment in either PDF or TIFF format.");
        };
    }
}