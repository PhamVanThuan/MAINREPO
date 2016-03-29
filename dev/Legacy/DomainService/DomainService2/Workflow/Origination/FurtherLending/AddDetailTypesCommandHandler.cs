using System;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;

namespace DomainService2.Workflow.Origination.FurtherLending
{
    public class AddDetailTypesCommandHandler : IHandlesDomainServiceCommand<AddDetailTypesCommand>
    {
        private IApplicationRepository applicationRepository;
        private ILookupRepository lookupRepository;
        private IAccountRepository accountRepository;

        public AddDetailTypesCommandHandler(IApplicationRepository applicationRepository, ILookupRepository lookupRepository,
                                            IAccountRepository accountRepository)
        {
            this.applicationRepository = applicationRepository;
            this.lookupRepository = lookupRepository;
            this.accountRepository = accountRepository;
        }

        public void Handle(IDomainMessageCollection messages, AddDetailTypesCommand command)
        {
            IApplication application = this.applicationRepository.GetApplicationByKey(command.ApplicationKey);
            if (application.ApplicationType.Key == (int)OfferTypes.FurtherLoan)
            {
                IDetail detail = this.accountRepository.CreateEmptyDetail();
                detail.Account = application.Account;
                detail.Amount = ((IApplicationMortgageLoan)application).LoanAgreementAmount.Value;
                detail.ChangeDate = DateTime.Now;
                detail.Description = command.ApplicationKey.ToString();
                detail.DetailDate = DateTime.Now;
                detail.DetailType = this.lookupRepository.DetailTypes.ObjectDictionary[Convert.ToString((int)DetailTypes.FurtherLoanInProgress)];
                detail.UserID = command.ADUser;
                detail.LinkID = command.ApplicationKey;
                accountRepository.SaveDetail(detail);
            }
            else if ((application.ApplicationType.Key == (int)OfferTypes.ReAdvance) || application.ApplicationType.Key == (int)OfferTypes.FurtherAdvance)
            {
                IDetail detail = this.accountRepository.CreateEmptyDetail();
                detail.Account = application.Account;
                detail.Amount = ((IApplicationMortgageLoan)application).LoanAgreementAmount.Value;
                detail.ChangeDate = DateTime.Now;
                detail.Description = command.ApplicationKey.ToString();
                detail.DetailDate = DateTime.Now;
                detail.DetailType = this.lookupRepository.DetailTypes.ObjectDictionary[Convert.ToString((int)DetailTypes.ReadvanceInProgress)];
                detail.UserID = command.ADUser;
                detail.LinkID = command.ApplicationKey;
                accountRepository.SaveDetail(detail);
            }
        }
    }
}