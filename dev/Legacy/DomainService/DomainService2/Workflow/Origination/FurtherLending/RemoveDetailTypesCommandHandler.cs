using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Workflow.Origination.FurtherLending
{
    public class RemoveDetailTypesCommandHandler : IHandlesDomainServiceCommand<RemoveDetailTypesCommand>
    {
        private IApplicationRepository applicationRepository;
        private IAccountRepository accountRepository;

        public RemoveDetailTypesCommandHandler(IApplicationRepository applicationRepository, IAccountRepository accountRepository)
        {
            this.applicationRepository = applicationRepository;
            this.accountRepository = accountRepository;
        }

        public void Handle(IDomainMessageCollection messages, RemoveDetailTypesCommand command)
        {
            IApplication app = this.applicationRepository.GetApplicationByKey(command.ApplicationKey);
            List<IDetail> ToRemove = new List<IDetail>();
            if (app.ApplicationType.Key == (int)SAHL.Common.Globals.OfferTypes.FurtherLoan) // 4
            {
                foreach (IDetail d in app.Account.Details)
                {
                    if (d.DetailType.Key == (int)SAHL.Common.Globals.DetailTypes.FurtherLoanInProgress) // 457
                    {
                        ToRemove.Add(d);
                    }

                    if (d.DetailType.Key == (int)SAHL.Common.Globals.DetailTypes.ClientWantsToNTU) // 412
                    {
                        ToRemove.Add(d);
                    }
                }
            }
            else if ((app.ApplicationType.Key == (int)SAHL.Common.Globals.OfferTypes.FurtherAdvance /*3*/) || (app.ApplicationType.Key == (int)SAHL.Common.Globals.OfferTypes.ReAdvance /*2*/))
            {
                foreach (IDetail d in app.Account.Details)
                {
                    if (d.DetailType.Key == (int)SAHL.Common.Globals.DetailTypes.ReadvanceInProgress/*456*/ && d.Description == command.ApplicationKey.ToString())
                    {
                        ToRemove.Add(d);
                    }
                    if (d.DetailType.Key == (int)SAHL.Common.Globals.DetailTypes.ClientWantsToNTU) // 412
                    {
                        ToRemove.Add(d);
                    }
                }
            }
            foreach (IDetail d in ToRemove)
            {
                app.Account.Details.Remove(messages, d);
            }

            if (ToRemove.Count > 0)
            {
                this.accountRepository.SaveAccount(app.Account);
            }
        }
    }
}