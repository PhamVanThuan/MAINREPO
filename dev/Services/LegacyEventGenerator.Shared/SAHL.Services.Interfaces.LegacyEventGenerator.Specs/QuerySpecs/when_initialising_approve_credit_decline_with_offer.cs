using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.Interfaces.LegacyEventGenerator.Queries;
using System;
using System.Linq;

namespace SAHL.Services.Interfaces.LegacyEventGenerator.Specs.QuerySpecs
{
    public class when_initialising_approve_credit_decline_with_offer : WithFakes
    {
        private static int sdsdgKey;
        private static CreateApproveApplicationCreditApproveDeclinedWithOfferLegacyEventQuery query;

        private Establish context = () =>
            {
                query = new CreateApproveApplicationCreditApproveDeclinedWithOfferLegacyEventQuery();
                sdsdgKey = 2623;
            };

        private Because of = () =>
            {
                query.Initialise(1234, 1, 1, DateTime.Now, 1234, @"SAHL\HaloUser");
            };

        private It should_set_the_expected_sdsdgKey_property = () =>
            {
                query.StageDefinitionStageDefinitionGroupKey.ShouldEqual(sdsdgKey);
            };
    }
}