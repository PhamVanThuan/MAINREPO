using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.LifeDomain.Models;
using SAHL.Services.Interfaces.LifeDomain.Queries;
using SAHL.Services.LifeDomain.Managers;
using SAHL.Services.LifeDomain.QueryHandlers;
using System;
using System.Collections.Generic;

namespace SAHL.Services.LifeDomain.Specs.QueryHandlersSpecs
{
    public class when_getting_a_disability_claim_by_key : WithCoreFakes
    {
        private static ILifeDomainDataManager lifeDomainDataManager;
        private static IEnumerable<DisabilityClaimDetailModel> result;
        private static GetDisabilityClaimByKeyQuery query;
        private static GetDisabilityClaimByKeyQueryHandler handler;

        private Establish context = () =>
        {
            lifeDomainDataManager = An<ILifeDomainDataManager>();
            result = new List<DisabilityClaimDetailModel>() { new DisabilityClaimDetailModel(0, 0, 0, 0, null
                                                                  , DateTime.Now, null, null, null, null, null, null, null
                                                                  , (int)DisabilityClaimStatus.Pending, "Pending", null, null, null
                                                             )};

            query = new GetDisabilityClaimByKeyQuery(0);
            handler = new GetDisabilityClaimByKeyQueryHandler(lifeDomainDataManager);
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
            lifeDomainDataManager.WasToldTo(x => x.GetDisabilityClaimDetailByKey(Param.IsAny<int>()));
        };
    }
}