using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.SharedServices.Common
{
    public class GetCaseNameCommandHandler : IHandlesDomainServiceCommand<GetCaseNameCommand>
    {
        private IApplicationReadOnlyRepository applicationReadOnlyRepository;

        public GetCaseNameCommandHandler(IApplicationReadOnlyRepository applicationReadOnlyRepository)
        {
            this.applicationReadOnlyRepository = applicationReadOnlyRepository;
        }

        public void Handle(IDomainMessageCollection messages, GetCaseNameCommand command)
        {
            // use the readonly repository as the legal name is based on uncommited
            IApplication app = this.applicationReadOnlyRepository.GetApplicationByKey(command.ApplicationKey);
            command.CaseNameResult = app.GetLegalName(LegalNameFormat.Full);
        }
    }
}