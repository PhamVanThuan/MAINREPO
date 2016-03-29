using Machine.Specifications;
using SAHL.Services.Interfaces.ITC.Models;
using SAHL.Services.ITC.Tests.TransUnion;
using SAHL.Services.ITC.TransUnion;

namespace SAHL.Services.ITC.Server.Specs.TransUnion.ITCReaderSpecs
{
    internal class when_there_is_no_credit_bureau_match
    {
        private static ItcResponse response;
        private static ItcReader reader;
        private static ItcProfile itcProfile;

        private Establish context = () =>
        {
            response = TestITCProvider.GetTestITC("Header only ITC.xml");
        };

        private Because of = () =>
        {
            reader = new ItcReader(response.Response);
            itcProfile = reader.GetITCProfile;
        };

        private It should_report_that_no_match_was_found = () =>
        {
            itcProfile.CreditBureauMatch.ShouldBeFalse();
        };
    }
}