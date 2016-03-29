using System;
using DomainService2.SharedServices.Common;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Specs.SharedServices.Common.GetFollowupTimeCommandHandlerSpecs
{
    [Subject(typeof(GetFollowupTimeCommandHandler))]
    public class When_getfollowuptime : DomainServiceSpec<GetFollowupTimeCommand, GetFollowupTimeCommandHandler>
    {
        static DateTime followupTime = DateTime.Now;

        Establish context = () =>
        {
            IMemoReadOnlyRepository memoRepository = An<IMemoReadOnlyRepository>();
            IMemo memo = An<IMemo>();

            memoRepository.WhenToldTo(x => x.GetMemoByKey(Param.IsAny<int>())).Return(memo);
            followupTime = Convert.ToDateTime(memo.ReminderDate);

            command = new GetFollowupTimeCommand(1111);
            handler = new GetFollowupTimeCommandHandler(memoRepository);
        };
        Because of = () =>
        {
            handler.Handle(messages, command);
        };
        It should_return_followuptime = () =>
        {
            command.Result.ShouldEqual<DateTime>(followupTime);
        };
    }
}