using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.Capitec.QueryHandlers;
using SAHL.Services.Interfaces.Capitec.Queries;

namespace SAHL.Services.Capitec.Specs.QueryHandlerSpecs.GetNewGuidQueryHandlerSpecs
{
    public class when_getting_a_new_guid : WithFakes
    {
        private static GetNewGuidQueryHandler handler;
        private static GetNewGuidQuery query;

        Establish context = () =>
        {
            handler = new GetNewGuidQueryHandler();
            query = new GetNewGuidQuery();

        };

        Because of = () =>
        {
            handler.HandleQuery(query);
        };

        It should_return_a_new_guid = () =>
        {
            query.Result.NewGuid.ShouldNotBeNull();
        };
    }
}