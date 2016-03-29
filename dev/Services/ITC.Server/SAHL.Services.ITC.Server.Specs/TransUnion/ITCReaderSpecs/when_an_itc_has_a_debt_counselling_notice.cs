using System;
using System.Globalization;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.Interfaces.ITC.Models;
using SAHL.Services.ITC.Tests.TransUnion;
using SAHL.Services.ITC.TransUnion;

namespace SAHL.Services.ITC.Server.Specs.TransUnion.ITCReaderSpecs
{
    public class when_an_itc_has_a_debt_counselling_notice
    {
        private static ItcResponse response;
        private static ItcReader reader;
        private static ItcProfile itcProfile;

        private Establish context = () =>
        {
            response = TestITCProvider.GetTestITC("Debt Counselling ITC.xml");
        };

        private Because of = () =>
        {
            reader = new ItcReader(response.Response);
            itcProfile = reader.GetITCProfile;
        };

        private It should_return_the_debt_counselling_code_in_the_ITC_profile = () =>
        {
            itcProfile.DebtCounselling.Code.ShouldEqual("DAP");
        };

        private It should_return_the_debt_counselling_date_in_the_ITC_profile = () =>
        {
            var expectedDate = DateTime.ParseExact("20131018", "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None);
            itcProfile.DebtCounselling.Date.ShouldEqual(expectedDate);
        };
    }
}