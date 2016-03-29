using BuildingBlocks.Services.Contracts;
using NUnit.Framework;

namespace BuildingBlocks.Assertions
{
    /// <summary>
    /// Contains assertions for the ClientEmail table
    /// </summary>
    public static class ClientEmailAssertions
    {
        private static readonly IClientEmailService clientEmailService;

        static ClientEmailAssertions()
        {
            clientEmailService = ServiceLocator.Instance.GetService<IClientEmailService>();
        }

        /// <summary>
        /// Checks that a Client Email record exists with the provided email address and email subject with an insert date greater than the date provided.
        /// </summary>
        /// <param name="emailTo">Email Address</param>
        /// <param name="emailSubject">Subject of the Email</param>
        /// <param name="date">Date Filter for Insert Date</param>
        public static void AssertClientEmailRecordWithSubjectAndToAddressRecordExists(string emailTo, string emailSubject, string date)
        {
            var r = clientEmailService.GetClientEmailByToAddressAndSubject(emailTo, emailSubject, date);
            if (!r.HasResults)
                Assert.Fail(string.Format(@"Client Email record with Subject: {0}, EmailTo: {1} with an Insert Date > than {2} was not found.",
                    emailSubject, emailTo, date));
        }

        /// <summary>
        /// Checks that a client SMS record has
        /// </summary>
        /// <param name="smsBody"></param>
        /// <param name="cellphoneNumber"></param>
        /// <param name="genericKey"></param>
        public static void AssertClientEmailSMS(string smsBody, string cellphoneNumber, int genericKey)
        {
            var results = clientEmailService.GetClientEmailSMS(smsBody, cellphoneNumber, genericKey);
            if (!results.HasResults)
                Assert.Fail(string.Format(@"No SMS record found for number {0} with body {1}", cellphoneNumber, smsBody));
        }

        public static void AssertNoClientEmailRecordExists(string emailTo, string emailSubject, string date)
        {
            var r = clientEmailService.GetClientEmailByToAddressAndSubject(emailTo, emailSubject, date);
            if (r.HasResults)
                Assert.Fail(string.Format(@"Client Email record with Subject:{0}, EmailTo:{1} with an Insert Date > than {2} was found.",
                    emailSubject, emailTo, date));
        }
    }
}