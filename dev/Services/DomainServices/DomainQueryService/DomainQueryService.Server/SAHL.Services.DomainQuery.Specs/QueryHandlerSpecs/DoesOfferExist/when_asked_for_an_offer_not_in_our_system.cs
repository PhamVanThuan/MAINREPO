using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.Services.DomainQuery.Managers.Application;
using SAHL.Services.DomainQuery.QueryHandlers;
using SAHL.Services.Interfaces.DomainQuery.Queries;
using System.Linq;

namespace SAHL.Services.DomainQuery.Specs.QueryHandlerSpecs.DoesOfferExist
{
    public class when_asked_for_an_offer_not_in_our_system : WithFakes
    {
        private static DoesOfferExistQueryHandler handler;
        private static DoesOfferExistQuery query;
        private static IApplicationDataManager dataManager;
        private static ISystemMessageCollection messages;
        private static int offerKey;

        private Establish context = () =>
        {
            dataManager = An<IApplicationDataManager>();
            offerKey = 42;

            handler = new DoesOfferExistQueryHandler(dataManager);
            query = new DoesOfferExistQuery(offerKey);

            dataManager.WhenToldTo(x => x.DoesOfferExist(offerKey)).Return(false);
        };

        private Because of = () =>
        {
            messages = handler.HandleQuery(query);
        };

        private It should_check_if_the_offer_exists = () =>
        {
            dataManager.WasToldTo(x => x.DoesOfferExist(query.OfferKey));
        };

        private It should_set_the_query_result = () =>
        {
            query.Result.ShouldNotBeNull();
            query.Result.Results.First().OfferExist.ShouldBeFalse();
        };

        private It should_not_return_any_messages = () =>
        {
            messages.AllMessages.ShouldBeEmpty();
        };
    }
}