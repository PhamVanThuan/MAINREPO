using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.SharedServices.Common
{
    public class DoCreditScoreCommandHandler : IHandlesDomainServiceCommand<DoCreditScoreCommand>
    {
        ICreditScoringRepository creditScoringRepository;
        IApplicationRepository applicationRepository;

        public DoCreditScoreCommandHandler(ICreditScoringRepository creditScoringRepository, IApplicationRepository applicationRepository)
        {
            this.creditScoringRepository = creditScoringRepository;
            this.applicationRepository = applicationRepository;
        }

        public void Handle(IDomainMessageCollection messages, DoCreditScoreCommand command)
        {
            bool success = false;

            IApplication app = applicationRepository.GetApplicationByKey(command.ApplicationKey);
            ICallingContext callingContext = creditScoringRepository.GetCallingContextByKey(command.CallingContextKey);
            IApplicationCreditScore applicationCreditScore = creditScoringRepository.GenerateApplicationCreditScore(app, callingContext, command.ADUserName);
            if (applicationCreditScore != null)
                success = true;

            command.Result = success;
        }
    }
}