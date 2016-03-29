using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.LifeDomain.Queries;
using SAHL.Services.LifeDomain.Managers;
using SAHL.Services.LifeDomain.QueryHandlers;

namespace SAHL.Services.LifeDomain.Specs.QueryHandlersSpecs
{
    public class when_getting_disability_claim_instance_subject : WithCoreFakes
    {
        private static ILifeDomainDataManager lifeDomainDataManager;
        private static GetDisabilityClaimInstanceSubjectQuery query;
        private static GetDisabilityClaimInstanceSubjectQueryHandler handler;

        private Establish context = () =>
        {
            lifeDomainDataManager = An<ILifeDomainDataManager>();

            query = new GetDisabilityClaimInstanceSubjectQuery(0);
            handler = new GetDisabilityClaimInstanceSubjectQueryHandler(lifeDomainDataManager);
        };

        private Because of = () =>
        {
            messages = handler.HandleQuery(query);
        };

        private It should_not_return_any_messages = () =>
        {
            messages.AllMessages.ShouldBeEmpty();
        };

        private It should_get_the_disability_claim_instance_subject = () =>
        {
            lifeDomainDataManager.WasToldTo(x => x.GetDisabilityClaimInstanceSubject(Param.IsAny<int>()));
        };
    }
}