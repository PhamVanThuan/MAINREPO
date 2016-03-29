using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.LifeDomain.Models;
using SAHL.Services.Interfaces.LifeDomain.Queries;
using SAHL.Services.LifeDomain.Managers;
using SAHL.Services.LifeDomain.QueryHandlers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.LifeDomain.Specs.QueryHandlersSpecs
{
    public class when_getting_pending_claims_for_an_account_with_no_pending : WithCoreFakes
    {
        private static ILifeDomainDataManager lifeDomainDataManager;
        private static IEnumerable<DisabilityClaimModel> disabilityClaims;
        private static GetPendingDisabilityClaimByAccountQuery query;
        private static GetPendingDisabilityClaimByAccountQueryHandler handler;

        private Establish context = () =>
        {
            disabilityClaims = new List<DisabilityClaimModel>() { new DisabilityClaimModel(0, 0, 0, DateTime.Now, null, null, null, null, null, null, (int)DisabilityClaimStatus.Approved,
                null, null, null) };
            lifeDomainDataManager = An<ILifeDomainDataManager>();
            lifeDomainDataManager.WhenToldTo(x => x.GetDisabilityClaimsByAccount(Arg.Any<int>()))
                .Return(disabilityClaims);
            query = new GetPendingDisabilityClaimByAccountQuery(0);
            handler = new GetPendingDisabilityClaimByAccountQueryHandler(lifeDomainDataManager);
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
            lifeDomainDataManager.WasToldTo(x => x.GetDisabilityClaimsByAccount(Param.IsAny<int>()));
        };

        private It should_return_an_empty_collection = () =>
        {
            query.Result.Results.First().ShouldBeNull();
        };
    }
}