using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Exchange;
using SAHL.Services.PollingManager.Server.Tests;
using SAHL.Services.PollingManager.Validators;
using System;
using System.Linq;

namespace SAHL.Services.PollingManager.Server.Specs.Helpers.LossControlMailValidatorSpecs
{
    public class when_validating_an_email_with_a_single_valid_pdf_attachment : WithFakes
    {
        private static bool result;
        private static LossControlMailValidator lossControlMailValidator;
        private static IMailMessage mail;

        private Establish context = () =>
        {
            lossControlMailValidator = new LossControlMailValidator();
            mail = FakeMailMessageFactory.GetMailMessageWithSingleValidAttachment(true, false);
            result = false;
        };

        private Because of = () =>
        {
            result = lossControlMailValidator.ValidateMail(mail);
        };

        private It should_be_true = () =>
        {
            result.ShouldBeTrue();
        };

        private It should_not_throw_any_exceptions = () =>
        {
            lossControlMailValidator.Errors.ShouldBeEmpty();
        };

        private It should_change_IsValid_to_true = () =>
        {
            lossControlMailValidator.IsValid.ShouldBeTrue();
        };
    }
}