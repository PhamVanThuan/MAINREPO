using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.ITC.Queries;
using SAHL.Services.ITC.Managers.Itc;
using SAHL.Services.ITC.QueryHandlers;
using System;

namespace SAHL.Services.ITC.Server.Specs.QueryHandlers.GetCapitecITCProfile
{
    public class when_getting_an_itc_profile_and_it_does_not_exist : WithFakes
    {
        private static GetCapitecITCProfileQuery query;
        private static GetCapitecITCProfileQueryHandler queryHandler;
        private static IItcManager itcManager;
        private static ISystemMessageCollection messages;
        private static Guid itcID;

        private Establish context = () =>
        {
            itcManager = An<IItcManager>();
            itcID = Guid.Parse("{C51CCD85-B49A-4353-8006-34F5B9D827BF}");
            query = new GetCapitecITCProfileQuery(itcID);
            queryHandler = new GetCapitecITCProfileQueryHandler(itcManager);
        };

        private Because of = () =>
        {
            messages = queryHandler.HandleQuery(query);
        };

        private It should_get_an_itc_using_the_itc_id = () =>
        {
            itcManager.WasToldTo(x => x.GetITCProfile(itcID));
        };

        private It should_not_set_the_itc_profile_on_the_query_result = () =>
        {
            query.Result.Results.ShouldBeEmpty();
        };

        private It should_not_return_any_errors = () =>
        {
            messages.HasErrors.ShouldBeFalse();
        };
    }
}