using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.PollingManager.Helpers;
using System;
using System.Collections.Generic;

namespace SAHL.Services.ExchangeManager.Server.Specs.Helpers.LossContolAccount
{
    public class when_given_properly_formated_subject_string : WithFakes
    {
        private static string emailSubject;
        private static char seperator;
        private static LossControlAccountHelper lossControlAccountHelper;
        private static Tuple<int, List<string>> accountErrorsPair;
        private static int expectedAccountNumber, actualAccountNumber;
        private static List<string> accountNumberErrors;

        private Establish context = () =>
        {
            expectedAccountNumber = 1408282;
            emailSubject = string.Format("{0}-Inv12345", expectedAccountNumber);
            seperator = '-';
            lossControlAccountHelper = new LossControlAccountHelper();
        };

        private Because of = () =>
        {
            accountErrorsPair = lossControlAccountHelper.StripAccountNumber(seperator, emailSubject);
            actualAccountNumber = accountErrorsPair.Item1;
            accountNumberErrors = accountErrorsPair.Item2;
        };

        private It should_get_correct_account_number = () =>
        {
            actualAccountNumber.ShouldEqual(expectedAccountNumber);
        };

        private It should_not_add_account_error_message = () =>
        {
            accountNumberErrors.Count.ShouldEqual(0);
        };
    }
}