using System;
using DomainService2.SharedServices.Common;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Specs.SharedServices.Common.GetFollowupTimeCommandHandlerSpecs
{
    [Subject(typeof(GetFollowupTimeCommandHandler))]
    public class When_getfollowuptime_has_no_reminder_date : DomainServiceSpec<GetFollowupTimeCommand, GetFollowupTimeCommandHandler>
    {
        static DateTime followupTime = DateTime.Now;

        Establish context = () =>
        {
            IMemoReadOnlyRepository memoRepository = An<IMemoReadOnlyRepository>();

            command = new GetFollowupTimeCommand(Param<int>.IsAnything);
            handler = new GetFollowupTimeCommandHandler(memoRepository);
        };
        Because of = () =>
        {
            handler.Handle(messages, command);
        };
        It should_return_followuptime = () =>
        {
            messages.Count.ShouldBeGreaterThan(0);
        };
    }
}