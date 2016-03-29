using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Exchange;
using SAHL.Services.PollingManager.Server.Tests;
using SAHL.Services.PollingManager.Validators;
using System;
using System.Linq;

namespace SAHL.Services.PollingManager.Server.Specs.Helpers.LossControlMailValidatorSpecs
{
    public class when_validating_an_email_with_a_non_numeric_account_number : WithFakes
    {
        private static LossControlMailValidator lossControlManager;
        private static IMailMessage mail;
        private static bool result;

        private Establish context = () =>
        {
            lossControlManager = new LossControlMailValidator();
            mail = FakeMailMessageFactory.GetMailMessageWithSingleValidAttachment(true, upperCaseAttachmentType: false);
            mail.Subject = "m1234 - sf52";
            result = false;
        };

        private Because of = () =>
        {
            result = lossControlManager.ValidateMail(mail);
        };

        private It should_return_an_error_message = () =>
        {
            lossControlManager.Errors.ShouldNotBeEmpty();
        };


        private It should_give_the_correct_error_for_an_invalid_subject = () =>
        {
            lossControlManager.Errors.ShouldContain("The email subject is in the incorrect format.");
        };
    }
}