using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.ITC.Queries;
using SAHL.Services.ITC.Managers.Itc;
using SAHL.Services.ITC.QueryHandlers;

namespace SAHL.Services.ITC.Server.Specs.QueryHandlers.GetCurrentITCForLegalEntity
{
    public class when_getting_current_itc_and_one_does_not_exist : WithFakes
    {
        private static IItcManager itcManager;
        private static GetCurrentITCForLegalEntityQueryHandler handler;
        private static GetCurrentITCForLegalEntityQuery query;
        private static ISystemMessageCollection messages;

        private static string identityNumber;

        private Establish context = () => {
            itcManager = An<IItcManager>();
            handler = new GetCurrentITCForLegalEntityQueryHandler(itcManager);

            identityNumber = "995544668877115";
            query = new GetCurrentITCForLegalEntityQuery(identityNumber);
        };

        private Because of = () =>
        {
            messages = handler.HandleQuery(query);
        };

        private It should_get_the_current_itc_for_the_legal_entity = () =>
        {
            itcManager.WasToldTo(x => x.GetCurrentItcProfileForLegalEntity(identityNumber));
        };

        private It should_return_no_results = () =>
        {
            query.Result.Results.ShouldBeEmpty();
        };

        private It should_not_return_any_errors = () =>
        {
            messages.HasErrors.ShouldBeFalse();
        };
    }
}