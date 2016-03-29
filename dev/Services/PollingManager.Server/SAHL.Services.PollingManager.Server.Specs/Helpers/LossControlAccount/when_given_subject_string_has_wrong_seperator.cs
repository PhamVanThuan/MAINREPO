using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.PollingManager.Helpers;
using System;
using System.Collections.Generic;

namespace SAHL.Services.ExchangeManager.Server.Specs.HelpersLossContolAccount
{
    public class when_given_subject_string_has_wrong_seperator : WithFakes
    {
        private static string emailSubject;
        private static char seperator;
        private static LossControlAccountHelper lossControlAccountHelper;
        private static Tuple<int, List<string>> accountErrorsPair;
        private static int expectedAccountNumber, actualAccountNumber;
        private static List<string> accountNumberErrors;

        private Establish context = () =>
        {
            expectedAccountNumber = 12345;
            emailSubject = string.Format("{0}:788888 This is some string", expectedAccountNumber);
            seperator = '-';
            lossControlAccountHelper = new LossControlAccountHelper();
        };

        private Because of = () =>
        {
            accountErrorsPair = lossControlAccountHelper.StripAccountNumber(seperator, emailSubject);
            actualAccountNumber = accountErrorsPair.Item1;
            accountNumberErrors = accountErrorsPair.Item2;
        };

        private It should_fail_to_extract_the_account_number = () =>
        {
            actualAccountNumber.ShouldEqual(0);
        };

        private It should_add_account_error_message = () =>
        {
            accountNumberErrors.Count.ShouldBeGreaterThan(0);
        };
    }
}