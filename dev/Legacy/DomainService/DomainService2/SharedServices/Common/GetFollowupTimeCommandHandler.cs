using System;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DomainMessages;

namespace DomainService2.SharedServices.Common
{
    public class GetFollowupTimeCommandHandler : IHandlesDomainServiceCommand<GetFollowupTimeCommand>
    {
        private IMemoReadOnlyRepository memoRepository;

        public GetFollowupTimeCommandHandler(IMemoReadOnlyRepository memoRepository)
        {
            this.memoRepository = memoRepository;
        }

        public void Handle(IDomainMessageCollection messages, GetFollowupTimeCommand command)
        {
            command.Result = DateTime.Now;
            IMemo memo = this.memoRepository.GetMemoByKey(command.MemoKey);
            if (memo != null)
            {
                command.Result = Convert.ToDateTime(memo.ReminderDate);
            }
            else
            {
                messages.Add(new Error("The memo does not have a reminder date set.", ""));
            }
        }
    }
}