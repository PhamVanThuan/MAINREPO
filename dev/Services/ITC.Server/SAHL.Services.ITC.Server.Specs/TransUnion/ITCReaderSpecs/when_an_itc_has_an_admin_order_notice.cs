using System;
using System.Globalization;
using System.Linq;
using Machine.Specifications;
using SAHL.Services.Interfaces.ITC.Models;
using SAHL.Services.ITC.Tests.TransUnion;
using SAHL.Services.ITC.TransUnion;

namespace SAHL.Services.ITC.Server.Specs.TransUnion.ITCReaderSpecs
{
    internal class when_an_itc_has_an_admin_order_notice
    {
        private static ItcResponse response;
        private static ItcReader reader;
        private static ItcProfile itcProfile;

        private Establish context = () =>
        {
            response = TestITCProvider.GetTestITC("Admin Order ITC.xml");
        };

        private Because of = () =>
        {
            reader = new ItcReader(response.Response);
            itcProfile = reader.GetITCProfile;
        };

        private It should_return_the_admin_order_notice_type_in_the_ITC_profile = () =>
        {
            itcProfile.Notices.First().NoticeType.ShouldEqual("ADMINISTRATION ORDER");
        };

        private It should_return_the_admin_order_notice_type_code_in_the_ITC_profile = () =>
        {
            itcProfile.Notices.First().NoticeTypeCode.ShouldEqual("A");
        };

        private It should_return_the_admin_order_notice_date_in_the_ITC_profile = () =>
        {
            var expectedDate = DateTime.ParseExact("20050609", "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None);
            itcProfile.Notices.First().NoticeDate.ShouldEqual(expectedDate);
        };
    }
}