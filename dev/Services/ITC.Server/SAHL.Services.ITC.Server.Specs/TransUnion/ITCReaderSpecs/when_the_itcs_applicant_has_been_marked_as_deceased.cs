using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;
using SAHL.Services.Interfaces.ITC.Models;
using SAHL.Services.ITC.Tests.TransUnion;
using SAHL.Services.ITC.TransUnion;

namespace SAHL.Services.ITC.Server.Specs.TransUnion.ITCReaderSpecs
{
    internal class when_the_itcs_applicant_has_been_marked_as_deceased
    {
        private static ItcResponse response;
        private static ItcReader reader;
        private static ItcProfile itcProfile;
        private static IEnumerable<ItcPaymentProfile> paymentProfiles;

        private Establish context = () =>
        {
            response = TestITCProvider.GetTestITC("Code Z - Consumer is deceased ITC.xml");
        };

        private Because of = () =>
        {
            reader = new ItcReader(response.Response);
            itcProfile = reader.GetITCProfile;
            paymentProfiles = itcProfile.PaymentProfiles;
        };

        private It should_contain_a_deceased_code_in_the_payment_history = () =>
        {
            paymentProfiles.Any(x => x.PaymentHistories.Any(y => y.StatusCode == "Z")).ShouldBeTrue();
        };
    }
}