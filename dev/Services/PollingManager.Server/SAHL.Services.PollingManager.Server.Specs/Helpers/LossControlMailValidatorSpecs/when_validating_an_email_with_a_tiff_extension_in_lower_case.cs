using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Exchange;
using SAHL.Services.PollingManager.Server.Tests;
using SAHL.Services.PollingManager.Validators;
using System;
using System.Linq;

namespace SAHL.Services.PollingManager.Server.Specs.Helpers.LossControlMailValidatorSpecs
{
    public class when_validating_an_email_with_a_tiff_extension_in_lower_case : WithFakes
    {
        private static LossControlMailValidator lossControlManager;
        private static IMailMessage mail;
        private static bool result;

        private Establish context = () =>
        {
            lossControlManager = new LossControlMailValidator();
            mail = FakeMailMessageFactory.GetMailMessageWithSingleValidAttachment(pdf: false, upperCaseAttachmentType: false);
            result = false;
        };

        private Because of = () =>
        {
            result = lossControlManager.ValidateMail(mail);
        };

        private It should_not_have_errors = () =>
        {
            lossControlManager.Errors.ShouldBeEmpty();
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}