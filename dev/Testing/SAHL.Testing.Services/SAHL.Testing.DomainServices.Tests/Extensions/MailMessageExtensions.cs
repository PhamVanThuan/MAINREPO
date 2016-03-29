using NUnit.Framework;
using SAHL.Core.Exchange;

namespace SAHL.Testing.Services.Tests.Extensions
{
    public static class MailMessageExtensions
    {
        public static void AssertBodyTextContains(this IMailMessage T, string expectedResult)
        {
            Assert.IsNotNull(T, @"No mail message was found");
            StringAssert.Contains(expectedResult, T.Body, string.Format(@"The mail message body does not contain the text '{0}'", expectedResult));
        }
    }
}