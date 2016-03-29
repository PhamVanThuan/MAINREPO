using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.LifeDomain.Models;
using SAHL.Services.Interfaces.LifeDomain.Queries;
using SAHL.Services.LifeDomain.Managers;
using SAHL.Services.LifeDomain.QueryHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Machine.Fakes;
using NSubstitute;

namespace SAHL.Services.LifeDomain.Specs.QueryHandlersSpecs
{
    public class when_getting_disability_claim_status_description : WithCoreFakes
    {
        private static ILifeDomainDataManager lifeDomainDataManager;
        private static IEnumerable<string> result;
        private static GetDisabilityClaimStatusDescriptionQuery query;
        private static GetDisabilityClaimStatusDescriptionQueryHandler handler;
        private static int disabilityClaimKey;
        private static string status;

        private Establish context = () =>
        {
            disabilityClaimKey = 999555;
            status = "Pending";
            lifeDomainDataManager = An<ILifeDomainDataManager>();
            result = new List<string>() { status };
            lifeDomainDataManager.WhenToldTo(x => x.GetDisabilityClaimStatusDescription(Arg.Any<int>())).Return(status);
            query = new GetDisabilityClaimStatusDescriptionQuery(disabilityClaimKey);
            handler = new GetDisabilityClaimStatusDescriptionQueryHandler(lifeDomainDataManager);
        };

        private Because of = () =>
        {
            messages = handler.HandleQuery(query);
        };

        private It should_not_return_any_messages = () =>
        {
            messages.AllMessages.ShouldBeEmpty();
        };

        private It should_get_the_disability_claims_by_account = () =>
        {
            lifeDomainDataManager.WasToldTo(x => x.GetDisabilityClaimStatusDescription(disabilityClaimKey));
        };

        private It should_return_the_status_in_the_query_results = () =>
        {
            query.Result.Results.First().ShouldEqual(status);
        };
    }
}