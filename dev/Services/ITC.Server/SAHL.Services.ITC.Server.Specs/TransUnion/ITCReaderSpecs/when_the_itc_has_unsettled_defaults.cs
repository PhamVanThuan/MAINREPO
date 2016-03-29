using System;
using System.Globalization;
using System.Linq;
using Machine.Specifications;
using SAHL.Services.Interfaces.ITC.Models;
using SAHL.Services.ITC.Tests.TransUnion;
using SAHL.Services.ITC.TransUnion;

namespace SAHL.Services.ITC.Server.Specs.TransUnion.ITCReaderSpecs
{
    internal class when_the_itc_has_unsettled_defaults
    {
        private static ItcResponse response;
        private static ItcReader reader;
        private static ItcProfile itcProfile;

        private Establish context = () =>
        {
            response = TestITCProvider.GetTestITC("Unsettled defaults of 3 within two years ITC.xml");
        };

        private Because of = () =>
        {
            reader = new ItcReader(response.Response);
            itcProfile = reader.GetITCProfile;
        };

        private It should_contain_the_defaults_in_the_ITC_payment_profile = () =>
        {
            itcProfile.Defaults.Count().ShouldEqual(3);
        };

        private It should_contain_the_details_of_the_default = () =>
        {
            itcProfile.Defaults.First().DefaultAmount.ShouldEqual(377);
            var expectedDate = DateTime.ParseExact("20140407", "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None);
            itcProfile.Defaults.First().InformationDate.ShouldEqual(expectedDate);
        };

        private It should_contain_the_details_of_the_last_default = () =>
        {
            itcProfile.Defaults.Last().DefaultAmount.ShouldEqual(13886);
            var expectedDate = DateTime.ParseExact("20140411", "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None);
            itcProfile.Defaults.Last().InformationDate.ShouldEqual(expectedDate);
        };
    }
}