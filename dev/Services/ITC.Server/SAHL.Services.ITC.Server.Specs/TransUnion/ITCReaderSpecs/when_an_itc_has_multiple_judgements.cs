using System;
using System.Globalization;
using System.Linq;
using Machine.Specifications;
using SAHL.Services.Interfaces.ITC.Models;
using SAHL.Services.ITC.Tests.TransUnion;
using SAHL.Services.ITC.TransUnion;

namespace SAHL.Services.ITC.Server.Specs.TransUnion.ITCReaderSpecs
{
    internal class when_an_itc_has_multiple_judgements
    {
        private static ItcResponse response;
        private static ItcReader reader;
        private static ItcProfile itcProfile;

        private Establish context = () =>
            {
                response = TestITCProvider.GetTestITC("Judgements 2 and value 10 000 ITC.xml");
            };

        private Because of = () =>
            {
                reader = new ItcReader(response.Response);
                itcProfile = reader.GetITCProfile;
            };

        private It should_return_both_judgements = () =>
            {
                itcProfile.Judgments.Count().ShouldEqual(2);
            };

        private It should_return_information_for_the_first_judgement = () =>
            {
                itcProfile.Judgments.First().Amount.ShouldEqual(10000);
                var expectedDate = DateTime.ParseExact(DateTime.Now.ToString("yyyyMMdd"), "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None);
                itcProfile.Judgments.First().JudgmentDate.ShouldEqual(expectedDate);
            };

        private It should_return_information_for_the_second_judgement = () =>
            {
                itcProfile.Judgments.Last().Amount.ShouldEqual(10000);
                var expectedDate = DateTime.ParseExact(DateTime.Now.AddDays(1).AddYears(-3).ToString("yyyyMMdd"), "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None);
                itcProfile.Judgments.Last().JudgmentDate.ShouldEqual(expectedDate);
            };
    }
}