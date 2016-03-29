using Machine.Specifications;
using SAHL.Services.Interfaces.ITC.Models;
using SAHL.Services.ITC.Tests.TransUnion;
using SAHL.Services.ITC.TransUnion;

namespace SAHL.Services.ITC.Server.Specs.TransUnion.ITCReaderSpecs
{
    internal class when_an_itc_has_a_valid_empirica_score
    {
        private static ItcResponse response;
        private static ItcReader reader;
        private static ItcProfile itcProfile;

        private Establish context = () =>
        {
            response = TestITCProvider.GetTestITC("default_response.xml");
        };

        private Because of = () =>
        {
            reader = new ItcReader(response.Response);
            itcProfile = reader.GetITCProfile;
        };

        private It should_contain_the_empirica_score = () =>
        {
            itcProfile.EmpericaScore.ShouldEqual(650);
        };

        private It should_report_that_a_match_was_found = () =>
        {
            itcProfile.CreditBureauMatch.ShouldBeTrue();
        };
    }
}