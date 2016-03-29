using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Workflow.PersonalLoan
{
    public class GetInstanceSubjectForPersonalLoanCommandHandler : IHandlesDomainServiceCommand<GetInstanceSubjectForPersonalLoanCommand>
    {
        private IX2Repository x2Repository;

        public GetInstanceSubjectForPersonalLoanCommandHandler(IX2Repository x2Repository)
        {
            this.x2Repository = x2Repository;
        }

        public void Handle(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, GetInstanceSubjectForPersonalLoanCommand command)
        {
            command.Result = x2Repository.GetPersonalLoansInstanceSubject(command.ApplicationKey);
        }
    }
}