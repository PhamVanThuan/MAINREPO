using System;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;

namespace DomainService2.SharedServices.Common
{
    public class AddDetailTypeCommandHandler : IHandlesDomainServiceCommand<AddDetailTypeCommand>
    {
        private IApplicationRepository applicationRepository;
        private IAccountRepository accountRepository;
        private ILookupRepository lookupRepository;

        public AddDetailTypeCommandHandler(IApplicationRepository applicationRepository, IAccountRepository accountRepository,
                                           ILookupRepository lookupRepository)
        {
            this.applicationRepository = applicationRepository;
            this.accountRepository = accountRepository;
            this.lookupRepository = lookupRepository;
        }

        public void Handle(IDomainMessageCollection messages, AddDetailTypeCommand command)
        {
            command.Result = false;
            IApplication application = applicationRepository.GetApplicationByKey(command.ApplicationKey);
            if (application.ApplicationType.Key == (int)OfferTypes.FurtherAdvance)
            {
                int detailTypeKey = accountRepository.GetDetailTypeKeyByDescription(command.DetailType);
                IReadOnlyEventList<IDetail> detailList = accountRepository.GetDetailByAccountKeyAndDetailType(application.Account.Key, detailTypeKey);

                // make sure the detailType doesn't already exist on this account.
                if (detailList.Count == 0)
                {
                    IDetail detail = accountRepository.CreateEmptyDetail();
                    detail.Account = application.Account;
                    detail.ChangeDate = DateTime.Now;
                    detail.Description = command.DetailType;
                    detail.DetailDate = DateTime.Now;
                    detail.DetailType = lookupRepository.DetailTypes.ObjectDictionary[detailTypeKey.ToString()];
                    detail.UserID = command.ADUser;
                    detail.LinkID = command.ApplicationKey;
                    accountRepository.SaveDetail(detail);

                    command.Result = true;
                }
            }
        }
    }
}